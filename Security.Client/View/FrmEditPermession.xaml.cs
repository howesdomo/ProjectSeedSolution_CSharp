using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using Security.Client.Common;
using Security.Client.SecurityServer;
using Security.Client.ViewModel;

namespace Security.Client.View
{
    /// <summary>
    /// FrmEditPermession.xaml 的交互逻辑
    /// </summary>
    public partial class FrmEditPermession : Window
    {
        private RoleModel currentRole;
        int NodeTotal = 0;
        int chkTotal = 0;
        ObservableCollection<PermissionModel> listModule;
        ObservableCollection<RolePromission> newPromissionList = new ObservableCollection<RolePromission>();
        ObservableCollection<RolePromission> delPromissionList = new ObservableCollection<RolePromission>();
        private SecurityServiceLink serviceLink = new SecurityServiceLink();
        public FrmEditPermession()
        {
            InitializeComponent();
            this.Loaded += delegate
            {
                currentRole = this.DataContext as RoleModel;
                btnStop.Content = currentRole.Effectiveness == true ? "停用" : "启用";
                chkIsAdmin.IsChecked = currentRole.IsAdminstrator;
                doBindPermession = new BindPermession(bindUI);
                startBind();
                currentRole.PropertyChanged += (s, e2) =>
                {
                    try
                    {
                        errors.Add(currentRole.errors[e2.PropertyName]);
                    }
                    catch (Exception)
                    {
                        errors.Clear();
                    };
                };
            };
        }
        private List<string> errors = new List<string>();
        delegate void BindPermession();
        private BindPermession doBindPermession;

        System.Threading.Thread thread;
        private void startBind()
        {
            FrmMain.ucBusyIndicator.IsBusy = true;
            thread = new System.Threading.Thread(bindData);
            thread.IsBackground = true;
            thread.Start();
        }

        private void bindUI()
        {
            //for (int i = 0; i < listModule.Count; i++)
            //{
            //    TreeViewItem root = new TreeViewItem();
            //    root.Header = listModule[i].ModuleName;
            //    root.Tag = listModule[i].ID;
            //    int hasCheck = 0;
            //    for (int j = 0; j < listModule[i].SysPromissionMTR.Count(); j++)
            //    {
            //        CheckBox tn = new CheckBox();
            //        tn.Margin = new Thickness(0, 5, 0, 5);
            //        tn.Content = listModule[i].SysPromissionMTR[j].PermissionName;
            //        tn.Tag = listModule[i].SysPromissionMTR[j].ID;
            //        bool openNode = SetTreeViewCheck(listModule[i].SysPromissionMTR[j].ID);
            //        tn.IsChecked = openNode;
            //        if (openNode) { hasCheck++; chkTotal++; }
            //        tn.Click += new RoutedEventHandler(tn_Click);
            //        root.Items.Add(tn);
            //        NodeTotal++;
            //    }
            //    root.IsExpanded = hasCheck > 0 ? true : false;
            //    tvModule.Items.Add(root);
            //}
            //lblCount.Text = chkTotal + "/" + NodeTotal;
            List<PermessionTreeViewModel> tvList = new List<PermessionTreeViewModel>();
            var grpPermissionModelList = listModule.Where(p => string.IsNullOrEmpty(p.ModuleID) == false && string.IsNullOrEmpty(p.ModuleName) == false).GroupBy(p => p.ModuleID);
            foreach (var model in grpPermissionModelList)
            {
                string str = model.Key;
                PermissionModel PermissionModel = model.FirstOrDefault();
                PermessionTreeViewModel treeInfo = new PermessionTreeViewModel();
                treeInfo.ID = PermissionModel.ModuleID;
                treeInfo.Name = PermissionModel.ModuleName;
                //treeInfo.Code = PermissionModel.SysCode;
                treeInfo.ParentID = null;
                this.AddPermissionItems(treeInfo, treeInfo.ID, listModule);
                tvList.Add(treeInfo);
            }
            foreach (var item in tvList)
            {
                recursion(item);
                this.checkNull(item);
            }

            tvModule.ItemsSource = tvList;
            lblCount.Text = chkTotal + "/" + NodeTotal;
            FrmMain.ucBusyIndicator.IsBusy = false;
        }

