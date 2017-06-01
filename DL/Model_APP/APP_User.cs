using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DL.Model
{
    [Serializable]
    public class User
    {
        public User()
        {

        }

        public string ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string CompanyCode { get; set; }

        public List<Permission> PermissionList { get; set; }
    }

    [Serializable]
    public class Permission
    {
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Seq { get; set; }
        public string ClassName { get; set; }

        // 所属功能组别
        public string ModuleID { get; set; }
        public string ModuleName { get; set; }
    }
}
