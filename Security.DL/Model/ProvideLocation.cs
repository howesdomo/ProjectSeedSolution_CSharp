using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{

    public class ProvideLocation
    {
        private string _id;
        private string _LocationCode1;
        private string _LocationName1;
        private string _LocationCode2;
        private string _LocationName2;
        private string _LocationCode3;
        private string _LocationName3;
        private string _LocationCode4;
        private string _LocationName4;
        private string _LocationCode5;
        private string _LocationName5;

        public string DisplayName { get; set; }

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

        public string LocationCode1
        {
            get
            {
                return _LocationCode1;
            }
            set
            {
                _LocationCode1 = value;
            }
        }

        public string LocationName1
        {
            get
            {
                return _LocationName1;
            }
            set
            {
                _LocationName1 = value;
            }
        }

        public string LocationCode2
        {
            get
            {
                return _LocationCode2;
            }
            set
            {
                _LocationCode2 = value;
            }
        }

        public string LocationName2
        {
            get
            {
                return _LocationName2;
            }
            set
            {
                _LocationName2 = value;
            }
        }

        public string LocationCode3
        {
            get
            {
                return _LocationCode3;
            }
            set
            {
                _LocationCode3 = value;
            }
        }

        public string LocationName3
        {
            get
            {
                return _LocationName3;
            }
            set
            {
                _LocationName3 = value;
            }
        }

        public string LocationCode4
        {
            get
            {
                return _LocationCode4;
            }
            set
            {
                _LocationCode4 = value;
            }
        }

        public string LocationName4
        {
            get
            {
                return _LocationName4;
            }
            set
            {
                _LocationName4 = value;
            }
        }

        public string LocationCode5
        {
            get
            {
                return _LocationCode5;
            }
            set
            {
                _LocationCode5 = value;
            }
        }

        public string LocationName5
        {
            get
            {
                return _LocationName5;
            }
            set
            {
                _LocationName5 = value;
            }
        }

        public int LocationTypeID_1
        {
            get;
            set;
        }

        public int LocationTypeID_2
        {
            get;
            set;
        }

        public int LocationTypeID_3
        {
            get;
            set;
        }

        public int LocationTypeID_4
        {
            get;
            set;
        }

        public int LocationTypeID_5
        {
            get;
            set;
        }
    }
}
