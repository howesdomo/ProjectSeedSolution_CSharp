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

using Security.Client.ViewModel;
using Security.Client.Common;
using Security.Client.SecurityServer;

namespace Security.Client.View
{
    /// <summary>
    /// FrmLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FrmLogin : Window
    {
        BusyIndicator ucBusyIndicator = new BusyIndicator();
        public FrmLogin()
        {
            InitializeComponent();
            ucBusyIndicator.IsBusy = false;
            LayoutRoot.Children.Add(ucBusyIndicator);
            init();
        }

        private void init()
        {
            this.DataContext = um;
            this.textBlock1.Text = SecStaticInfo.SystemVersion;
            this.Title = this.Title + " " + SecStaticInfo.SystemVersion;
            this.txtLoginName.Focus();
            doLogin = new DelegateLogin(stopBusyIndicator);
            doOpenMain = new DelegateOpenMain(openMain);
        }

        /// <summary>
        /// 绑定的委托
        /// </summary>
        delegate void DelegateLogin();
        /// <summary>
        /// 绑定的委托事件
        /// </summary>
        private DelegateLogin doLogin;
        System.Threading.Thread thread;
        private void startLogin()
        {
            if (um.CheckLogin())
            {
                this.btnLogin.IsEnabled = false;
                ucBusyIndicator.IsBusy = true;
                loginName = this.txtLoginName.Text.Trim();
                pwd = this.txtPwd.Password;
                //try
                //{
                    thread = new System.Threading.Thread(Login);


                    thread.IsBackground = true;
                    thread.Start();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                //    this.stopBusyIndicator();
                //}
            }
        }

        void stopBusyIndicator()
        {
            txtPwd.Password = "";
            btnLogin.IsEnabled = true;
            ucBusyIndicator.IsBusy = false;
        }

        delegate void DelegateOpenMain();
        private DelegateOpenMain doOpenMain;

        private void openMain()
        {
            SecStaticInfo.FrmMain = new FrmMain();

            SecStaticInfo.FrmMain.Show();

            this.Close();
        }

        UserModel um = new UserModel();

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            um.Password = txtPwd.Password;
            //Login();
            startLogin();
        }

        string loginName;
        string pwd;

        private void Login()
        {
            try
            {
                SecurityServiceLink client = new SecurityServiceLink();


                var user = client.LoginSecurityMgrSys(loginName, pwd);
                if (user != null)
                {
                    bool level = true;

                    foreach (UserRole ur in user.UserRole)
                    {
                        if (ur.RO_RoleMTR.IsAdminstrator.HasValue && ur.RO_RoleMTR.IsAdminstrator.Value)
                        {
                            level = false;
                            break;
                        }
                    }

                    if (level) 
                    {

                        MessageBox.Show("登录失败，您没有足够权限登录本系统！", "失败", MessageBoxButton.OK, MessageBoxImage.Error);

                        this.Dispatcher.BeginInvoke(doLogin);
                        //this.btnLogin.IsEnabled = true;
                        return;
                    }

                    SecStaticInfo.User = UserModel.ConvertToChild(user);


                    this.Dispatcher.BeginInvoke(doOpenMain);

                }
                else
                {

                    MessageBox.Show("用户名或密码错误，请重新登录!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    //txtPwd.Password = "";
                    //btnLogin.IsEnabled = true;
                    this.Dispatcher.BeginInvoke(doLogin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Dispatcher.BeginInvoke(doLogin);
            }

           
        }

        private void txtPwd_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                um.Password = txtPwd.Password;
                //Login();
                startLogin();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSetConfig_Click(object sender, RoutedEventArgs e)
        {
            View.Frm_SetConfig frm = new View.Frm_SetConfig();
            frm.Owner = this;
            frm.ShowDialog();
        }
    }
}
