using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DL.DataAccess;

namespace DL.DataLogic
{
    public class TestBll
    {
        public DateTime GetDBTime()
        {
            return new TestDal().GetDBTime();
        }
    }
}
