using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP_Server.Models
{
    public class SOAPResult
    {
        public SOAPResult()
        {

        }

        public bool IsComplete  { get; set; }

        public string ExceptionInfo { get; set; }

        #region 业务逻辑

        /// <summary>
        /// 业务逻辑运行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 业务逻辑报错信息
        /// </summary>
        public string BusinessExceptionInfo { get;set;}

        /// <summary>
        /// 返回内容
        /// </summary>
        public string ReturnObjectJson { get; set; }

        #endregion
    }
}