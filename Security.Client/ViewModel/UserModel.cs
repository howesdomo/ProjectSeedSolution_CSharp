using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Collections.ObjectModel;
using Security.Client.Common;
using Security.Client.SecurityServer;

namespace Security.Client.ViewModel
{
    public class UserModel : UserMTR, IDataErrorInfo
    {
        public bool Vis()
        {
            bool level = true;
            if (string.IsNullOrEmpty(this.UserName))
            {
                SetError("UserName", "请输入名称");
                level = false;
            }
            else
            {
                ClearError("UserName");
            }
            if (string.IsNullOrEmpty(this.LoginAccount))
            {
                SetError("LoginAccount", "请输入登录账号");
                level = false;
            }
            else
            {
                ClearError("LoginAccount");
            }
            if (level)
            {
                if (this.UserName.Length >= 25)
                {
                    SetError("UserName", "名称过长");
                    level = false;
                }
                else
                {
                    ClearError("UserName");
                }
                if (this.LoginAccount.Length >= 25)
                {
                    SetError("LoginAccount", "登陆名过长");
                    level = false;
                }
                else
                {
                    ClearError("LoginAccount");
                }
            }
            return level;

        }

        public bool CheckLogin()
        {
            bool level = true;
            if (string.IsNullOrEmpty(this.Password))
            {
                SetError("Password", "请输入密码");
                level = false;
            }
            else
            {
                ClearError("Password");
            }
            if (string.IsNullOrEmpty(this.LoginAccount))
            {
                SetError("LoginAccount", "请输入登录账号");
                level = false;
            }
            else
            {
                ClearError("LoginAccount");
            }
            return level;
        }

        public UserMTR ToUser()
        {
            return new UserMTR()
            {
                Effectiveness = this.Effectiveness,
                ID = this.ID,
                CreatorID = this.CreatorID,
                LastUpdateDatetime = this.LastUpdateDatetime,
                LoginAccount = this.LoginAccount,
                Password = this.Password,
                UserName = this.UserName,
                UserRole = this.UserRole

            };

        }

        public static UserModel ConvertToChild(UserMTR u)
        {
            UserModel uv = new UserModel();
            uv.CreatorID = u.CreatorID;
            uv.Effectiveness = u.Effectiveness;
            uv.ID = u.ID;
            uv.LastUpdateDatetime = u.LastUpdateDatetime;
            uv.LoginAccount = u.LoginAccount;
            uv.Password = u.Password;
            uv.UserName = u.UserName;
            uv.UserRole = u.UserRole;
            uv.UserLocationRelation = u.UserLocationRelation;
            return uv;
        }

        public static ObservableCollection<UserModel> ConvertToList(ObservableCollection<UserMTR> l)
        {
            ObservableCollection<UserModel> list = new ObservableCollection<UserModel>();
            foreach (UserMTR r in l)
            {
                list.Add(UserModel.ConvertToChild(r));
            }
            return list;
        }

        public static UserModel ConvertROUser(RO_User su)
        {
            UserModel uv = new UserModel();
            uv.Effectiveness = su.Effectiveness;
            uv.CreatorID = SecStaticInfo.User.ID;
            uv.ID = su.UserID;
            uv.LastUpdateDatetime = DateTime.Now;
            uv.LoginAccount = su.LoginAccount;
            uv.Password = su.Password;
            uv.UserName = su.UserName;
            uv.UserRole = su.UserRole;
            return uv;
        }

        public UserModel Clone()
        {
            UserModel r = new UserModel
            {
                Effectiveness = this.Effectiveness,
                ID = this.ID,
                CreatorID = this.CreatorID,
                LastUpdateDatetime = this.LastUpdateDatetime,
                LoginAccount = this.LoginAccount,
                Password = this.Password,
                UserName = this.UserName
            };
            UserRole[] temp = new UserRole[this.UserRole.Count()];
            this.UserRole.CopyTo(temp, 0);
            if (r.UserRole == null)
                r.UserRole = new UserRole[] { };
            r.UserRole = temp;

            UserLocationRelation[] temp2 = new UserLocationRelation[this.UserLocationRelation.Count()];
            this.UserLocationRelation.CopyTo(temp2, 0);
            if (r.UserLocationRelation == null)
                r.UserLocationRelation = new UserLocationRelation[] { };
            r.UserLocationRelation = temp2;

            //foreach (var item in temp)
            //{
            //    r.UserRole.Add(item);
            //}
            return r;
        }

        #region 修改密码专用属性、方法
        private string newPassword = "";

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                if (!this.newPassword.Equals(value))
                {
                    newPassword = value;
                    this.RaisePropertyChanged("NewPassword");
                }
            }
        }
        private string newPassword2 = "";

        public string NewPassword2
        {
            get { return newPassword2; }
            set
            {
                if (!this.newPassword2.Equals(value))
                {
                    newPassword2 = value;
                    this.RaisePropertyChanged("NewPassword2");
                }
            }
        }

        public bool CheckChangePwd()
        {
            bool level = true;
            if (string.IsNullOrEmpty(this.NewPassword))
            {
                SetError("NewPassword", "请输入新密码");
                level = false;
            }
            else
            {
                ClearError("NewPassword");
            }
            if (string.IsNullOrEmpty(this.NewPassword2))
            {
                SetError("NewPassword2", "请输入确认密码");
                level = false;
            }
            else
            {
                ClearError("NewPassword2");
            }
            return level;
        }
        #endregion

        #region Error接口实现
        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (this.errors.ContainsKey(columnName))
                {
                    return this.errors[columnName];
                }
                return string.Empty;
            }
        }



        public Dictionary<string, string> errors = new Dictionary<string, string>();

        public void SetError(string propertyName, string errorMessage)
        {
            errors[propertyName] = errorMessage;
            this.RaisePropertyChanged(propertyName);
        }

        private void ClearError(string propertyName)
        {
            this.errors.Remove(propertyName);
        }

        private void ClearAllErrors()
        {
            List<string> properties = new List<string>();
            foreach (KeyValuePair<string, string> error in this.errors)
            {
                properties.Add(error.Key);
            }
            this.errors.Clear();
            foreach (string property in properties)
            {
                this.RaisePropertyChanged(property);
            }
        }
        #endregion

        public string UserCodeName
        {
            get
            {
                return string.Format("{0} - {1}", base.LoginAccount, base.UserName);
            }
        }
    }
}
