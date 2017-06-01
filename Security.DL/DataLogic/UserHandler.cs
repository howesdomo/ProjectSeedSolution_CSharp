using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.Model;
using EML.Util;
using Security.DL.DataSources;

namespace Security.DataLogic
{
    public class UserHandler
    {
        string _conn;
        public UserHandler(string conn)
        {
            _conn = conn;
        }

        public UserMTR Login(string LoginAccount, string Password)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            //Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
            Password = CryptUtil.Encrypt(Password);

            var u = UserStruct.UserMTR.FirstOrDefault(p => p.LoginAccount == LoginAccount && p.Password == Password);
            if (u != null)
            {
                u.Password = CryptUtil.Decrypt(u.Password);
                return u;
            }
            else
            {
                return null;
            }
        }

        public ReturnMessage AddUser(UserMTR pUser, List<UserRole> newList, List<UserLocationRelation> userLocationList)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                if (ExistLoginAccount(pUser))
                {
                    objReturn.Code = 201;
                    objReturn.Message = "登录名已存在。";
                }
                else
                {
                    foreach (var a in newList)
                    {
                        UserRole rp = new UserRole();
                        rp.RoleID = a.ID;
                        rp.ID = Guid.NewGuid();
                        rp.UserMTR = pUser;
                        rp.LastUpdateDatetime = DateTime.Now;
                        pUser.UserRole.Add(rp);
                    }

                    if (userLocationList != null)
                    {
                        foreach (UserLocationRelation model in userLocationList)
                        {
                            model.LastUpdateDatetime = System.DateTime.Now;
                            model.ID = Guid.NewGuid();
                            pUser.UserLocationRelation.Add(model);
                        }
                    }

                    //pUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pUser.Password, "MD5");
                    pUser.ID = Guid.NewGuid();
                    pUser.Password = CryptUtil.Encrypt(pUser.Password);
                    pUser.LastUpdateDatetime = DateTime.Now;
                    UserStruct.UserMTR.InsertOnSubmit(pUser);
                    UserStruct.SubmitChanges();
                    objReturn.Code = 1;
                }
            }
            catch (Exception ex)
            {
                objReturn.Code = 202;
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

        public ReturnMessage AddUser(UserMTR pUser, List<UserRole> newList, List<UserDepartmentRelation> userDepartmentList)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                if (ExistLoginAccount(pUser))
                {
                    objReturn.Code = 201;
                    objReturn.Message = "登录名已存在。";
                }
                else
                {
                    foreach (var a in newList)
                    {
                        UserRole rp = new UserRole();
                        rp.RoleID = a.ID;
                        rp.ID = Guid.NewGuid();
                        rp.UserMTR = pUser;
                        rp.LastUpdateDatetime = DateTime.Now;
                        pUser.UserRole.Add(rp);
                    }

                    if (userDepartmentList != null)
                    {
                        foreach (UserDepartmentRelation model in userDepartmentList)
                        {
                            model.LastUpdateTime = System.DateTime.Now;
                            model.ID = Guid.NewGuid();
                            pUser.UserDepartmentRelation.Add(model);
                        }
                    }

                    //pUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(pUser.Password, "MD5");
                    pUser.ID = Guid.NewGuid();
                    pUser.Password = CryptUtil.Encrypt(pUser.Password);
                    pUser.LastUpdateDatetime = DateTime.Now;
                    UserStruct.UserMTR.InsertOnSubmit(pUser);
                    UserStruct.SubmitChanges();
                    objReturn.Code = 1;
                }
            }
            catch (Exception ex)
            {
                objReturn.Code = 202;
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

        public ReturnMessage ModifyUser(UserMTR pUser, List<UserRole> newList, List<UserRole> delList, List<UserLocationRelation> userLocationList)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                if (ExistLoginAccount(pUser))
                {
                    objReturn.Code = 201;
                    objReturn.Message = "登录名已存在。";
                }
                else
                {
                    var user = (from s in UserStruct.UserMTR
                                where s.ID == pUser.ID
                                select s).First();
                    user.UserName = pUser.UserName;
                    user.Effectiveness = pUser.Effectiveness;
                    user.LastUpdateDatetime = DateTime.Now;

                    if (pUser.Password == "123456")
                    {
                        string pw = CryptUtil.Encrypt(pUser.Password);
                        if (pw != user.Password)
                        {
                            user.Password = pw;
                        }
                    }
                   
                    user.LoginAccount = pUser.LoginAccount;

                    if (newList != null)
                    {
                        foreach (var a in newList)
                        {
                            UserRole rp = new UserRole();
                            rp.RoleID = a.ID;
                            rp.ID = Guid.NewGuid();
                            rp.UserMTR = user;
                            rp.LastUpdateDatetime = DateTime.Now;
                            user.UserRole.Add(rp);
                        }
                    }

                    if (delList != null)
                    {
                        foreach (var a in delList)
                        {
                            var r = (from s in UserStruct.UserRole
                                     where s.RO_RoleMTR.ID == a.ID && s.UserID == user.ID
                                     select s).First();
                            UserStruct.UserRole.DeleteOnSubmit(r);
                        }
                    }

                    if (userLocationList != null)
                    {
                        UserStruct.UserLocationRelation.DeleteAllOnSubmit<UserLocationRelation>(user.UserLocationRelation);
                        foreach (UserLocationRelation model in userLocationList)
                        {
                            model.LastUpdateDatetime = System.DateTime.Now;
                            model.ID = Guid.NewGuid();
                            user.UserLocationRelation.Add(model);
                        }
                    }



                    UserStruct.SubmitChanges();
                    objReturn.Code = 1;
                }
            }
            catch (Exception ex)
            {
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }


        public ReturnMessage ModifyUser(UserMTR pUser, List<UserRole> newList, List<UserRole> delList, List<UserDepartmentRelation> userDepartmentList)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                if (ExistLoginAccount(pUser))
                {
                    objReturn.Code = 201;
                    objReturn.Message = "登录名已存在。";
                }
                else
                {
                    var user = (from s in UserStruct.UserMTR
                                where s.ID == pUser.ID
                                select s).First();
                    user.UserName = pUser.UserName;
                    user.Effectiveness = pUser.Effectiveness;
                    user.LastUpdateDatetime = DateTime.Now;

                    if (pUser.Password == "123456")
                    {
                        string pw = CryptUtil.Encrypt(pUser.Password);
                        if (pw != user.Password)
                        {
                            user.Password = pw;
                        }
                    }

                    user.LoginAccount = pUser.LoginAccount;

                    if (newList != null)
                    {
                        foreach (var a in newList)
                        {
                            UserRole rp = new UserRole();
                            rp.RoleID = a.ID;
                            rp.ID = Guid.NewGuid();
                            rp.UserMTR = user;
                            rp.LastUpdateDatetime = DateTime.Now;
                            user.UserRole.Add(rp);
                        }
                    }

                    if (delList != null)
                    {
                        foreach (var a in delList)
                        {
                            var r = (from s in UserStruct.UserRole
                                     where s.RO_RoleMTR.ID == a.ID && s.UserID == user.ID
                                     select s).First();
                            UserStruct.UserRole.DeleteOnSubmit(r);
                        }
                    }

                    if (userDepartmentList != null)
                    {
                        UserStruct.UserDepartmentRelation.DeleteAllOnSubmit<UserDepartmentRelation>(user.UserDepartmentRelation);
                        foreach (var model in userDepartmentList)
                        {
                            model.LastUpdateTime = System.DateTime.Now;
                            model.ID = Guid.NewGuid();
                            user.UserDepartmentRelation.Add(model);
                        }
                    }



                    UserStruct.SubmitChanges();
                    objReturn.Code = 1;
                }
            }
            catch (Exception ex)
            {
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

        public ReturnMessage PauseUser(UserMTR pUser)
        {
            ReturnMessage objReturn = new ReturnMessage();
            return objReturn;
        }

        public ReturnMessage DeleteUser(UserMTR user)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                UserMTR u = UserStruct.UserMTR.Single(p => p.ID == user.ID);

                UserStruct.UserMTR.DeleteOnSubmit(u);
                UserStruct.SubmitChanges();
                objReturn.Code = 1;
            }
            catch (Exception ex)
            {
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

        public List<UserMTR> GetAllUsers()
        {
            
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            //UserStruct.DeferredLoadingEnabled = false;
            List<UserMTR> ltgUser = new List<UserMTR>();
            ltgUser = (from s in UserStruct.UserMTR
                       where s.LoginAccount != "admin"
                       orderby s.Effectiveness descending, s.LoginAccount
                       select s).ToList();
            return ltgUser;
        }

        public List<UserRole> GetUserRole()
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            List<UserRole> ltgUser = new List<UserRole>();

            var a = (from u in UserStruct.UserMTR
                     join s in UserStruct.UserRole
                     on u.ID equals s.UserID
                     join r in UserStruct.UserRole
                     on s.RoleID equals r.ID
                     where u.LoginAccount != "admin"
                     select new
                     {
                         UserID = u.ID,
                         UserName = u.UserName,
                         LoginAccount = u.LoginAccount,
                         RoleName = r.RO_RoleMTR.RoleName,
                         RoleID = s.RoleID,
                         LastUpdateDatetime = u.LastUpdateDatetime
                     }).ToList();

            for (int i = 0; i < a.Count; i++)
            {
                UserRole objOne = new UserRole();
                objOne.RO_RoleMTR.RoleName = a[i].RoleName;
                objOne.RoleID = (Guid)a[i].RoleID;
                objOne.UserID = (Guid)a[i].UserID;
                objOne.UserMTR.LoginAccount = a[i].LoginAccount;
                objOne.UserMTR.UserName = a[i].UserName;
                objOne.LastUpdateDatetime = (DateTime)a[i].LastUpdateDatetime;
                ltgUser.Add(objOne);
            }
            return ltgUser;
        }

        public List<RO_User> GetUsersByRole(Guid pRoleID)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            List<RO_User> ltgUser = new List<RO_User>();

            var a = (from u in UserStruct.UserMTR
                     join s in UserStruct.UserRole
                     on u.ID equals s.UserID
                     join r in UserStruct.RO_RoleMTR
                     on s.RoleID equals r.ID
                     where r.ID == pRoleID
                     orderby u.LastUpdateDatetime descending
                     select new
                     {
                         UserID = u.ID,
                         u.UserName,
                         u.LoginAccount,
                         r.RoleName,
                         u.LastUpdateDatetime,
                         u.Password,
                         u.Effectiveness,
                         RoleID = r.ID,
                         u.UserRole
                     }).ToList();

            for (int i = 0; i < a.Count; i++)
            {
                RO_User objOne = new RO_User();
                objOne.UserID = a[i].UserID;
                objOne.LoginAccount = a[i].LoginAccount;
                objOne.RoleGroup = a[i].RoleName;
                objOne.UserName = a[i].UserName;
                objOne.Effectiveness = a[i].Effectiveness.Value;
                objOne.Password = a[i].Password;
                objOne.UpdateDate = (DateTime)a[i].LastUpdateDatetime;
                ltgUser.Add(objOne);
            }

            for (int i = 0; i < ltgUser.Count; i++)
            {
                UserMTR u = (from s in UserStruct.UserMTR
                             where s.ID == ltgUser[i].UserID
                             select s).First();
                string strRole = "";
                for (int j = 0; j < u.UserRole.Count; j++)
                {
                    strRole = strRole + "，" + u.UserRole[j].RO_RoleMTR.RoleName;
                    UserRole objOne = new UserRole();
                    objOne.UserID = u.ID;
                    objOne.RoleID = u.UserRole[j].RoleID;
                    ltgUser[i].UserRole.Add(objOne);
                    ltgUser[i].UserRole[ltgUser[i].UserRole.Count - 1].RO_RoleMTR = u.UserRole[j].RO_RoleMTR;
                }
                ltgUser[i].RoleGroup = strRole.Remove(0, 1);
            }

            return ltgUser;

            //for (int i = 0; i < ltgUser.Count; i++)
            //{
            //    UserMTR u = (from s in UserStruct.UserMTR
            //                 where s.ID == ltgUser[i].UserID
            //                 select s).First();
            //    string strRole = "";
            //    for (int j = 0; j < u.UserRole.Count; j++)
            //    {
            //        strRole = strRole + "，" + u.UserRole[j].RO_RoleMTR.RoleName;
            //    }
            //    ltgUser[i].RO_RoleMTR.RoleName = strRole.Remove(0, 1);
            //    //ltgUser[i].UserRoleList = u.UserRoleList;
            //}
            //return ltgUser;
        }

        public List<UserMTR> GetUsersByCondition(string name)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            List<UserMTR> ltgUser = new List<UserMTR>();
            ltgUser = (from s in UserStruct.UserMTR
                       where s.UserName.Contains(name)
                       select s).ToList();
            return ltgUser;
        }

        public bool ExistLoginAccount(UserMTR user)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            bool blnExist = false;
            int count = (from s in UserStruct.UserMTR
                         where s.LoginAccount == user.LoginAccount
                         && s.ID != user.ID
                         select s).Count();
            if (count > 0)
            {
                blnExist = true;
            }
            return blnExist;
        }

        public ReturnMessage ModifyPassword(UserMTR pUser, string newPwd)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                UserMTR user = (from s in UserStruct.UserMTR
                                where s.ID == pUser.ID
                                select s).First();
                //if (user.Password == FormsAuthentication.HashPasswordForStoringInConfigFile(pUser.Password, "MD5"))
                //{
                //    if (user.Password != FormsAuthentication.HashPasswordForStoringInConfigFile(newPwd, "MD5"))
                //    {
                //        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPwd, "MD5");
                //        UserStruct.SubmitChanges();
                //        objReturn.Code = 1;
                //    }
                //    else
                //    {
                //        objReturn.Code = 203;
                //        objReturn.Message = "新旧密码不能相同。";
                //    }
                //}
                //else
                //{
                //    objReturn.Code = 201;
                //    objReturn.Message = "旧密码不正确。";
                //}
                if (user.Password == CryptUtil.Encrypt(pUser.Password))
                {
                    if (user.Password != CryptUtil.Encrypt(newPwd))
                    {
                        user.Password = CryptUtil.Encrypt(newPwd);
                        UserStruct.SubmitChanges();
                        objReturn.Code = 1;
                    }
                    else
                    {
                        objReturn.Code = 203;
                        objReturn.Message = "新旧密码不能相同。";
                    }
                }
                else
                {
                    objReturn.Code = 201;
                    objReturn.Message = "旧密码不正确。";
                }
            }
            catch (Exception ex)
            {
                objReturn.Code = 202;
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

        public List<UserMTR> GetAllLoginAccount()
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            var a = (from s in UserStruct.UserMTR
                     select new { s.ID, s.LoginAccount }).ToList();
            List<UserMTR> AccountList = new List<UserMTR>();
            for (int i = 0; i < a.Count; i++)
            {
                UserMTR u = new UserMTR();
                u.ID = a[i].ID;
                u.LoginAccount = a[i].LoginAccount;
                AccountList.Add(u);
            }
            return AccountList;
        }

        public UserMTR GetUser()
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            var u = (from s in UserStruct.UserMTR
                     where s.LoginAccount == "admin"
                     select s).FirstOrDefault();
            return u;
        }

        public ReturnMessage CheckUser(string userName, string pwd, UserMTR user)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            pwd = CryptUtil.Encrypt(pwd);
            //pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            var u = from s in UserStruct.UserMTR
                    where s.LoginAccount == userName
                    && s.Password == pwd
                    && s.LoginAccount != user.LoginAccount
                    select s;
            if (u.Count() > 0)
            {
                return new ReturnMessage() { Code = 1 };
            }
            else
            {
                return new ReturnMessage() { Code = 200 };
            }
        }

        public List<UserLocationRelation> GetUserLocationByUserID(string userID)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            List<UserLocationRelation> ltgUserLocation = new List<UserLocationRelation>();
            ltgUserLocation = (from s in UserStruct.UserLocationRelation
                            where s.UserID.Equals(userID)
                       select s).ToList();
            return ltgUserLocation;
        }


        public List<UserDepartmentRelation> GetUserDepartmentByUserID(string userID)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            List<UserDepartmentRelation> ltgUserLocation = new List<UserDepartmentRelation>();
            ltgUserLocation = (from s in UserStruct.UserDepartmentRelation
                               where s.UserID.Equals(userID)
                               select s).ToList();
            return ltgUserLocation;
        }
    }
}
