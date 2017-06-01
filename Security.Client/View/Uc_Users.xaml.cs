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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.ObjectModel;

using Security.Client.SecurityServer;
using Security.Client.ViewModel;
using Security.Client.Common;

namespace Security.Client.View
{
    /// <summary>
    /// Uc_Users.xaml 的交互逻辑
    /// </summary>
    public partial class Uc_Users : UserControl
    {
        SecurityServiceLink client = new SecurityServiceLink();
        public Uc_Users()
        {
             InitializeComponent();
            doBindUser = new BindUser(doBindUsers);

            startBind();
            
        }

        ObservableCollection<UserModel> userList;
        UserModel currentEditUser;

        /// <summary>
        /// 绑定的委托
        /// </summary>
        delegate void BindUser();
        /// <summary>
        /// 绑定的委托事件
        /// </summary>
        private BindUser doBindUser;

        private void doBindUsers()
        {
            if (userList != null)
            {
                listBox1.ItemsSource = userList;
                if (userList.Count == 0)
                {
                    btnDel.IsEnabled = false;
                    btnEdit.IsEnabled = false;
                }
                if (currentEditUser != null)
                {
                    foreach (UserModel rv in listBox1.Items)
                    {
                        if (rv.ID == currentEditUser.ID)
                        {
                            listBox1.SelectedItem = rv;
                            break;
                        }
                    }
                }
            }
            FrmMain.ucBusyIndicator.IsBusy = false;
        }

        System.Threading.Thread thread;
        private void startBind()
        {
            FrmMain.ucBusyIndicator.IsBusy = true;
            thread = new System.Threading.Thread(bindUsers);
            thread.IsBackground = true;
            thread.Start();
        }

        private void bindUsers()
        {
            try
            {
                ObservableCollection<UserMTR> list = new ObservableCollection<UserMTR>(client.GetAllUsers());
                userList = UserModel.ConvertToList(list);

                SecurityServiceLink web = new SecurityServiceLink();
                LocationAll = new List<Location>();
                //LocationAll.AddRange(web.GetLocationTreeList()); // 本项目没有 User 和 Location 权限, 顾不加载数据
            }
            catch (Exception ex)
            {
                userList = null;
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Dispatcher.BeginInvoke(doBindUser);
        }

        UserModel selectUser;
        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            selectUser = listBox1.SelectedItem as UserModel;
            if (selectUser == null)
            {
                btnDel.IsEnabled = false;
                btnEdit.IsEnabled = false;
                return;
            }
            btnDel.IsEnabled = true;
            btnEdit.IsEnabled = true;

            listBoxRole.ItemsSource = selectUser.UserRole;
            this.tviAll.ItemsSource = GetLocationItem(LocationAll.Where(p => p.ParentID == string.Empty).ToList());            
            this.tviAll.Header = "全部";
            this.tviAll.IsExpanded = true;
            lblCreatDate.Text = selectUser.LastUpdateDatetime.ToString();
            lblLoginName.Text = selectUser.LoginAccount;
            lblName.Text = selectUser.UserName;
        }

        private List<Location> LocationAll;
        private List<TreeViewItem> GetLocationItem(List<Location> tList)
        {
            List<TreeViewItem> list = new List<TreeViewItem>();
            foreach (Location t in tList)
            {
                TreeViewItem tviSub = new TreeViewItem();
                tviSub.DataContext = t;
                tviSub.Margin = new Thickness(0, 1, 0, 0);

                CheckBox tn = new CheckBox();
                tn.IsEnabled = false;
                tn.Tag = t.ID;
                tn.Style = null;
                string company = string.Empty;
                if (string.IsNullOrEmpty(t.ParentID))
                {
                    company = t.Code;
                }
                else
                {
                    company = this.getCompanyCode(t.ParentID);
                }
                bool ischeck = SetTreeViewCheck(t.LocationTypeID, t.Code, company);
                tn.IsChecked = ischeck;
                //tviSub.IsExpanded = ischeck;
                tn.Content = new TextBlock() { Foreground = Brushes.Black, Text = t.Name };
                tviSub.Header = tn;

                //tn.Checked += (s, e) =>
                //{
                //    tviSub.IsExpanded = true;
                //    foreach (TreeViewItem tv in tviSub.Items)
                //    {
                //        (tv.Header as CheckBox).IsChecked = true;
                //    }
                //};

                //tn.Unchecked += (s, e) =>
                //{
                //    tviSub.IsExpanded = true;
                //    foreach (TreeViewItem tv in tviSub.Items)
                //    {
                //        (tv.Header as CheckBox).IsChecked = false;
                //    }
                //};

                List<TreeViewItem> subList = GetLocationItem(new List<Location>(LocationAll.Where(p => p.ParentID == t.ID)));

                if (subList.Count > 0)
                {
                    tviSub.ItemsSource = subList;
                }
                //if (!tviSub.IsExpanded)
                //{
                //    tviSub.IsExpanded = (subList.Where(p => p.IsExpanded == true).Count() > 0);
                //}
                list.Add(tviSub);
            }
            return list;
        }

        



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

