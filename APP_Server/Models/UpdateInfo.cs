using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP_Server.Models
{
    public class UpdateInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string version;
        /// <summary>
        /// 下载URL
        /// </summary>
        public string url;
        /// <summary>
        /// 描述
        /// </summary>
        public string description;

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