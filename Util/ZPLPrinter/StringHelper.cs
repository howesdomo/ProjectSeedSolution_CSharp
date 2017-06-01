using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ZPLPrinter
{
    public class StringHelper
    {
        /// <summary>
        /// 统计字符串长度
        /// </summary>
        /// <param name="content">待统计字符串信息</param>
        /// <returns></returns>
        public static int CountStringLength(string content)
        {
            int charCount = 0;
            for (int i = 0; i < content.Length; i++)
            {
                char j = content[i];
                ushort s = j;
                if (s >= 0x4E00 && s <= 0x9FA5) { charCount += 2; } // 中文的编码的+2
                else { charCount += 1; } // 非中文编码的+1
            }
            return charCount;
        }
    }
}
