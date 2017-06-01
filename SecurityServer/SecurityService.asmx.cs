using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Configuration;
using System.Xml.Serialization;

using EML.Util;
using Security.Model;
using Security.DL.DataSources;
using Security.DataLogic;

//using ENPOT.Security.Model;
//using ENPOT.Security.DataLogic;
//using ENPOT.Security.DataSources;

namespace SecurityServer
{
    /// <summary>
    /// SecurityService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class SecurityService : System.Web.Services.WebService
    {
        [WebMethod(Description = "获取服务器时间")]
        public DateTime GetServiceDateTime()
        {
            return DateTime.Now;
        }

        [WebMethod(Description = "获取更新信息")]
        public UpdateInfo GetUpdateInfo(EnumApplication type)
        {
            UpdateInfo info = new UpdateInfo();

            string pathHeader = string.Empty;
            string updateXmlFile = string.Empty;

            updateXmlFile = "Update.xml";


            pathHeader = "/Update/";

            switch (type)
            {
                case EnumApplication.Client:
                    pathHeader = "/Update/Client/";
                    break;
                case EnumApplication.PDA:
                    pathHeader = "/Update/PDA/";
                    break;
                default:
                    break;

            }

            XmlSerializer ser = new XmlSerializer(typeof(UpdateInfo));
            try
            {
                string path = this.Server.MapPath("~/" + pathHeader);
                if (!string.IsNullOrEmpty(pathHeader))
                {
                    using (Stream stream = new FileStream(
                        path + updateXmlFile,
                        FileMode.Open, FileAccess.Read))
                    {
                        info = (UpdateInfo)ser.Deserialize(stream);

                        foreach (UpdateInfo.Item item in info.Items)
                        {
                            item.FileLen = new FileInfo(path + item.Url).Length;
                            item.Url = this.Context.Request.ApplicationPath + pathHeader + item.Url;
                        }
                        info.Result = true;
                    }
                }
                else
                {
                    info.Result = false;
                }

            }
            catch (Exception ex)
            {
                info.Result = false;
                info.Message = ex.Message;
            }

            return info;
        }

        [WebMethod(Description = "初始化数据库连接字符串配置")]
        public string ConfigSettingInit(string SecConnHost, string SecConnCatalog, string SecConnUser, string SecConnPassword
            , string EMLConnHost, string EMLConnCatalog, string EMLConnUser, string EMLConnPassword
            )
        {
            string errorMsg = string.Empty;
            try
            {
                ConfigSetting sec = new ConfigSetting();
                sec.A = CryptUtil.Encrypt(SecConnHost);
                sec.B = CryptUtil.Encrypt(SecConnCatalog);
                sec.C = CryptUtil.Encrypt(SecConnUser);
                sec.D = CryptUtil.Encrypt(SecConnPassword);

                ConfigSetting eml = new ConfigSetting();
                eml.A = CryptUtil.Encrypt(EMLConnHost);
                eml.B = CryptUtil.Encrypt(EMLConnCatalog);
                eml.C = CryptUtil.Encrypt(EMLConnUser);
                eml.D = CryptUtil.Encrypt(EMLConnPassword);

                ConfigSetting.Save(sec, ServiceStaticInfo.SecSettingPath);
                ConfigSetting.Save(eml, ServiceStaticInfo.EMLSettingPath);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                return "设置成功";
            }
            else
            {
                return "设置失败。\n" + errorMsg;
            }
        }

        #region 更新ProvideLocation信息

        [WebMethod(Description = "由ProvideLocation信息 生成 ProvideLocation的信息")]
        public bool InsertProvideLocation(ProvideLocation location)
        {
            ProvideLocationBll plBll = new ProvideLocationBll();
            return plBll.InsertProvideLocation(location);
        }

        [WebMethod(Description = "拿取Location，并且含有ProvideLocationID的信息")]
        public List<Location> GetLocationProvideLocationID()
        {
            LocationBll lBll = new LocationBll();
            return lBll.GetLocationProvideLocationID();
        }

        [WebMethod(Description = "新增一个库位的库容")]
        public bool InsertLocationCapacity(string companyCode, string LocationID, decimal capacity, string UM)
        {
            LocationBll handler = new LocationBll();
            return handler.InsertLocationCapacity(companyCode, LocationID, capacity, UM);
        }


        [WebMethod(Description = "修改库位")]
        public bool UpdateLocatorAndCapacity(string locationID, string name, decimal capacity, string UM)
        {
            LocationBll handler = new LocationBll();
            return handler.UpdateLocatorAndCapacity(locationID, name, capacity, UM);
        }

        [WebMethod(Description = "删除库位")]
        public bool DeleteLocatorAndCapacity(string locationID)
        {
            LocationBll handler = new LocationBll();
            return handler.DeleteLocatorAndCapacity(locationID);
        }

        /// <summary>
        /// 检查库位是否有库存
        /// </summary>
        /// <param name="locationCode">库位ID</param>
        /// <param name="locationTypeID">库位类型</param>
        /// <param name="companyCode"></param>
        [WebMethod(Description = "检查库位是否有库存")]
        public bool CheckrContainerRelationForLocator(string locationCode, int locationTypeID, string companyCode)
        {
            LocationBll handler = new LocationBll();
            return handler.CheckrContainerRelationForLocator(locationCode, locationTypeID, companyCode);
        }

        #endregion

        [WebMethod]
        public List<Location> GetLocationProvideLocationIDByTypeControlKey(string key)
        {
            LocationBll bll = new LocationBll();
            return bll.GetLocationProvideLocationIDByTypeControlKey(key);
        }

        #region 主数据

        [WebMethod]
        public List<LocationType> GetLocationTypeByTypeControlKey(string key)
        {
            LocationBll bll = new LocationBll();
            return bll.GetLocationTypeByTypeControlKey(key);
        }

        [WebMethod]
        public List<Location> GetLocationByTypeControlKey(string key)
        {
            LocationBll bll = new LocationBll();
            return bll.GetLocationByTypeControlKey(key);
        }

        [WebMethod]
        public List<Location> GetLocationByTypeControlKeyParentID(string key, string parentID)
        {
            LocationBll bll = new LocationBll();
            return bll.GetLocationByTypeControlKeyParentID(key, parentID);
        }



        [WebMethod]
        public List<Location> GetLocationTreeList()
        {
            LocationBll handler = new LocationBll();
            return handler.GetLocationTreeList();
        }

        [WebMethod]
        public List<LocationType> GetLocationTypeList()
        {
            LocationBll handler = new LocationBll();
            return handler.GetLocationTypeList();
        }


        [WebMethod]
        public bool InsertLocation(Location model)
        {
            bool result = false;
            LocationBll handler = new LocationBll();

            result = handler.InsertLocation(model);

            if (result)
            {
                ProvideLocationBll plBll = new ProvideLocationBll();
                var p = plBll.GetProvideLocationByCode(model.CompanyCode, model.Code, model.LocationTypeID);
                if (p == null || (p != null && string.IsNullOrEmpty(p.ID)))
                {
                    var l = handler.GetLocationByCode(model.CompanyCode, model.Code, model.LocationTypeID);
                    plBll.InsertProvideLocation(l);
                }
            }

            return result;
        }

        [WebMethod(Description = "每组树内部,同类型或同等级不能重复")]
        public bool CheckLocationIsExist(string parentId, int locationTypeID, string code)
        {
            LocationBll handler = new LocationBll();
            return handler.CheckLocationIsExist(parentId, locationTypeID, code);
        }

        [WebMethod]
        public bool DeleteLocation(string id, string locationCode)
        {
            LocationBll handler = new LocationBll();
            return handler.DeleteLocation(id, locationCode);
        }

        [WebMethod]
        public bool UpdateLocation(Location model)
        {
            LocationBll handler = new LocationBll();
            Location isExist = handler.GetLocationByName(model.CompanyCode, model.Name, model.LocationTypeID);
            if (isExist == null)
            {
                return handler.UpdateLocation(model);
            }
            else
            {
                return false;
            }
        }

        [WebMethod(Description = "在Location中存在Name与传入字段相同")]
        public Location GetLocationByName(string companyCode, string locationName, int locationTypeID)
        {
            LocationBll handler = new LocationBll();
            return handler.GetLocationByName(companyCode, locationName, locationTypeID);
        }


        [WebMethod]
        public List<Department> GetDepartmentTreeList()
        {
            DepartmentBll handler = new DepartmentBll();
            return handler.GetDepartmentTreeList();
        }

        #endregion



        /// <summary>
        /// 获取地点路径
        /// </summary>
        [WebMethod]
        public ProvideLocation GetProvideLocationByCode(string companyCode, string locationCode, int locationTypeID)
        {
            ProvideLocationBll bll = new ProvideLocationBll();
            return bll.GetProvideLocationByCode(companyCode, locationCode, locationTypeID);
        }


        [WebMethod(Description = "用户登陆，NULL：所在公司的账号不存在； User：ID是Empty，Account有Value，代表有账号，但密码错误；")]
        public User Login(string account, string password, string device)
        {
            UserBll bll = new UserBll();
            return bll.Login(account, password, device);
        }

        [WebMethod(Description = "更改密码，返回的User和Login方法一样")]
        public User ChangePassword(string account, string oldPassword, string newPassword, string companyCode)
        {
            UserBll bll = new UserBll();
            return bll.ChangePassword(account, oldPassword, newPassword, companyCode);
        }

        [WebMethod(Description = "根据用户名称，获取用户信息")]
        public List<User> GetUserByUserName(string userName)
        {
            UserBll bll = new UserBll();
            return bll.GetUserByUserName(userName);
        }

        [WebMethod]
        public string Decrypt(string str)
        {
            return CryptUtil.Decrypt(str);
        }

        [WebMethod]
        public string Encrypt(string str)
        {
            return CryptUtil.Encrypt(str);
        }

        private string CreateConnStr()
        {
            return SQLHelper.SecurityConnString;
        }

        #region SecurityMgrSys

        //[WebMethod]
        //public List<Location> GetLocationByTypeControlKey(string key)
        //{
        //    LocationBll bll = new LocationBll();
        //    return bll.GetLocationByTypeControlKey(key);
        //}


        //[WebMethod]
        //public List<Location> GetLocationTreeList()
        //{
        //    LocationBll handler = new LocationBll();
        //    return handler.GetLocationTreeList();
        //}


        [WebMethod]
        public bool CheckServiceConnection()
        {
            return true;
        }

        #region RolePermission
        //新增角色
        [WebMethod]
        public ReturnMessage AddRole(RoleMTR role, List<RolePromission> newList, Guid userID)
        {
            var conn = CreateConnStr();
            RoleHandler handler = new RoleHandler(conn);
            ReturnMessage objReturn = handler.AddRole(role, newList, userID);
            return objReturn;
        }

        [WebMethod]
        public ReturnMessage DeleteRole(RoleMTR role)
        {
            var conn = CreateConnStr();
            RoleHandler handler = new RoleHandler(conn);
            ReturnMessage objReturn = new ReturnMessage();
            objReturn = handler.DeleteRole(role);
            return objReturn;
        }

        //返回所以角色
        [WebMethod]
        public List<RoleMTR> GetAllRoles()
        {
            var conn = CreateConnStr();
            RoleHandler handler = new RoleHandler(conn);
            List<RoleMTR> ltgRole = new List<RoleMTR>();
            ltgRole = handler.GetAllRoles();
            return ltgRole;
        }

        //按角色名称查找
        [WebMethod]
        public List<RoleMTR> GetRolesByCondition(string rolename)
        {
            var conn = CreateConnStr();
            RoleHandler handler = new RoleHandler(conn);
            List<RoleMTR> ltgRole = new List<RoleMTR>();
            ltgRole = handler.GetRolesByCondition(rolename);
            return ltgRole;
        }

        [WebMethod]
        public List<RO_SysPromissionMTR> GetPermissionByUser(UserMTR u)
        {
            var conn = CreateConnStr();
            RoleHandler handler = new RoleHandler(conn);
            return handler.GetPermissionByUser(u);
        }

        [WebMethod]
        public ReturnMessage ModiftyRole(RoleMTR role, List<RolePromission> newList,
            List<RolePromission> delList, Guid userID)
        {
            var conn = CreateConnStr();
            ReturnMessage objReturn = new ReturnMessage();
            RoleHandler handler = new RoleHandler(conn);
            objReturn = handler.ModiftyRole(role, newList, delList, userID);
            return objReturn;
        }

        [WebMethod]
        public List<SysModuleMTR> GetAllModules()
        {
            var conn = CreateConnStr();
            List<SysModuleMTR> listModule = new List<SysModuleMTR>();
            RoleHandler handler = new RoleHandler(conn);
            listModule = handler.GetAllModules();
            return listModule;
        }

        [WebMethod]
        public List<PermissionModel> GetAllModuleList()
        {
            var conn = CreateConnStr();
            List<PermissionModel> listModule = new List<PermissionModel>();
            RoleHandler handler = new RoleHandler(conn);
            listModule = handler.GetAllModuleList();
            return listModule;
        }
        #endregion

        #region User

        [WebMethod]
        public UserMTR LoginSecurityMgrSys(string LoginAccount, string Password)
        {
            var conn = CreateConnStr();
            UserHandler userHandler = new UserHandler(conn);
            UserMTR objReturn = userHandler.Login(LoginAccount, Password);
            return objReturn;
        }

        [WebMethod]
        public UserMTR GetUser()
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            return handler.GetUser();
        }

