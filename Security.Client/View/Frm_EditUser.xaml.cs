using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Collections.ObjectModel;

using Security.Client.SecurityServer;
using Security.Client.Common;
using Security.Client.ViewModel;

namespace Security.Client.View
{
    /// <summary>
    /// Frm_EditUser.xaml 的交互逻辑
    /// </summary>
    public partial class Frm_EditUser : Window
    {
        SecurityServiceLink client = new SecurityServiceLink();
        ObservableCollection<RoleMTR> list;
        List<Location> LocationAll;
        private Location currentLocation;
        public Frm_EditUser()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Frm_EditUser_Loaded);

            tvLocation.SelectedItemChanged += (o, ex) =>
            {
                if (tvLocation.SelectedItem != null)
                {
                    currentLocation = tvLocation.SelectedItem as Location;
                }
            };

        }
        RoleModel currentRole;
        public Frm_EditUser(RoleModel CR)
        {
            InitializeComponent();
            currentRole = CR;
            this.Loaded += new RoutedEventHandler(Frm_EditUser_Loaded);
            //bl.GetAllRolesCompleted += new EventHandler<GetAllRolesCompletedEventArgs>(bl_GetAllRolesCompleted);

        }

        UserModel currentUser;
        void Frm_EditUser_Loaded(object sender, RoutedEventArgs e)
        {
            currentUser = this.DataContext as UserModel;
            btnStop.Content = currentUser.Effectiveness.Value == true ? "停用" : "启用";
            if (currentUser.ID != Guid.Empty)
            {
                btnStop.Visibility = Visibility.Visible;
                btnResetPwd.Visibility = System.Windows.Visibility.Visible;
            }
            doBindRole = new BindRole(doBindRoles);
            startBind();

            currentUser.PropertyChanged += (s, e2) =>
            {
                try
                {
                    errors.Add(currentUser.errors[e2.PropertyName]);
                }
                catch (Exception)
                {
                    errors.Clear();
                };
            };

            //this.txtName.Focus();
        }

        private List<string> errors = new List<string>();

        /// <summary>
        /// 绑定的委托
        /// </summary>
        delegate void BindRole();
        /// <summary>
        /// 绑定的委托事件
        /// </summary>
        private BindRole doBindRole;

        System.Threading.Thread thread;
        private void startBind()
        {
            FrmMain.ucBusyIndicator.IsBusy = true;
            thread = new System.Threading.Thread(bindRoles);
            thread.IsBackground = true;
            thread.Start();
        }

        private void bindRoles()
        {
            try
            {
                list = new ObservableCollection<RoleMTR>(client.GetAllRoles());
                SecurityServiceLink web = new SecurityServiceLink();
                LocationAll = new List<Location>();
                // LocationAll.AddRange(web.GetLocationByTypeControlKey(Config.LocationTypeControlKey));
                // LocationAll.AddRange(web.GetLocationTreeList()); // TO_EML_HNGSL : 本项目无 User 和 Location 关联
            }
            catch (Exception ex)
            {
                list = null;
                LocationAll = null;
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Dispatcher.BeginInvoke(doBindRole);

        }

        private void doBindRoles()
        {
            if (list != null)
            {
                foreach (RoleMTR r in list)
                {
                    CheckBox chk = new CheckBox();

                    chk.Margin = new Thickness(0, 1, 0, 1);

                    chk.IsChecked = SetCheck(r.ID);
                    if (currentRole != null && currentRole.ID == r.ID)
                        chk.IsChecked = true;
                    chk.Tag = r;
                    chk.Content = r.RoleName;
                    treeView1.Items.Add(chk);
                }
            }

            if (LocationAll != null)
            {
                this.tviAll.ItemsSource = GetLocationItem(LocationAll.Where(p => p.ParentID == string.Empty).ToList());
                this.tviAll.Header = "全部";
            }

            FrmMain.ucBusyIndicator.IsBusy = false;
        }

        private List<TreeViewItem> GetLocationItem(List<Location> tList)
        {
            List<TreeViewItem> list = new List<TreeViewItem>();
            foreach (Location t in tList)
            {
                TreeViewItem tviSub = new TreeViewItem();
                tviSub.DataContext = t;
                tviSub.Margin = new Thickness(0, 1, 0, 0);
                string company = string.Empty;
                if (string.IsNullOrEmpty(t.ParentID))
                {
                    company = t.Code;
                }
                else
                {
                    company = this.getCompanyCode(t.ParentID);
                }
                CheckBox tn = new CheckBox();
                tn.Tag = t.ID;
                bool ischeck = this.SetTreeViewCheck(t.LocationTypeID, t.Code, company);
                tn.IsChecked = ischeck;
                tviSub.IsExpanded = ischeck;
                tn.Content = t.Name;
                tviSub.Header = tn;

                tn.Checked += (s, e) =>
                {
                    tviSub.IsExpanded = true;
                    foreach (TreeViewItem tv in tviSub.Items)
                    {
                        (tv.Header as CheckBox).IsChecked = true;
                    }
                    // FindVisualChild<System.Windows.Controls.CheckBox>(tviAll, t.ParentID);
                };

                tn.Unchecked += (s, e) =>
                {
                    tviSub.IsExpanded = true;
                    foreach (TreeViewItem tv in tviSub.Items)
                    {
                        (tv.Header as CheckBox).IsChecked = false;
                    }
                };

                List<TreeViewItem> subList = GetLocationItem(new List<Location>(LocationAll.Where(p => p.ParentID == t.ID)));

                if (subList.Count > 0)
                {
                    tviSub.ItemsSource = subList;
                }
                if (!tviSub.IsExpanded)
                {
                    tviSub.IsExpanded = (subList.Where(p => p.IsExpanded == true).Count() > 0);
                }
                list.Add(tviSub);
            }
            return list;
        }

        //public void FindVisualChild<TChild>(DependencyObject obj, string parentID) where TChild : DependencyObject
        //{
        //    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
        //    {
        //        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        //        if (child != null && child is TChild)
        //        {
        //            System.Windows.Controls.CheckBox cb = (System.Windows.Controls.CheckBox)child;
        //            if (cb.Tag.ToString() == parentID)
        //            {
        //                cb.IsChecked = true;
        //                break;
        //            }
        //        }
        //        FindVisualChild<TChild>(child, parentID);
        //    }
        //}


        string companyCode = string.Empty;
        private string getCompanyCode(string parentID)
        {
            Location item = LocationAll.FirstOrDefault(p => p.ID == parentID);
            if (string.IsNullOrEmpty(item.ParentID))
            {
                companyCode = item.Code;
            }
            else
            {
                getCompanyCode(item.ParentID);
            }
            return companyCode;
        }

        private bool SetTreeViewCheck(int typeId, string Code, string companyCode)
        {
            if (currentUser == null)
                return false;
            if (currentUser.UserLocationRelation == null)
                return false;
            foreach (UserLocationRelation ur in currentUser.UserLocationRelation)
            {
                if (ur.LocationTypeID == typeId && ur.LocationCode == Code && ur.CompanyCode == companyCode)
                    return true;
            }
            return false;
        }

        private bool SetTreeViewCheck(string ID)
        {
            if (currentUser == null)
                return false;
            foreach (var ur in currentUser.UserLocationRelation)
            {
                if (ur.UserID.ToString() == ID)
                    return true;
            }
            return false;
        }

        private bool SetCheck(Guid id)
        {
            if (currentUser == null) return false;
            if (currentUser.UserRole == null) return false;
            foreach (UserRole ur in currentUser.UserRole)
            {
                if (ur.RO_RoleMTR.ID == id)
                    return true;
            }
            return false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            errors.Clear();
            if (currentUser.Vis())
            {
                //新增
                if (currentUser.ID == Guid.Empty)
                {
                    ObservableCollection<UserRole> nlist = new ObservableCollection<UserRole>();
                    foreach (CheckBox chk in treeView1.Items)
                    {
                        if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                        {
                            RoleMTR r = chk.Tag as RoleMTR;
                            nlist.Add(new UserRole { ID = r.ID, RO_RoleMTR = new RO_RoleMTR { RoleName = r.RoleName } });
                        }
                    }

                    List<UserLocationRelation> newUserLocationRelation = new List<UserLocationRelation>();
                    this.treeItems(tviAll.Items, newUserLocationRelation);
                    try
                    {
                        UserRole[] temp = new UserRole[nlist.Count()];
                        nlist.CopyTo(temp, 0);
                        var rm = client.AddUser(currentUser.ToUser(), temp, newUserLocationRelation.ToArray());
                        if (rm.Code == 1)
                        {
                            var resule = MessageBox.Show("操作成功", "新增", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show(rm.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    ObservableCollection<UserRole> nlist = new ObservableCollection<UserRole>();
                    ObservableCollection<UserRole> dlist = new ObservableCollection<UserRole>();
                    foreach (CheckBox chk in treeView1.Items)
                    {
                        RoleMTR r = chk.Tag as RoleMTR;
                        if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                        {
                            //查找新增
                            bool add = true;
                            foreach (UserRole ur in currentUser.UserRole)
                            {
                                if (ur.RoleID == r.ID)
                                {
                                    add = false;
                                    break;
                                }
                            }
                            if (add)
                                nlist.Add(new UserRole { ID = r.ID, RO_RoleMTR = new RO_RoleMTR { RoleName = r.RoleName } });
                        }
                        else
                        {//查找删除
                            foreach (UserRole ur in currentUser.UserRole)
                            {
                                if (ur.RoleID == r.ID)
                                {
                                    dlist.Add(new UserRole { ID = r.ID, RO_RoleMTR = new RO_RoleMTR { RoleName = r.RoleName } });
                                    break;
                                }
                            }
                        }
                    }

                    List<UserLocationRelation> newUserLocationRelation = new List<UserLocationRelation>();
                    this.treeItems(tviAll.Items, newUserLocationRelation);
                    try
                    {
                        //bl.ModifyUserAsync(currentUser.ToUser(), nlist, dlist);
                        UserRole[] temp = new UserRole[nlist.Count()];
                        nlist.CopyTo(temp, 0);
                        UserRole[] temp2 = new UserRole[dlist.Count()];
                        dlist.CopyTo(temp2, 0);
                        var rm = client.ModifyUser(currentUser.ToUser(), temp, temp2, newUserLocationRelation.ToArray());
                        if (rm.Code == 1)
                        {
                            var resule = MessageBox.Show("操作成功", "修改", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show(rm.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                            currentUser.ID = Guid.Empty;
                        }
                    }
                    catch (Exception exx)
                    {
                        MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                var msg = "";

                var list = errors.Distinct();
                foreach (var str in list)
                {
                    msg += str + "\n";
                }
                MessageBox.Show(msg);
                this.txtName.Focus();
            }
        }

        public void treeItems(ItemCollection treeCollection, List<UserLocationRelation> newUserLocationRelation)
        {
            foreach (TreeViewItem tv in treeCollection)
            {
                CheckBox cb = tv.Header as CheckBox;
                if (cb != null)
                {
                    if (cb.IsChecked.HasValue && cb.IsChecked.Value)
                    {
                        Location newLocation = LocationAll.FirstOrDefault(p => p.ID == cb.Tag.ToString());
                        UserLocationRelation newUserLocation = new UserLocationRelation();
                        newUserLocation.LocationCode = newLocation.Code;
                        newUserLocation.LocationTypeID = newLocation.LocationTypeID;
                        if (string.IsNullOrEmpty(newLocation.ParentID))
                        {
                            newUserLocation.CompanyCode = newLocation.Code;
                        }
                        else
                        {
                            newUserLocation.CompanyCode = this.getCompanyCode(newLocation.ParentID);
                        }
                        newUserLocation.UserID = currentUser.ID;
                        newUserLocation.LastUpdateUserID = SecStaticInfo.User.ID;
                        newUserLocationRelation.Add(newUserLocation);
                    }
                    treeItems(tv.Items, newUserLocationRelation);
                }
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.Effectiveness.Value)
            {
                currentUser.Effectiveness = false;
                MessageBox.Show("停用成功", "停用");
                btnStop.Content = "启用";
            }
            else
            {
                currentUser.Effectiveness = true;
                MessageBox.Show("启用成功", "启用");
                btnStop.Content = "停用";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnResetPwd_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("您确认要重置该用户的密码?\t注:初始密码为 123456 ", "重置密码", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                currentUser.Password = "123456";
                MessageBox.Show("请点击保存，密码重置！", "重置密码");
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
