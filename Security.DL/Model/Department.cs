using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class Department
    {
        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _parentid;
        public string ParentID
        {
            get
            {
                return _parentid;
            }
            set
            {
                _parentid = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _typeCode;
        public string TypeCode
        {
            get
            {
                return _typeCode;
            }
            set
            {
                _typeCode = value;
            }
        }
    }
}
