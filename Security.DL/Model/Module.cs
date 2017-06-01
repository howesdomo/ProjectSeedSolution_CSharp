using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class Module
    {
        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Module()
        {
        }

        public Module(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }
    }
}
