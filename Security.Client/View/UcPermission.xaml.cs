using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Security.Client.SecurityServer;
using Security.Client.Common;
using Security.Client.ViewModel;


namespace Security.Client.View
{
    public partial class UcPermission : UserControl
    {
        private SecurityServiceLink serviceLink = new SecurityServiceLink();
        private ObservableCollection<RoleModel> roleList;
        private RoleModel currentEditRole = new RoleModel();
        public UcPermission()
        {
            InitializeComponent();
            doBindRole = new BindRole(doBindRoles);
            startBind();
        }

        delegate void BindRole();
        private BindRole doBindRole;

        System.Threading.Thread thread;
        private void startBind()
        {
            FrmMain.ucBusyIndicator.IsBusy = true;
            thread = new System.Threading.Thread(bindRoles);
            thread.IsBackground = true;
            thread.Start();
        }

        private void doBindRoles()
        {
            if (roleList != null)
            {
                lbxRole.ItemsSource = roleList;
                if (roleList.Count == 0)
                    btnEdit.IsEnabled = false;
                if (currentEditRole != null)
                {
                    foreach (RoleModel rv in lbxRole.Items)
                    {
                        if (rv.ID == currentEditRole.ID)
                        {
                            lbxRole.SelectedItem = rv;
                            break;
                        }
                    }
                }
            }
            FrmMain.ucBusyIndicator.IsBusy = false;
        }

        private void bindRoles()
        {
            try
            {
                var result = serviceLink.GetAllRoles().ToList();
                roleList = RoleModel.ConvertToList(result.OrderBy(i => i.RoleName).ToList());
            }
            catch (Exception ex)
            {
                roleList = null;
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Dispatcher.BeginInvoke(doBindRole);
        }

        private void lbxRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            RoleModel currentSelectRole = lbxRole.SelectedItem as RoleModel;
            if (currentSelectRole == null)
            {
                btnEdit.IsEnabled = false;
                return;
            }
            btnEdit.IsEnabled = true;
            lblName.Text = currentSelectRole.RoleName;
            lblCreatDate.Text = currentSelectRole.LastUpdateDatetime.ToString();


            //Thread t = new Thread(new ThreadStart(() =>
            //{
            //    Dispatcher.BeginInvoke(new Action(() =>
            //    {

            //    }));
            //}));
            //t.Start();

            //getPromissionByRole(currentSelectRole);

            List<RolePromission> parentList = currentSelectRole.RolePromission.Where(p => p.RO_SysPromissionMTR.Count() == 0).ToList();
            if (parentList == null)
            {
                return;
            }
            List<RolePromission> childList = currentSelectRole.RolePromission.ToList();
            List<PermessionTreeViewModel> tvList = new List<PermessionTreeViewModel>();
            foreach (RolePromission model in parentList)
            {
                PermessionTreeViewModel treeInfo = new PermessionTreeViewModel();
                treeInfo.ID = model.RightID.ToString();
                treeInfo.ParentID = null;
                this.AddPermissionItems(treeInfo, treeInfo.ID, childList);
                tvList.Add(treeInfo);
            }
            tvPromission.ItemsSource = tvList;
            tabPromission.Header = "权限清单";

            ObservableCollection<RO_User> listUser = RoleModel.ConvertROUser(serviceLink.GetUsersByRoleID(currentSelectRole.ID).ToList());
            dgUser.ItemsSource = listUser;
            tabUser.Header = "用户清单(" + listUser.Count + ")";

        }

