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

using Security.Client.ViewModel;
using Security.Client.Common;
using Security.Client.ViewModel;

namespace Security.Client.View
{
    /// <summary>
    /// Uc_ChangePwd.xaml 的交互逻辑
    /// </summary>
    public partial class Uc_ChangePwd : UserControl
    {
        SecurityServiceLink client = new SecurityServiceLink();
        UserModel u = new UserModel();
        public Uc_ChangePwd()
        {
            InitializeComponent();
            //Storyboard1.Begin();
            this.Loaded += new RoutedEventHandler(Frm_ChangePwd_Loaded);
        }

        void Frm_ChangePwd_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = u;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtNewPwd1.Password = "";
            txtNewPwd2.Password = "";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            u.NewPassword = this.txtNewPwd1.Password;
            u.NewPassword2 = this.txtNewPwd2.Password;
            if (u.CheckChangePwd())
            {
                if (u.NewPassword == SecStaticInfo.User.Password)
                {
                    MessageBox.Show("新密码不能与旧密码一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!u.NewPassword.Equals(u.NewPassword2))
                {
                    MessageBox.Show("两次输入的密码不一致！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                //bl.ModifyUserCompleted += (o, ex) =>
                //{
                //    if (ex.Result.Code == 1)
                //    {
                //        MessageBox.OnButtonClick += (result) =>
                //        {
                //            SecStaticInfo.User.Password = u.NewPassword;
                //            btnSubmit.IsEnabled = false;
                //            btnClear.IsEnabled = false;
                //            txtNewPwd1.IsEnabled = false;
                //            txtNewPwd2.IsEnabled = false;
                //            SecStaticInfo.SendCommand(0);
                //        };
                //        MessageBox.Show("修改密码成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                //    }
                //    else
                //    {
                //        MessageBox.Show(ex.Result.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //    }
                //};
                try
                {
                    UserModel temp = SecStaticInfo.User.Clone();
                    //temp.Password = u.NewPassword;
                    //bl.ModifyUserAsync(temp.ToUser(), new ObservableCollection<SecurityService.RoleDTO>(),
                    //    new ObservableCollection<SecurityService.RoleDTO>());
                    var rm = client.ModifyPassword(temp.ToUser(), u.NewPassword);
                    if (rm.Code == 1)
                    {
                        var result = MessageBox.Show("修改密码成功", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        SecStaticInfo.User.Password = u.NewPassword;
                        btnSubmit.IsEnabled = false;
                        btnClear.IsEnabled = false;
                        txtNewPwd1.IsEnabled = false;
                        txtNewPwd2.IsEnabled = false;
                        SecStaticInfo.SendCommand(0);
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
        }
    }
}
