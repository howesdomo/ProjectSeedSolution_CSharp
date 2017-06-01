using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Security.Model
{
    public class Role
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

        public Role()
        { }

        public Role(string id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

    }
}
