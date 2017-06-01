using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Security.Client.View;

namespace Security.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is System.Exception)
                {
                    HandleException((System.Exception)e.ExceptionObject);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                HandleException(e.Exception);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }


        public static void HandleException(Exception ex)
        {
            ////记录日志
            //if (!System.IO.Directory.Exists("Log"))
            //{
            //    System.IO.Directory.CreateDirectory("Log");
            //}
            //DateTime now = DateTime.Now;
            //string logpath = string.Format(@"Log\fatal_{0}{1}{2}.log", now.Year, now.Month, now.Day);
            //System.IO.File.AppendAllText(logpath, string.Format("\r\n----------------------{0}--------------------------\r\n", now.ToString("yyyy-MM-dd HH:mm:ss")));
            //System.IO.File.AppendAllText(logpath, ex.Message);
            //System.IO.File.AppendAllText(logpath, "\r\n");
            //System.IO.File.AppendAllText(logpath, ex.StackTrace);
            //System.IO.File.AppendAllText(logpath, "\r\n");

            string msg = string.Empty;
            msg = ex.Message;
            string caption = "发生未知错误。请与管理员联系以获取帮助。";
            if (ex.InnerException != null)
            {
                //System.IO.File.AppendAllText(logpath, ex.InnerException.Message);
                //System.IO.File.AppendAllText(logpath, "\r\n");
                //System.IO.File.AppendAllText(logpath, ex.InnerException.StackTrace);
                //System.IO.File.AppendAllText(logpath, "\r\n");

                msg += "\n" + ex.InnerException.Message;
                msg += "\n" + ex.InnerException.StackTrace;
            }
            //System.IO.File.AppendAllText(logpath, "\r\n----------------------footer--------------------------\r\n");

            MessageBox.Show(msg, caption);

            //msg = "\n是否退出程序？";
            //var rs = MessageBox.Show(msg, caption, MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes);
            //if (rs == MessageBoxResult.Yes)
            //{
            //    Application.Current.Shutdown();
            //}
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            FrmLogin frm = new FrmLogin();
            this.MainWindow = frm;
            frm.Show();
        }

    }
}
