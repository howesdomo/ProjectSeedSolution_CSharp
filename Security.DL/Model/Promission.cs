using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class Permission
    {


        public string PermissionID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string ModuleID
        {
            get;
            set;
        }

        public string ModuleName
        {
            get;
            set;
        }

        public int Seq 
        {
            get; 
            set; 
        }

        public string ClassName
        {
            get;
            set;
        }

        public Permission()
        { }

        public Permission(string id, string name, string code, string className)
        {
            this.PermissionID = id;
            this.Name = name;
            this.Code = code;
            this.ClassName = className;
        }

        public Permission(string id, string name, string code, string className, int seq)
        {
            this.PermissionID = id;
            this.Name = name;
            this.Code = code;
            this.ClassName = className;
            this.Seq = seq;
        }
    }
}
