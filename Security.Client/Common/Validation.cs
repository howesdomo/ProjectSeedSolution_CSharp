using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Security.Client.Common
{
    public class IsNullOrEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "不能为空！");
            }
            else if (value.ToString().Length >= 25)
            {
                return new ValidationResult(false, "名字过长！");
            }
            else
            {
                return new ValidationResult(true, null);
            }
           
        }
    }
}