        public void AddPermissionItems(PermessionTreeViewModel treeInfo, string pid, List<RolePromission> permissionModelList)
        {
            List<RolePromission> result = permissionModelList.Where(x => x.RO_SysPromissionMTR.Count() > 0 && x.RO_SysPromissionMTR[0].ModuleID.ToString() == pid)
                .OrderBy(i => i.RO_SysPromissionMTR[0].SysCode) // Edit By Howe
                .ToList();
            if (result.Count > 0)
            {
                if (result[0].RO_SysPromissionMTR[0].RO_SysModuleMTR.Count() > 0)
                {
                    treeInfo.Name = result[0].RO_SysPromissionMTR[0].RO_SysModuleMTR[0].ModuleName;
                }
            }
            foreach (RolePromission item in result)
            {
                PermessionTreeViewModel tvInfo = new PermessionTreeViewModel();
                tvInfo.ID = item.RightID.ToString();
                tvInfo.Name = item.RO_SysPromissionMTR[0].PermissionName;
                tvInfo.Code = item.RO_SysPromissionMTR[0].SysCode;
                tvInfo.ParentID = item.RO_SysPromissionMTR[0].ModuleID.ToString();
                treeInfo.Childrens.Add(tvInfo);
                this.AddPermissionItems(tvInfo, item.RightID.ToString(), permissionModelList);
            }
        }

        private void getPromissionByRole(RoleModel currentSelectRole)
        {
            int promissionCount = 0;
            var temp = (from s in currentSelectRole.RolePromission
                        group s by s.RO_SysPromissionMTR[0].RO_SysModuleMTR[0].ModuleName into g
                        select new { ModuleName = g.Key, PermissionList = g.ToList() }).ToList();
            tvPromission.Items.Clear();
            for (int i = 0; i < temp.Count; i++)
            {
                TreeViewItem root = new TreeViewItem();
                root.Margin = new Thickness(0, 3, 0, 3);
                root.Header = temp[i].ModuleName;
                for (int j = 0; j < temp[i].PermissionList.Count(); j++)
                {
                    TreeViewItem tn = new TreeViewItem();
                    tn.Margin = new Thickness(0, 3, 0, 3);
                    tn.Header = temp[i].PermissionList[j].RO_SysPromissionMTR[0].PermissionName;
                    root.Items.Add(tn);
                    promissionCount++;
                }
                root.IsExpanded = true;
                tvPromission.Items.Add(root);
            }
            tabPromission.Header = "权限清单(" + promissionCount + ")";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrmEditPermession frm = new FrmEditPermession();
            frm.DataContext = new RoleModel() { Effectiveness = true };
            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    // tvPromission.Items.Clear(); // Edit By Howe : 新增权限角色时修改 Items会报错，改用下面。
                    tvPromission.ItemsSource = null;
                    startBind();
                }
                FrmMain.opacity = 1;
                this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            };
            FrmMain.opacity = 0.9;
            this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            frm.ShowDialog();

