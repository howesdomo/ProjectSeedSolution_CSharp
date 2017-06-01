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
using Security.Client.Common;
using Security.Client.ViewModel;
using System.Reflection;

namespace Security.Client.View
{
    /// <summary>
    /// FrmMain.xaml 的交互逻辑
    /// </summary>
    public partial class FrmMain : Window
    {
        public static BusyIndicator ucBusyIndicator;
        public static double opacity = 1;

        public FrmMain()
        {
            InitializeComponent();
            
            this.initUI();
            this.initEvent();
        }

        private void initUI()
        {
            this.WindowState = WindowState.Maximized;
            ucBusyIndicator = new BusyIndicator();
            ucBusyIndicator.IsBusy = false;
            LayoutRoot.Children.Add(ucBusyIndicator);
            this.Title = this.Title + " " + SecStaticInfo.SystemVersion;
            lblToday.Text = "当前系统日期: " + DateTime.Now.ToLongDateString();
            lblLoginUser.Text = "当前登录用户: " + SecStaticInfo.User.UserName;
            lblState.Text = SecStaticInfo.SystemWelcomeInfo;
            changeOpacity = new delegateOpacity(changeOpa);
        }

        private void initEvent()
        {
            this.listBox1.SelectionChanged += new SelectionChangedEventHandler(listBox1_SelectionChanged);
        }

        public delegate void delegateOpacity();
        public static delegateOpacity changeOpacity;

        void changeOpa()
        {
            this.Opacity = opacity;
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            fullScreen();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            ListBoxItem item = listBox1.SelectedItem as ListBoxItem;
            Assembly ass = Assembly.GetExecutingAssembly();
            MainContent.Content = ass.CreateInstance(item.Tag.ToString());
            lblState.Text = SecStaticInfo.SystemWelcomeInfo;

        }

        private void fullScreen()
        {
            if (!FullScreenHelper.IsFullscreen(this))
            {
                FullScreenHelper.GoFullscreen(this);
            }
            else
            {
                FullScreenHelper.ExitFullscreen(this);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                fullScreen();
            }
        }
    }
}
