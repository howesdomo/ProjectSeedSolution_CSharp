using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DL.Model;

namespace APP_Server.Models
{
    [Serializable]
    public class TestClass
    {
        public TestClass()
        {

        }

        public string OrderNo { get; set; }
        public User[] Cartons { get; set; }

        public List<string> StringList { get; set; }

        public String[] Strings { get; set; }
        public string[] SStrings { get; set; }

        public Int32[] Ints { get; set; }
        public int[] SInts { get; set; }

        public Int64[] Longs { get; set; }
        public long[] SLongs { get; set; }

        public Double[] Doubles { get; set; }
        public double[] SDoubles { get; set; }

        public Decimal[] Decimals { get; set; }
        public decimal[] SDecimals { get; set; }

        public Boolean[] Bools { get; set; }
        public bool[] SBools { get; set; }

        public DateTime TestDateTime { get; set; }
        public DateTime? TestDateTimeIsNull { get; set; }

        public DateTime[] DateTimes { get; set; }
        public List<DateTime> DateTimeList { get; set; }

        public DateTime?[] DateTimeIsNulls { get; set; }
        public List<DateTime?> DateTimeIsNullList { get; set; }
    }
}