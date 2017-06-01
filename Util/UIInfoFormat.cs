using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class UIInfoFormat
    {
        public static string GetCodeName(string a, string b)
        {
            return string.Format("{0} - {1}", a, b);
        }

        public static string GetUIDateString(DateTime d)
        {
            return d.ToString("yyyy-MM-dd");
        }

        public static string GetUIDateString(DateTime? d)
        {
            if (d.HasValue)
            {
                return d.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetUIDateTimeString(DateTime d)
        {
            return d.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetUIDateTimeString(DateTime? d)
        {
            if (d.HasValue)
            {
                return d.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetUITimeSpanDurationString(TimeSpan t)
        {
            string r = string.Empty;
            if (t.TotalMinutes >= TimeSpan.FromHours(1).TotalSeconds)
            {
                r = t.ToString();
            }
            else
            {
                r = t.ToString().Substring(3, t.ToString().Length - 3);
            }

            if (r.Contains('.'))
            {
                r = r.Substring(0, r.IndexOf('.'));
            }

            return r;
        }

        public static string GetUIIntString(int d)
        {
            if (d == 0)
            {
                return string.Empty;
            }
            else
            {
                return d.ToString();
            }
        }

        public static string GetUIIntString(int? d)
        {
            if (d.HasValue)
            {
                if (d == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return d.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetUIIntString(decimal d)
        {
            if (d == 0)
            {
                return string.Empty;
            }
            else
            {
                return d.ToString();
            }
        }


        public static string GetUIDecimalString(decimal d)
        {
            return d.ToString("#,##0.##");
        }
    }
}