        [WebMethod]
        public ReturnMessage AddUser(UserMTR user, List<UserRole> newList, List<UserLocationRelation> userLocationList)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.AddUser(user, newList, userLocationList);
            return objReturn;
        }



        [WebMethod]
        public ReturnMessage ModifyUser(UserMTR user, List<UserRole> newList, List<UserRole> delList, List<UserLocationRelation> userLocation)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.ModifyUser(user, newList, delList, userLocation);
            return objReturn;
        }

        [WebMethod]
        public ReturnMessage ModifyPassword(UserMTR user, string newPwd)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.ModifyPassword(user, newPwd);
            return objReturn;
        }

        [WebMethod]
        public ReturnMessage DeleteUser(UserMTR user)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.DeleteUser(user);
            return objReturn;
        }

        [WebMethod]
        public List<UserMTR> GetAllUsers()
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<UserMTR> ltgUser = handler.GetAllUsers();
            return ltgUser;
        }

        [WebMethod]
        public List<UserMTR> GetUsersByCondition(string name)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<UserMTR> ltgUser = handler.GetUsersByCondition(name);
            return ltgUser;
        }

        [WebMethod]
        public List<RO_User> GetUsersByRoleID(Guid pRoleID)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<RO_User> ltgUser = new List<RO_User>();
            ltgUser = handler.GetUsersByRole(pRoleID);
            return ltgUser;
        }

        [WebMethod]
        public List<UserRole> GetUserRole()
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<UserRole> ltgUder = handler.GetUserRole();
            return ltgUder;
        }

        [WebMethod]
        public List<UserLocationRelation> GetUserLocationByUserID(string userID)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<UserLocationRelation> ltgUder = handler.GetUserLocationByUserID(userID);
            return ltgUder;
        }

        [WebMethod]
        public List<UserDepartmentRelation> GetUserDepartmentByUserID(string userID)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            List<UserDepartmentRelation> ltgUder = handler.GetUserDepartmentByUserID(userID);
            return ltgUder;
        }

        [WebMethod]
        public ReturnMessage AddUserSecond(UserMTR user, List<UserRole> newList, List<UserDepartmentRelation> userDepartmentList)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.AddUser(user, newList, userDepartmentList);
            return objReturn;
        }

        [WebMethod]
        public ReturnMessage ModifyUserSecond(UserMTR user, List<UserRole> newList, List<UserRole> delList, List<UserDepartmentRelation> userDepartmentList)
        {
            var conn = CreateConnStr();
            UserHandler handler = new UserHandler(conn);
            ReturnMessage objReturn = handler.ModifyUser(user, newList, delList, userDepartmentList);
            return objReturn;
        }


        #endregion

        #endregion
    }
}
