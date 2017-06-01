using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using APP_Server.Models;
using Newtonsoft.Json;
using DL.Model;
using DL;
using System.Xml.Serialization;
using System.IO;
using DL.DataLogic;

namespace APP_Server
{
    /// <summary>
    /// APP_WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class APP_WebService : System.Web.Services.WebService
    {
        // *** Start Common Method ***

        #region Android 2 WebService ( 利用反射运行APP_WebService.asmx的方法, 并且捕获异常和业务逻辑错误 )

        [WebMethod(Description = "利用反射运行APP_WebService.asmx的方法, 并且捕获异常和业务逻辑错误")]
        public string ExecuteWebServiceMethodV3(string methodName, List<string> jsonArgs)
        {
            SOAPResult soapResult = new SOAPResult();
            string result = string.Empty;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            try
            {
                Type t = typeof(APP_WebService);
                var methods = t.GetMethods().ToList();
                var matchMethod = methods.FirstOrDefault(i => i.Name.Equals(methodName));
                if (matchMethod != null)
                {
                    var methodParams = matchMethod.GetParameters();
                    var invokeArgs = new object[methodParams.Length];
                    if (methodParams != null && methodParams.Length > 0)
                    {
                        if (jsonArgs != null && jsonArgs.Count == methodParams.Length)
                        {
                            for (int i = 0; i < methodParams.Length; i++)
                            {
                                if (methodParams[i].ParameterType.FullName.Equals("System.String"))
                                {
                                    invokeArgs[i] = jsonArgs[i];
                                }
                                else
                                {
                                    invokeArgs[i] = JsonConvert.DeserializeObject(jsonArgs[i], methodParams[i].ParameterType);
                                }
                            }
                        }
                        else
                        {
                            string errorMsg = string.Format("ERROR : {0} 所需参数数目 {1}; 传入参数数目 {2}",
                                methodName,
                                methodParams.Length,
                                jsonArgs != null ? jsonArgs.Count : 0);

                            throw new Exception(errorMsg);
                        }
                    }

                    object invokeResult = matchMethod.Invoke(this, invokeArgs);
                    result = JsonConvert.SerializeObject(invokeResult);
                    soapResult.IsComplete = true;
                    soapResult.ExceptionInfo = string.Empty;
                    soapResult.IsSuccess = true;
                    soapResult.BusinessExceptionInfo = string.Empty;

                    if (string.IsNullOrEmpty(result) == false && result.Equals("null") == false)
                    {
                        soapResult.ReturnObjectJson = result;
                    }
                }
                else
                {
                    throw new Exception(string.Format("APP_WebService找不到方法 {0}", methodName));
                }
            }
            catch (Exception ex) // 抛错必定会是 "调用的目标发生了异常"
            {
                if (ex.InnerException != null && ex.InnerException is BusinessException)
                {
                    soapResult.IsComplete = true;
                    Util.ExceptionUtil.GetExceptionFullInfoForInvoke(ex, sb);
                    soapResult.ExceptionInfo = sb.ToString();
                    soapResult.IsSuccess = false;
                    soapResult.BusinessExceptionInfo = ex.InnerException.Message;
                }
                else
                {
                    soapResult.IsComplete = false;
                    Util.ExceptionUtil.GetExceptionFullInfoForInvoke(ex, sb);
                    soapResult.ExceptionInfo = sb.ToString();
                    soapResult.IsSuccess = false;
                    soapResult.BusinessExceptionInfo = string.Empty;
                }
            }

            return JsonConvert.SerializeObject(soapResult);
        }

        // TODO 未能测试此版本由于 Android 中创建SOAP数据无法处理 HashMap<String, String> 的转换
        [WebMethod(Description = "返回 Dictionary<string, string>; Item1 : SOAPResult; Item2 : MethodResult")]
        public List<string> ExecuteWebServiceMethodV4(string methodName, System.Collections.DictionaryEntry[] jsonArgs)
        {
            List<string> finalResult = new List<string>();
            SOAPResult soapResult = new SOAPResult();
            string result = string.Empty;
            try
            {
                Type t = typeof(APP_WebService);
                var methods = t.GetMethods().ToList();
                var matchMethod = methods.FirstOrDefault(i => i.Name.Equals(methodName));
                if (matchMethod != null)
                {
                    var methodParams = matchMethod.GetParameters();
                    var invokeArgs = new object[methodParams.Length];
                    if (methodParams != null && methodParams.Length > 0)
                    {
                        if (jsonArgs != null && jsonArgs.Length == methodParams.Length)
                        {
                            string paramValue = string.Empty;
                            for (int i = 0; i < methodParams.Length; i++)
                            {
                                System.Collections.DictionaryEntry matchParam = jsonArgs.FirstOrDefault(p => p.Key.ToString().Equals(methodParams[i].Name));
                                if (matchParam.Value != null)
                                {
                                    invokeArgs[i] = JsonConvert.DeserializeObject(paramValue, methodParams[i].ParameterType);
                                }
                                else
                                {
                                    string errorMsg = string.Format("ExecuteWebServiceMethod ERROR : {0} 所需参数 {1}",
                                        methodName,
                                        methodParams[i].Name);

                                    throw new Exception(errorMsg);
                                }
                            }
                        }
                        else
                        {
                            string errorMsg = string.Format("ExecuteWebServiceMethod ERROR : {0} 所需参数数目 {1}; 传入参数数目 {2}",
                                methodName,
                                methodParams.Length,
                                jsonArgs.Length);

                            throw new Exception(errorMsg);
                        }
                    }

                    object invokeResult = matchMethod.Invoke(this, invokeArgs);
                    result = JsonConvert.SerializeObject(invokeResult);
                }
                else
                {
                    throw new Exception(string.Format("ExecuteWebServiceMethod ERROR : APP_WebService找不到方法 {0}", methodName));
                }
            }
            catch (BusinessException e)
            {
                soapResult.IsComplete = true;
                soapResult.ExceptionInfo = string.Empty;
                soapResult.IsComplete = false;
                soapResult.BusinessExceptionInfo = e.Message;
            }
            catch (Exception ex)
            {
                soapResult.IsComplete = false;
                soapResult.ExceptionInfo = ex.Message;
                soapResult.IsComplete = false;
                soapResult.BusinessExceptionInfo = string.Empty;
            }

            finalResult.Add(JsonConvert.SerializeObject(soapResult));
            finalResult.Add(result);

            return finalResult;
        }

        #endregion

        #region 连接服务器测试

        [WebMethod(Description = "测试 - 连接服务器测试(连接服务器,连接数据库测试)")]
        public bool Test()
        {
            bool r = true;

            DateTime d1 = DateTime.Now;
            DateTime d2 = new TestBll().GetDBTime();

            double d = new TimeSpan(d1.Ticks).TotalMinutes - new TimeSpan(d2.Ticks).TotalMinutes;

            string tempErrorMsg = "服务器时间与数据库时间相差 {0} 分钟";
            if (Math.Abs(d) >= 60)
            {
                throw new BusinessException(string.Format(tempErrorMsg, Math.Abs(d)));
            }

            return r;
        }

        #endregion

        #region 获取程序最新版本

        [WebMethod(Description = "获取更新信息")]
        public UpdateInfo GetLastestVersion(string selectedType)
        {
            UpdateInfo info = new UpdateInfo();
            string pathHeader = string.Empty;
            string updateXmlFile = string.Empty;
            updateXmlFile = "Update.xml";
            pathHeader = "/Update/";
            switch (selectedType.ToUpper())
            {
                case "APK":
                    pathHeader = "/Update/APK/";
                    break;
                case "DATABASE":
                    pathHeader = "/Update/DATABASE/";
                    break;
                default:
                    break;

            }
            XmlSerializer ser = new XmlSerializer(typeof(UpdateInfo));

            string path = this.Server.MapPath("~/" + pathHeader);
            if (!string.IsNullOrEmpty(pathHeader))
            {
                using (Stream stream = new FileStream(
                    path + updateXmlFile,
                    FileMode.Open, FileAccess.Read))
                {
                    info = (UpdateInfo)ser.Deserialize(stream);
                    info.url = "http://" + this.Context.Request.Url.Authority + this.Context.Request.ApplicationPath + pathHeader + info.url;
                    info.Result = true;
                }
            }

            return info;
        }

        [WebMethod(Description = "获取更新信息(测试)")]
        public UpdateInfo GetLastestVersionTEST()
        {
            string selectedType = "APK";
            UpdateInfo info = new UpdateInfo();
            string pathHeader = string.Empty;
            string updateXmlFile = string.Empty;
            updateXmlFile = "Update.xml";
            pathHeader = "/Update/";
            switch (selectedType.ToUpper())
            {
                case "APK":
                    pathHeader = "/Update/APK/";
                    break;
                case "DATABASE":
                    pathHeader = "/Update/DATABASE/";
                    break;
                default:
                    break;

            }
            XmlSerializer ser = new XmlSerializer(typeof(UpdateInfo));

            string path = this.Server.MapPath("~/" + pathHeader);
            if (!string.IsNullOrEmpty(pathHeader))
            {
                using (Stream stream = new FileStream(
                    path + updateXmlFile,
                    FileMode.Open, FileAccess.Read))
                {
                    info = (UpdateInfo)ser.Deserialize(stream);
                    info.url = "http://" + this.Context.Request.Url.Authority + this.Context.Request.ApplicationPath + pathHeader + info.url;
                    info.Result = true;
                }
            }

            return info;
        }

        #endregion

        #region 登录

        [WebMethod(Description = "登录 - APP用户登录")]
        public User LoginCheckVersion(User tmp, string versionCode)
        {
            var updateinfo = GetLastestVersion("APK");
            if (int.Parse(updateinfo.version) > int.Parse(versionCode))
            {
                throw new BusinessException(string.Format("请更新程序到 Version{0}", updateinfo.version));
            }

            return Login(tmp);
        }

        [WebMethod(Description = "登录 - APP用户登录")]
        public User Login(User tmp)
        {
            Security.Model.User tmpR = new Security.DataLogic.UserBll().Login(tmp.Account, tmp.Password, tmp.CompanyCode);

            if (tmpR != null && string.IsNullOrEmpty(tmpR.ID) == false)
            {
                var tempPDAPermissionList = tmpR.PermissionList
                    .Where(i => i.ModuleID.Equals("DE335673-654E-4B74-A449-90E97E1E2A12", StringComparison.InvariantCultureIgnoreCase))
                    .ToList();

                List<string> ignorePermissionList = new List<string>();
                ignorePermissionList.Add("B0E82D4E-C1D2-4F20-8996-45A82152DEE4"); // B0E82D4E-C1D2-4F20-8996-45A82152DEE4 发料批次违规放行权限

                var PDAPermissionList = // 过滤掉忽略列表后
                tempPDAPermissionList.Where(i => ignorePermissionList.Contains(i.PermissionID.ToUpper()) == false)
                    .Select(j => new Permission()
                    {
                        ID = j.PermissionID,
                        Code = j.Code,
                        Name = j.Name,
                        ClassName = j.ClassName,
                        Seq = j.Seq,

                        ModuleID = j.ModuleID,
                        ModuleName = j.ModuleName,
                    })
                    .ToList();

                return new User()
                {
                    ID = tmpR.ID,
                    Account = tmpR.Account,
                    UserName = tmpR.UserName,
                    CompanyCode = tmpR.CompanyCode,
                    PermissionList = PDAPermissionList
                };
            }
            else
            {
                throw new BusinessException("账号密码错误。");
            }
        }

        [WebMethod(Description = "登录 - APP用户登录(测试)")]
        public User LoginTEST()
        {
            User tmp = new User()
            {
                Account = "a2222",
                Password = "2",
                CompanyCode = "BP02"
            };

            return Login(tmp);
        }

        #endregion

        #region 修改密码

        [WebMethod(Description = "修改密码 - APP用户修改密码")]
        public void ChangePassword(User tmp, string newPassword)
        {
            this.Login(tmp); // 校验账号密码, 有问题会报错
            new Security.DataLogic.UserBll().ChangePassword(tmp.Account, tmp.Password, newPassword, tmp.CompanyCode);
        }

        [WebMethod(Description = "修改密码 - APP用户修改密码(测试)")]
        public void ChangePasswordTEST(string oldPassword, string newPassword)
        {
            User tmp = new User()
            {
                Account = "a2222",
                Password = oldPassword,
                CompanyCode = "BP02"
            };

            ChangePassword(tmp, newPassword);
        }

        #endregion

        // *** End Common Method ***

        #region Read/Write XML & PDA打印底图

        /// <summary>
        /// 标签模版路径
        /// </summary>
        public string SaveTmpPath
        {
            get
            {
                return System.IO.Path.Combine(Server.MapPath("Config"), "save.tmp");
            }
        }

        [WebMethod(Description = "Read/Write XML & PDA打印底图")]
        public BarcodeDeclareXML GetBarcodeDeclareXML(int alignLeft, int alignTop)
        {
            string directory = Server.MapPath("Config");
            var l = System.IO.Directory.GetFiles(directory, "*.cfg");

            BarcodeDeclareXML r = new BarcodeDeclareXML();

            using (System.IO.FileStream fs = new System.IO.FileStream(l.FirstOrDefault(i => i.Contains("Read")), System.IO.FileMode.Open))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    r.ReadObj = sr.ReadToEnd();
                }
            }
            using (System.IO.FileStream fs = new System.IO.FileStream(l.FirstOrDefault(i => i.Contains("Write")), System.IO.FileMode.Open))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
                {
                    r.WriteObj = sr.ReadToEnd();
                }
            }

            // r.PrintZPLTemplate = new DeliveryOrderBll().GetPrintZPLTemplate(this.SaveTmpPath, alignLeft, alignTop);

            return r;
        }

        #endregion

    }
}