            ObservableCollection<RoleModel> tmp = this.lbxRole.DataContext as ObservableCollection<RoleModel>;
            if (tmp != null && tmp.Count > 0)
            {
                ObservableCollection<RoleModel> result = new ObservableCollection<RoleModel>();
                foreach (var item in tmp.OrderBy(i => i.RoleName))
                {
                    result.Add(item);
                }
                this.lbxRole.DataContext = result;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbxRole.SelectedItem == null)
                return;
            FrmEditPermession frm = new FrmEditPermession();
            RoleModel selectRole = lbxRole.SelectedItem as RoleModel;
            frm.DataContext = selectRole.Clone();
            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    currentEditRole = selectRole;
                    startBind();
                }
                FrmMain.opacity = 1;
                this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            };
            FrmMain.opacity = 0.9;
            this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            frm.ShowDialog();

            ObservableCollection<RoleModel> tmp = this.lbxRole.DataContext as ObservableCollection<RoleModel>;
            if (tmp != null && tmp.Count > 0)
            {
                ObservableCollection<RoleModel> result = new ObservableCollection<RoleModel>();
                foreach (var item in tmp.OrderBy(i => i.RoleName))
                {
                    result.Add(item);
                }
                this.lbxRole.DataContext = result;
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (lbxRole.SelectedItem == null)
                currentEditRole = null;
            else
                currentEditRole = lbxRole.SelectedItem as RoleModel;
            Frm_EditUser frm = new Frm_EditUser(currentEditRole);
            frm.DataContext = new UserModel() { Effectiveness = true, Password = "123456" };
            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    try
                    {
                        startBind();
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                FrmMain.opacity = 1;
                this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            };
            FrmMain.opacity = 0.9;
            this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            frm.ShowDialog();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (lbxRole.SelectedItem == null)
            {
                currentEditRole = null;
                return;
            }
            else
            {
                currentEditRole = lbxRole.SelectedItem as RoleModel;
            }
            RO_User selectUser = dgUser.SelectedItem as RO_User;
            UserModel editUser = UserModel.ConvertROUser(selectUser);
            editUser.UserLocationRelation = serviceLink.GetUserLocationByUserID(editUser.ID.ToString());
            Frm_EditUser frm = new Frm_EditUser();
            frm.Owner = SecStaticInfo.FrmMain;
            frm.DataContext = editUser;
            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    tvPromission.ItemsSource = null;
                    try
                    {
                        startBind();
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                FrmMain.opacity = 1;
                this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            };
            FrmMain.opacity = 0.9;
            this.Dispatcher.BeginInvoke(FrmMain.changeOpacity);
            frm.ShowDialog();
        }

        private void dgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
            {
                btnEditUser.IsEnabled = false;
                btnDelUser.IsEnabled = false;
                return;
            }
            btnEditUser.IsEnabled = true;
            btnDelUser.IsEnabled = true;
        }

        private void btnDelUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
                return;
            RO_User user = dgUser.SelectedItem as RO_User;
            var result = MessageBox.Show("是否删除用户:" + user.UserName, "删除", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ReturnMessage returnMsg = serviceLink.DeleteUser(UserModel.ConvertROUser(user).ToUser());
                    if (returnMsg.Code == 1)
                    {
                        startBind();
                    }
                    else
                    {
                        MessageBox.Show(returnMsg.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception exx)
                {
                    MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private List<RoleModel> temp;
        private int searchIndex = 0;
        private bool hasChange = false;
        private void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (hasChange)
                return;
            // 在此处添加事件处理程序实现。
            temp = (from s in roleList
                    where s.RoleName.Contains(txtSearch.Text)
                    select s).ToList();
            lblCount.Text = "找到记录:  " + temp.Count;
            if (temp.Count > 0)
                SecStaticInfo.SetStateInfo("当前记录: " + temp.Count);
            else
                SecStaticInfo.SetStateInfo("当前记录: 0", 3);
            if (temp.Count > 0)
            {
                searchIndex = 0;
                lbxRole.SelectedItem = temp[searchIndex];
                lbxRole.ScrollIntoView(temp[searchIndex]);
            }
            if (temp.Count > 1)
            {
                btnNext.IsEnabled = true;
            }
            else
            {
                btnNext.IsEnabled = false;
                btnPre.IsEnabled = false;
            }
        }
        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtSearch.Text = "";
            btnPre.IsEnabled = false;
            btnNext.IsEnabled = false;
            searchIndex = 0;
            lblCount.Text = "找到记录:  0";
            SecStaticInfo.SetStateInfo(SecStaticInfo.SystemWelcomeInfo);
            hasChange = true;
        }
        private void btnPre_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (searchIndex - 1 >= 0)
            {
                lbxRole.SelectedItem = temp[searchIndex - 1];
                lbxRole.ScrollIntoView(temp[searchIndex - 1]);
                searchIndex--;
                SecStaticInfo.SetStateInfo("当前记录:" + (searchIndex + 1) + "/" + temp.Count);
            }
            if (searchIndex == 0)
            {
                btnPre.IsEnabled = false;
                SecStaticInfo.SetStateInfo("已到达第一条记录!");
            }
            btnNext.IsEnabled = true;
        }

        private void btnNext_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (searchIndex + 1 < temp.Count)
            {
                lbxRole.SelectedItem = temp[searchIndex + 1];
                lbxRole.ScrollIntoView(temp[searchIndex + 1]);
                searchIndex++;
                SecStaticInfo.SetStateInfo("当前记录:" + (searchIndex + 1) + "/" + temp.Count);
            }
            if (searchIndex + 1 == temp.Count)
            {
                btnNext.IsEnabled = false;
                SecStaticInfo.SetStateInfo("已到达最后一条记录!");
            }
            btnPre.IsEnabled = true;
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            hasChange = false;
        }
    }
}
