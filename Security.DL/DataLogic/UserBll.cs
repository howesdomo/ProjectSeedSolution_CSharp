using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.DataAccess;
using Security.Model;

namespace Security.DataLogic
{
    public class UserBll
    {

        private UserDal _userDal;


        public UserBll()
        {
            this._userDal = new UserDal();
        }

        public User Login(string account, string password, string companyCode)
        {
            User result = null;

            result = this._userDal.GetUserForLogin(account, password, companyCode);

            if (result == null || string.IsNullOrEmpty(result.ID))
            {
                return result;
            }
            
            LocationDal locationDal = new LocationDal();

            result.LocationList = locationDal.GetLocationListByUser(result.ID, result.CompanyCode);
            if (!string.IsNullOrEmpty(result.ID))
            {
                result.AllLocationList = locationDal.GetAllLocationListByUser(result.ID);
            }
            RoleDal roleDal = new RoleDal();
            result.RoleList = roleDal.GetRoleListByUserID(result.ID);

            ModuleDal moduleDal = new ModuleDal();
            result.PermissionList = moduleDal.GetPromissionListForLogin(result.RoleList);

            return result;
        }

        public User ChangePassword(string account, string oldPassword, string newPassword, string companyCode)
        {
            string pw = oldPassword;
            int count = this._userDal.ChangePassword(account, oldPassword, newPassword, companyCode);
            if (count > 0)
            {
                pw = newPassword;
            }
            return this.Login(account, pw, companyCode);
        }

        public List<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            return this._userDal.GetUserByUserName(userName);
        }

        public Module Module
        {
            get;
            set;
        }

        public Permission Promission
        {
            get;
            set;
        }

    }
}
