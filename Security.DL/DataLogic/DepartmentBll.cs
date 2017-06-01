using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.Model;
using Security.DataAccess;
using EML.Util;
namespace Security.DataLogic
{
   public class DepartmentBll
    {

        private DepartmentDal dal;
        public DepartmentBll()
        {
            this.dal = new DepartmentDal();
        }

       public List<Department> GetDepartmentTreeList()
       {
           return this.dal.GetDepartmentTreeList();
       }
    }
}
