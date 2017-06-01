using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Security.Client.ViewModel;
using Security.Client.ViewModel;
using Security.Client.View;

namespace Security.Client.Common
{
     /// <summary>
    /// 记录所有全局静态变量的存储类
    /// </summary>
    public static class SecStaticInfo
    {
        public delegate void OnStateChange(string msg);
        public delegate void MainFormCommand(int cmd);
        /// <summary>
        /// 向主菜单发送指令
        /// </summary>
        public static event MainFormCommand DoCommand;
        /// <summary>
        /// 设置状态栏内容
        /// </summary>
        public static event OnStateChange StateChange;

        public static FrmMain FrmMain;

        /// <summary>
        /// 记录当前登录用户
        /// </summary>
        public static UserModel User { get; set; }
        /// <summary>
        /// 系统欢迎信息:
        /// 欢迎使用Enpot权限管理系统 v 1.2 beta
        /// </summary>
        public static string SystemWelcomeInfo
        {
            get { return "欢迎使用权限管理系统 " + SecStaticInfo.SystemVersion; }
        }
        /// <summary>
        /// 系统版本号
        /// </summary>
        public static string SystemVersion
        {
            get { return "V" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
        /// <summary>
        /// 修改状态栏内容
        /// </summary>
        /// <param name="msg"></param>
        public static void SetStateInfo(string msg)
        {
            if (StateChange != null)
                StateChange(msg);
        }
        /// <summary>
        /// 修改状态栏内容，并持续显示一段时间
        /// 时间结束后状态栏内容将变回 StaticInfo.SystemWelcomeInfo 的值
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="sec">持续显示时间(秒)</param>
        public static void SetStateInfo(string msg, int sec)
        {
            if (StateChange != null)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(sec * 1000);
                timer.Tick += (o, e) =>
                    {
                        StateChange(SecStaticInfo.SystemWelcomeInfo);
                        timer.Stop();
                        timer = null;
                    };
                timer.Start();
                StateChange(msg);
            }
        }
        /// <summary>
        /// 修改状态栏内容，并持续显示一段时间
        /// 时间结束后将显示另一段内容提示
        /// </summary>
        /// <param name="msg">初始状态栏内容</param>
        /// <param name="msg2">时间结束后显示的内容</param>
        /// <param name="sec">初始内容持续显示时间</param>
        public static void SetStateInfo(string msg, string msg2, int sec)
        {
            if (StateChange != null)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(sec * 1000);
                timer.Tick += (o, e) =>
                {
                    StateChange(msg2);
                    timer.Stop();
                    timer = null;
                };
                timer.Start();
                StateChange(msg);
            }
        }
        /// <summary>
        /// 向主菜单窗口发送指令
        /// </summary>
        /// <param name="cmd"></param>
        public static void SendCommand(int cmd)
        {
            if (DoCommand != null)
                DoCommand(cmd);
        }
    }
}