        private bool SetTreeViewCheck(string ID)
        {
            if (selectUser == null)
                return false;
            if (selectUser.UserLocationRelation == null)
                return false;
            foreach (var ur in selectUser.UserLocationRelation)
            {
                if (ur.ID.ToString() == ID )
                    return true;
            }
            return false;
        }

        private bool SetTreeViewCheck(int typeId, string Code, string companyCode)
        {
            if (selectUser == null)
                return false;
            if (selectUser.UserLocationRelation == null)
                return false;
            foreach (UserLocationRelation ur in selectUser.UserLocationRelation)
            {
                if (ur.LocationTypeID == typeId && ur.LocationCode == Code && ur.CompanyCode == companyCode)
                    return true;
            }
            return false;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frm_EditUser frm = new Frm_EditUser();
            frm.Owner = SecStaticInfo.FrmMain;
            frm.DataContext = new UserModel() { Effectiveness =true, Password = "123456" };

            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    try
                    {
                        //bl.GetAllUsersAsync();
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            Frm_EditUser frm = new Frm_EditUser();
            frm.Owner = SecStaticInfo.FrmMain;
            UserModel selectUser = listBox1.SelectedItem as UserModel;
            UserModel editUser = selectUser.Clone();
            frm.DataContext = editUser;
            frm.Closed += (o, ex) =>
            {
                if (frm.DialogResult.Value)
                {
                    currentEditUser = editUser;
                    try
                    {
                        //bl.GetAllUsersAsync();
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

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            UserModel uv = listBox1.SelectedItem as UserModel;
            var result = MessageBox.Show("是否删除用户:" + uv.UserName, "删除", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    //bl.DeleteUserAsync((listBox1.SelectedItem as UserModel).ToUser());
                    var rm = client.DeleteUser((listBox1.SelectedItem as UserModel).ToUser());
                    startBind();
                }
                catch (Exception exx)
                {
                    MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //MsgBox.OnButtonClick += (result) =>
            //{
            //    if (result == MsgDialogResult.OK)
            //    {
            //        try
            //        {
            //            bl.DeleteUserAsync((listBox1.SelectedItem as UserModel).ToUser());
            //        }
            //        catch (Exception exx)
            //        {
            //            MsgBox.Show(exx.Message, "错误", MsgInfo.Error);

            //        }

            //    }
            //};
            //MsgBox.Show("是否删除用户:" + uv.UserName, "删除", MsgBoxButton.OKCancel, MsgInfo.Warning);
            //bl.DeleteUserCompleted += (oo, ee) =>
            //{
            //    try
            //    {
            //        bl.GetAllUsersAsync();
            //    }
            //    catch (Exception exx)
            //    {
            //        MessageBox.Show(exx.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);

            //    }

            //};
        }
        List<UserModel> temp;
        int searchIndex = 0;
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (hasChange)
                return;
            // 在此处添加事件处理程序实现。
            temp = (from s in userList
                    where s.UserName.Contains(txtSearch.Text)
                    select s).ToList();
            lblCount.Text = "找到记录:  " + temp.Count;
            if (temp.Count > 0)
                SecStaticInfo.SetStateInfo("当前记录: " + temp.Count);
            else
                SecStaticInfo.SetStateInfo("当前记录: 0", 3);
            if (temp.Count > 0)
            {
                searchIndex = 0;
                listBox1.SelectedItem = temp[searchIndex];
                listBox1.ScrollIntoView(temp[searchIndex]);
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
        bool hasChange = false;
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            btnPre.IsEnabled = false;
            btnNext.IsEnabled = false;
            searchIndex = 0;
            lblCount.Text = "找到记录:  0";
            SecStaticInfo.SetStateInfo(SecStaticInfo.SystemWelcomeInfo);
            hasChange = true;
        }

        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            if (searchIndex - 1 >= 0)
            {
                listBox1.SelectedItem = temp[searchIndex - 1];
                listBox1.ScrollIntoView(temp[searchIndex - 1]);
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

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (searchIndex + 1 < temp.Count)
            {
                listBox1.SelectedItem = temp[searchIndex + 1];
                listBox1.ScrollIntoView(temp[searchIndex + 1]);
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
