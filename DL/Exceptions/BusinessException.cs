using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Model
{
    /// <summary>
    /// 业务逻辑报错
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string a)
            : base(a)
        {

        }
    }
}
