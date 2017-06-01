using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Security.Client.ViewModel
{
    public class PermessionTreeViewModel : INotifyPropertyChanged
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    this.OnPropertyChanged("Name");
                    this.OnPropertyChanged("TreeName");
                }
            }
        }
        private string code;
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                if (code != value)
                {
                    code = value;
                    this.OnPropertyChanged("Code");
                    this.OnPropertyChanged("TreeName");
                }
            }
        }
        public List<PermessionTreeViewModel> Childrens { get; set; }

        public bool HasSubcomponents
        {
            get
            {
                return Childrens.Count > 0;
            }
        }

        public string TreeName
        {
            get
            {
                return GetCodeName(Code,Name);
            }
        }

        protected string GetCodeName(string code, string name)
        {
            string codeName = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    codeName = string.Format("{0}_{1}", code, name);
                }
                if (string.IsNullOrEmpty(codeName))
                {
                    codeName = code;
                }
            }

            if (string.IsNullOrEmpty(codeName))
            {
                codeName = name;
            }
            return codeName;
        }

        private bool? _shouldInstall;

        public bool? ShouldInstall
        {
            get
            {
                return _shouldInstall;
            }
            set
            {
                if (value != _shouldInstall)
                {
                    _shouldInstall = value;
                    OnPropertyChanged("ShouldInstall");
                }
            }
        }

        public PermessionTreeViewModel()
        {
            this.Childrens = new List<PermessionTreeViewModel>();
            ShouldInstall = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
