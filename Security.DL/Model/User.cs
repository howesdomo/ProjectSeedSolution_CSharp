using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Security.Model
{
    public class User
    {

        public string ID
        {
            get;
            set;
        }

        public string Account
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string CompanyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 根据CompanyCode和UserID获取的权限地点
        /// </summary>
        public List<Location> LocationList
        {
            get;
            set;
        }

        /// <summary>
        /// 根据UserID获取所有的权限地点
        /// </summary>
        public List<Location> AllLocationList
        {
            get;
            set;
        }


        public List<Role> RoleList
        {
            get;
            set;
        }

        //public Dictionary<Module, List<Promission>> PromissionList
        //{
        //    get;
        //    set;
        //}

        public List<Permission> PermissionList
        {
            get;
            set;
        }

        public User()
        { 
        }

        public User(string id, string account, string password, string username)
        {
            this.ID = id;
            this.Account = account;
            this.Password = password;
            this.UserName = username;
            this.CompanyCode = string.Empty;
        }


        public List<Department> DepartmentList
        {
            get;
            set;
        }
    }
}
