using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class ExceptionUtil
    {
        public static string GetExceptionInfo(Exception ex)
        {
            return ex.Message + "\r\n" + ex.StackTrace;
        }

        public static void GetExceptionFullInfo(Exception ex, StringBuilder sb, int level = 1)
        {
            if (string.IsNullOrEmpty(sb.ToString()) == false)
            {
                sb.AppendLine("************** Inner Exception " + level + "**************");
            }

            sb.AppendLine(ex.Message + "\r\n" + ex.StackTrace);

            if (ex.InnerException != null)
            {
                level = level + 1;
                GetExceptionFullInfo(ex.InnerException, sb, level);
            }
        }

        /// <summary>
        /// 反射方式调用WebService, 去掉 第一层错误 调用的目标发生了异常。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sb"></param>
        /// <param name="level"></param>
        public static void GetExceptionFullInfoForInvoke(Exception ex, StringBuilder sb, int level = 1)
        {
            if (ex.Message.Equals("调用的目标发生了异常。") == false) // 跳过调用目标发生异常
            {
                sb.AppendLine(ex.Message + "\r\n" + ex.StackTrace);
                level = level + 1;
            }

            if (ex.InnerException != null)
            {
                GetExceptionFullInfo(ex.InnerException, sb, level);
            }
        }
    }
}
