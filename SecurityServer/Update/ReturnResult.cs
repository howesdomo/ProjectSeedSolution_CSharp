using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecurityServer
{
    public class ReturnResult
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
    }
}