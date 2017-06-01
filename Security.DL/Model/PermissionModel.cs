using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class PermissionModel
    {
        public string ID { get; set; }
        public string ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string PermissionName { get; set; }
        public string SysCode { get; set; }
        public int Seq { get; set; }
    }
}
