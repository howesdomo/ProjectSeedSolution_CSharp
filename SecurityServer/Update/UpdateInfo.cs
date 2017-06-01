using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecurityServer
{
    public class UpdateInfo : ReturnResult
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 更新内容项
        /// </summary>
        public Item[] Items { get; set; }

        public class Item
        {
            /// <summary>
            /// 来源路径
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// 目标路径
            /// </summary>
            public string[] LocalPaths { get; set; }

            /// <summary>
            /// 文件长度
            /// </summary>
            public long FileLen { get; set; }
        }
    }

    public enum EnumApplication
    {
        None,

        Client = 1,

        PDA = 2,

    }
}