        // ** Add By Howe
        private void checkNull(PermessionTreeViewModel item)
        {
            int shouldInstallCount = item.Childrens.Count(i => i.ShouldInstall == true);
            if (shouldInstallCount == item.Childrens.Count)
            {
                item.ShouldInstall = true;
            }
            else if (shouldInstallCount == 0)
            {
                item.ShouldInstall = false;
            }
            else
            {
                item.ShouldInstall = null;
            }
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private void recursion(PermessionTreeViewModel model)
        {
            foreach (var item in model.Childrens)
            {
                recursion(item);
                checkNull(model);
            }
        }

        // ** Add By Howe

        int hasCheck = 0;
        public void AddPermissionItems(PermessionTreeViewModel treeInfo, string pid, ObservableCollection<PermissionModel> permissionModelList)
        {
            bool openNode = SetTreeViewCheck(treeInfo.ID);
            treeInfo.ShouldInstall = openNode;
            if (openNode) { this.hasCheck++; this.chkTotal++; }
            this.NodeTotal++;
            List<PermissionModel> result = permissionModelList.Where(x => x.ModuleID == pid).ToList();
            foreach (PermissionModel item in result)
            {
                PermessionTreeViewModel tvInfo = new PermessionTreeViewModel();
                tvInfo.ID = item.ID;
                tvInfo.Name = item.PermissionName;
                tvInfo.Code = item.SysCode;
                tvInfo.ParentID = item.ModuleID;
                treeInfo.Childrens.Add(tvInfo);
                this.AddPermissionItems(tvInfo, item.ID, permissionModelList);
            }
        }

        private void bindData()
        {
            try
            {
                listModule = new ObservableCollection<PermissionModel>(serviceLink.GetAllModuleList());
            }
            catch (Exception ex)
            {
                listModule = null;
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Dispatcher.BeginInvoke(doBindPermession);
        }

        bool SetTreeViewCheck(string id)
        {
            if (currentRole == null)
                return false;
            if (currentRole.RolePromission == null)
                return false;
            foreach (RolePromission item in currentRole.RolePromission)
            {
                if (item.RightID.ToString().ToUpper() == id.ToUpper())
                    return true;
            }
            return false;
        }

        bool SetTreeViewCheck(Guid id)
        {
            if (currentRole == null)
                return false;
            if (currentRole.RolePromission == null)
                return false;
            foreach (RolePromission item in currentRole.RolePromission)
            {
                if (item.RightID == id)
                    return true;
            }
            return false;
        }

        void tn_Click(object sender, RoutedEventArgs e)
        {
            chkTotal = 0;
            foreach (TreeViewItem root in tvModule.Items)
            {
                foreach (CheckBox chk in root.Items)
                {
                    if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                    {
                        chkTotal++;
                    }
                }
            }
            lblCount.Text = chkTotal + "/" + NodeTotal;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (currentRole.Effectiveness.Value)
            {
                currentRole.Effectiveness = false;
                btnStop.Content = "启用";
            }
            else
            {
                currentRole.Effectiveness = true;
                btnStop.Content = "停用";
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (currentRole.Vis())
            {
                List<PermessionTreeViewModel> treeList = tvModule.ItemsSource as List<PermessionTreeViewModel>;
                if (treeList == null) { return; }
                currentRole.IsAdminstrator = chkIsAdmin.IsChecked;
                if (currentRole.ID != Guid.Empty)
                {
                    #region 修改
                    newPromissionList = null;
                    newPromissionList = new ObservableCollection<RolePromission>();
                    delPromissionList = null;
                    delPromissionList = new ObservableCollection<RolePromission>();
                    foreach (PermessionTreeViewModel root in treeList)
                    {
                        GetNewCheckedChildrens(root, newPromissionList);
                    }
                    foreach (PermessionTreeViewModel root in treeList)
                    {
                        GetDelCheckedChildrens(root, delPromissionList);
                    }
                    //List<RO_SysModuleMTR> newModuleList = new List<RO_SysModuleMTR>();
                    //foreach (TreeViewItem root in tvModule.Items)
                    //{
                    //    foreach (CheckBox chk in root.Items)
                    //    {
                    //        if (chk.IsChecked.HasValue && chk.IsChecked.Value)
                    //        {
                    //            //查找新增
                    //            bool add = true;
                    //            foreach (RolePromission rp in currentRole.RolePromission)
                    //            {
                    //                if (rp.RightID.ToString() == chk.Tag.ToString())
                    //                {
                    //                    add = false;
                    //                    break;
                    //                }
                    //            }
                    //            if (add)
                    //            {
                    //                newPromissionList.Add(new RolePromission { RightID = Guid.Parse(chk.Tag.ToString()), RoleID = currentRole.ID });
                    //            }
                    //            bool blModule = newModuleList.Exists(p => p.ID.Equals(Guid.Parse(root.Tag.ToString())));
                    //            if (!blModule)
                    //            {
                    //                newModuleList.Add(new RO_SysModuleMTR { ID = Guid.Parse(root.Tag.ToString()) });
                    //            }
                    //        }
                    //        else
                    //        {//查找删除
                    //            foreach (RolePromission rp in currentRole.RolePromission)
                    //            {
                    //                if (rp.RightID.ToString() == chk.Tag.ToString())
                    //                {
                    //                    delPromissionList.Add(new RolePromission { RightID = Guid.Parse(chk.Tag.ToString()), RoleID = currentRole.ID });
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    ReturnMessage returnMessage = serviceLink.ModiftyRole(currentRole.ToRole(), newPromissionList.ToArray(), delPromissionList.ToArray(), SecStaticInfo.User.ID);
                    if (returnMessage.Code == 1)
                    {
                        MessageBox.Show("操作成功");
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show(returnMessage.Message);
                    }
                    #endregion
                }
                else
                {
                    #region 新增
                    currentRole.ID = Guid.NewGuid();
                    ObservableCollection<RolePromission> addperList = new ObservableCollection<RolePromission>();
                    foreach (PermessionTreeViewModel root in treeList)
                    {
                        GetCheckedChildrens(root, addperList);
                    }
                    ReturnMessage returnMsg = serviceLink.AddRole(currentRole.ToRole(), addperList.ToArray(), SecStaticInfo.User.ID);
                    if (returnMsg.Code == 1)
                    {
                        MessageBox.Show("操作成功");
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show(returnMsg.Message);
                        currentRole.ID = Guid.Empty;
                    }
                    #endregion
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

        /// <summary>
        /// 获取旧权限和新增权限
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="addperList"></param>
        private void GetNewCheckedChildrens(PermessionTreeViewModel feature, ObservableCollection<RolePromission> newList)
        {
            if (feature.ShouldInstall == null || feature.ShouldInstall.Value == true)
            {
                //查找新增
                RolePromission rp = currentRole.RolePromission.FirstOrDefault(p => p.RightID.ToString().ToUpper() == feature.ID.ToUpper());
                if (rp == null)
                {
                    newList.Add(new RolePromission { RightID = Guid.Parse(feature.ID) });
                }
                foreach (PermessionTreeViewModel childFeature in feature.Childrens)
                {
                    GetNewCheckedChildrens(childFeature, newList);
                }
            }
        }

        private void GetDelCheckedChildrens(PermessionTreeViewModel feature, ObservableCollection<RolePromission> delList)
        {
            if (feature.ShouldInstall != null && feature.ShouldInstall.Value == false)
            {
                //查找删除
                RolePromission rp = currentRole.RolePromission.FirstOrDefault(p => p.RightID.ToString().ToUpper() == feature.ID.ToUpper());
                if (rp != null)
                {
                    delList.Add(new RolePromission { RightID = Guid.Parse(feature.ID) });
                }
                foreach (PermessionTreeViewModel childFeature in feature.Childrens)
                {
                    GetDelCheckedChildrens(childFeature, delList);
                }
            }
            else if (feature.ShouldInstall == null)
            {
                foreach (PermessionTreeViewModel childFeature in feature.Childrens)
                {
                    GetDelCheckedChildrens(childFeature, delList);
                }
            }
        }


        /// <summary>
        /// 新增权限
        /// </summary>
        private static void GetCheckedChildrens(PermessionTreeViewModel feature, ObservableCollection<RolePromission> addperList)
        {
            if (feature.ShouldInstall == null || feature.ShouldInstall.Value == true)
            {
                RolePromission r = new RolePromission();
                r.RightID = Guid.Parse(feature.ID);
                addperList.Add(r);
                foreach (PermessionTreeViewModel childFeature in feature.Childrens)
                {
                    GetCheckedChildrens(childFeature, addperList);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ItemCheckbox_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = GetParentTreeViewItem((DependencyObject)sender);
            if (item != null)
            {
                PermessionTreeViewModel feature = item.DataContext as PermessionTreeViewModel;
                if (feature != null)
                {
                    UpdateChildrenCheckedState(feature);

                    UpdateParentCheckedState(item);
                }
            }
            chkTotal = 0;
            List<PermessionTreeViewModel> tvList = tvModule.ItemsSource as List<PermessionTreeViewModel>;

            foreach (PermessionTreeViewModel tree in tvList)
            {
                SumTreeViewCount(tree);
            }

            lblCount.Text = chkTotal + "/" + NodeTotal;
        }

        private void SumTreeViewCount(PermessionTreeViewModel tree)
        {
            if (tree.ShouldInstall == null || tree.ShouldInstall.Value == true)
            {
                chkTotal++;
                if (tree.Childrens.Count > 0)
                {
                    foreach (PermessionTreeViewModel item in tree.Childrens)
                    {
                        SumTreeViewCount(item);
                    }
                }
            }
        }

        private static TreeViewItem GetParentTreeViewItem(DependencyObject item)
        {
            if (item != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(item);
                TreeViewItem parentTreeViewItem = parent as TreeViewItem;
                return (parentTreeViewItem != null) ? parentTreeViewItem : GetParentTreeViewItem(parent);
            }
            return null;
        }

        private static void UpdateParentCheckedState(TreeViewItem item)
        {
            TreeViewItem parent = GetParentTreeViewItem(item);
            if (parent != null)
            {
                PermessionTreeViewModel feature = parent.DataContext as PermessionTreeViewModel;
                if (feature != null)
                {
                    bool? childrenCheckedState = feature.Childrens.First<PermessionTreeViewModel>().ShouldInstall;
                    for (int i = 1; i < feature.Childrens.Count(); i++)
                    {
                        if (childrenCheckedState != feature.Childrens[i].ShouldInstall)
                        {
                            childrenCheckedState = null;
                            break;
                        }
                    }
                    feature.ShouldInstall = childrenCheckedState;
                    UpdateParentCheckedState(parent);
                }
            }
        }

        private static void UpdateChildrenCheckedState(PermessionTreeViewModel feature)
        {
            if (feature.ShouldInstall.HasValue)
            {
                foreach (PermessionTreeViewModel childFeature in feature.Childrens)
                {
                    childFeature.ShouldInstall = feature.ShouldInstall;
                    if (childFeature.Childrens.Count() > 0)
                    {
                        UpdateChildrenCheckedState(childFeature);
                    }
                }
            }
        }
    }
}
