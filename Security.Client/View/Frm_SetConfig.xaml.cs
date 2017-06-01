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

namespace Security.Client.View
{
    /// <summary>
    /// Frm_SetConfig.xaml 的交互逻辑
    /// </summary>
    public partial class Frm_SetConfig : Window
    {
        Common.BusyIndicator busyIndicator = new Common.BusyIndicator();
        public Frm_SetConfig()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            this.Loaded += new RoutedEventHandler(Frm_SetConfig_Loaded);
            busyIndicator.IsBusy = false;
            LayoutRoot.Children.Add(busyIndicator);
        }

        void Frm_SetConfig_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtIP.Text = Common.Config.GetServerIP();
            this.txtPort.Text = Common.Config.GetServerPort();
            this.txtApplication.Text = Common.Config.GetServerApplication();

            closeBusy = new delegateBusy(closeBusyIndicator);
        }

        static string ip;
        static string port;
        static string app;

        //static string pducIp;
        //static string pducPort;
        //static string pducApp;

        private void btnTestServer_Click(object sender, RoutedEventArgs e)
        {
            ip = this.txtIP.Text.Trim();
            port = this.txtPort.Text.Trim();
            app = this.txtApplication.Text.Trim();
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(app))
            {
                MessageBox.Show("请配置完成后再测试连接！");

                return;
            }

            busyIndicator.IsBusy = true;
            testOK = false;
            thread = new System.Threading.Thread(testServer);
            thread.IsBackground = true;
            thread.Start();
        }

        System.Threading.Thread thread;
        void testServer()
        {
            try
            {
                Common.SecurityServiceLink client = new Common.SecurityServiceLink(ip, port, app);

                testOK = client.CheckServiceConnection();
                MessageBox.Show("连接成功", "连接成功", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception)
            {
                testOK = false;
                MessageBox.Show("连接失败", "连接失败", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            this.Dispatcher.BeginInvoke(closeBusy);
        }
        static bool testOK = false;

        delegate void delegateBusy();
        delegateBusy closeBusy;
        void closeBusyIndicator()
        {
            busyIndicator.IsBusy = false;
            this.btnSave.IsEnabled = testOK;
        }

        private void btnTestPudc_Click(object sender, RoutedEventArgs e)
        {
            //pducIp = this.txtPducIP.Text.Trim();
            //pducPort = this.txtPducPort.Text.Trim();
            //pducApp = this.txtPducApplication.Text.Trim();
            //if (string.IsNullOrEmpty(pducIp) || string.IsNullOrEmpty(pducPort) || string.IsNullOrEmpty(pducApp))
            //{
            //    MessageBox.Show("请配置完成后再测试连接！");

            //    return;
            //}

            busyIndicator.IsBusy = true;
            testOK2 = false;
            thread2 = new System.Threading.Thread(testServer2);
            thread2.IsBackground = true;
            thread2.Start();
        }

        System.Threading.Thread thread2;
        void testServer2()
        {
            try
            {
                Common.SecurityServiceLink sec =  new Common.SecurityServiceLink(ip, port, app);
                //Common.ProductionServiceLink pduc = new Common.ProductionServiceLink(pducIp, pducPort, pducApp);
                sec.CheckServiceConnection();
                //pduc.CheckServiceConnection();


                testOK2 = true;
                MessageBox.Show("连接成功", "连接成功", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception)
            {
                testOK2 = false;
                MessageBox.Show("连接失败", "连接失败", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }

            this.Dispatcher.BeginInvoke(closeBusy);
        }
        static bool testOK2 = false;

        string isOK()
        {
            ip = this.txtIP.Text.Trim();
            port = this.txtPort.Text.Trim();
            app = this.txtApplication.Text.Trim();

            //pducIp = this.txtPducIP.Text.Trim();
            //pducPort = this.txtPducPort.Text.Trim();
            //pducApp = this.txtPducApplication.Text.Trim();

            var msg = "";

            if (string.IsNullOrWhiteSpace(ip))
            {
                msg += "请输入SecurityServerIP\n";
            }
            if (string.IsNullOrWhiteSpace(port))
            {
                msg += "请输入SecurityServerPort\n";
            }
            if (string.IsNullOrWhiteSpace(app))
            {
                msg += "请输入SecurityServerApplication\n";
            }

            //if (string.IsNullOrWhiteSpace(pducIp))
            //{
            //    msg += "请输入PductionServerIP\n";
            //}
            //if (string.IsNullOrWhiteSpace(pducPort))
            //{
            //    msg += "请输入PductionServerPort\n";
            //}
            //if (string.IsNullOrWhiteSpace(pducApp))
            //{
            //    msg += "请输入PductionServerApplication\n";
            //}

            return msg;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var msg = isOK();
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                return;
            }
            try
            {
                Common.Config.SetServer(ip, port, app);
                MessageBox.Show("保存成功", "成功", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
