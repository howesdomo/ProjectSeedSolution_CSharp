using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class Location
    {
        private string _id;
        private string _parentid;
        private int _locationTypeId;
        private string _name;
        private string _code;

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

        public int LocationTypeID
        {
            get
            {
                return _locationTypeId;
            }
            set
            {
                _locationTypeId = value;
            }
        }

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

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        public string CompanyCode
        {
            get;
            set;
        }

        public string ProvideLocationID
        {
            get;
            set;
        }

        public string ParentCode { get; set; }

        public int ParentTypeID { get; set; }

        public decimal _Capacity;
        public decimal Capacity
        {
            get
            {
                return _Capacity;
            }
            set
            {
                _Capacity = value;
            }
        }

        public string _UM;
        public string UM
        {
            get
            {
                return _UM;
            }
            set
            {
                _UM = value;
            }
        }

        public string _UMName;
        public string UMName
        {
            get
            {
                return _UMName;
            }
            set
            {
                _UMName = value;
            }
        }
    }
}
