using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class CodeHelper
    {
        public static Tuple<string, string, string> SplitLocationCode(string code)
        {
            string area = code.Substring(0, 2);
            string row = code.Substring(2, 2);
            string cell = code.Substring(5, 2);
            return new Tuple<string, string, string>(area, row, cell);
        }

    }
}
