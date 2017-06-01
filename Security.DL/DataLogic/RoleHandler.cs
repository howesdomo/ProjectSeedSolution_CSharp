using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.Model;
using Security.DL.DataSources;

namespace Security.DataLogic
{
   public class RoleHandler
    {
        string _conn;
        public RoleHandler(string conn)
        {
            _conn = conn;
        }

       /// <summary>
       /// 新增角色
       /// </summary>
       /// <param name="pRole">角色</param>
       /// <param name="newList">权限</param>
       /// <returns></returns>
        public ReturnMessage AddRole(RoleMTR pRole, List<RolePromission> newList,Guid userID)
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                if (ExistRoleName(pRole))
                {
                    objReturn.Code = 201;
                    objReturn.Message = "权限组:" + pRole.RoleName + "已存在。";
                }
                else
                {
                    foreach (var a in newList)
                    {
                        RolePromission rp = new RolePromission();
                        rp.RightID = a.RightID;
                        rp.ID = Guid.NewGuid();
                        rp.RoleMTR = pRole;
                        rp.LastUpdateDatetime = System.DateTime.Now;
                        rp.LastUpdateUserID = userID;
                        RoleStruct.RolePromission.InsertOnSubmit(rp);
                        pRole.RolePromission.Add(rp);
                    }

                    //foreach (Guid moduleID in moduleList)
                    //{
                    //    RoleModule rm = new RoleModule();
                    //    rm.ID = Guid.NewGuid();
                    //    rm.ModuleID = moduleID;
                    //    rm.RoleMTR = pRole;
                    //    rm.Effectiveness =true;
                    //    rm.LastUpdateUserID = userID;
                    //    rm.LastUpdateDatetime = System.DateTime.Now;
                    //    RoleStruct.RoleModule.InsertOnSubmit(rm);
                    //    pRole.RoleModule.Add(rm);
                    //}

                    pRole.IsAdminstrator = false;
                    pRole.LastUpdateDatetime = DateTime.Now;
                    RoleStruct.RoleMTR.InsertOnSubmit(pRole);

                    RoleStruct.SubmitChanges();
                    objReturn.Code = 1;
                }
            }
            catch (Exception ex)
            {
                objReturn.Message = ex.Message;
            }
            return objReturn;
        }

       public ReturnMessage ModiftyRole(RoleMTR role, List<RolePromission> newList,
          List<RolePromission> delList, Guid userID)
       {
           RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
           ReturnMessage objReturn = new ReturnMessage();
           try
           {
               if (ExistRoleName(role))
               {
                   objReturn.Code = 201;
                   objReturn.Message = "权限组:" + role.RoleName + "已存在。";
               }
               else
               {
                   var role1 = (from s in RoleStruct.RoleMTR
                                where s.ID == role.ID
                                select s).First();
                   role1.RoleName = role.RoleName;
                   role1.Effectiveness = role.Effectiveness;
                   role1.LastUpdateDatetime = DateTime.Now;

                   foreach (var a in newList)
                   {
                       RolePromission rp = new RolePromission();
                       rp.RightID = a.RightID;
                       rp.ID = Guid.NewGuid();
                       rp.LastUpdateUserID = userID;
                       rp.LastUpdateDatetime = System.DateTime.Now;
                       rp.RoleMTR = role1;
                       // RoleStruct.RolePermission.InsertOnSubmit(rp);
                       role1.RolePromission.Add(rp);
                   }

                   foreach (var a in delList)
                   {
                       var r = (from s in RoleStruct.RolePromission
                                where s.RightID == a.RightID && s.RoleID == role1.ID
                                select s).First();
                      RoleStruct.RolePromission.DeleteOnSubmit(r);
                       //role1.RolePromission.Remove(r);
                   }

                   var query = (from s in RoleStruct.RoleModule
                            where s.RoleID == role1.ID
                            select s);
                   //foreach (var a in query)
                   //{
                   //    role1.RoleModule.Remove(a);
                   //}
                   RoleStruct.RoleModule.DeleteAllOnSubmit(query);

                   //foreach (var a in newRoleModule)
                   //{
                   //    RoleModule rm = new RoleModule();
                   //    rm.ID = Guid.NewGuid();
                   //    rm.ModuleID = a.ID;
                   //    rm.LastUpdateUserID = userID;
                   //    rm.LastUpdateDatetime = System.DateTime.Now;
                   //    rm.Effectiveness = true;
                   //    rm.RoleMTR = role1;
                   //    role1.RoleModule.Add(rm);
                   //}

                   RoleStruct.SubmitChanges();
                   objReturn.Code = 1;
               }
           }
           catch (Exception ex)
           {
               objReturn.Message = ex.Message;
           }

           return objReturn;
       }

        public ReturnMessage DeleteRole(RoleMTR pRole)
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            ReturnMessage objReturn = new ReturnMessage();
            try
            {
                RoleStruct.RoleMTR.DeleteOnSubmit(pRole);
                RoleStruct.SubmitChanges();
                objReturn.Code = 1;
            }
            catch (Exception ex)
            {
                objReturn.Message = ex.Message;
            }

            return objReturn;
        }

        public List<RoleMTR> GetAllRoles()
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            List<RoleMTR> ltgRole = new List<RoleMTR>();
            ltgRole = (from r in RoleStruct.RoleMTR
                       orderby r.Effectiveness descending
                       where r.IsAdminstrator == false
                       select r).ToList();
            return ltgRole;
        }

        public bool ExistRoleName(RoleMTR role)
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            bool blnExist = false;
            int count = (from s in RoleStruct.RoleMTR
                         where s.RoleName == role.RoleName
                         && s.ID != role.ID
                         select s).Count();
            if (count > 0)
            {
                blnExist = true;
            }
            return blnExist;
        }

        public List<RoleMTR> GetRolesByCondition(string rolename)
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            List<RoleMTR> ltgRole = new List<RoleMTR>();
            var list = (from s in RoleStruct.RoleMTR
                        where s.RoleName.Contains(rolename)
                        select s).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                RoleMTR objOne = new RoleMTR();
                objOne.ID = list[i].ID;
                objOne.IsAdminstrator = list[i].IsAdminstrator;
                objOne.LastUpdateDatetime = list[i].LastUpdateDatetime;
                objOne.LastUpdateUserID = list[i].LastUpdateUserID;
                objOne.RoleName = list[i].RoleName;
                ltgRole.Add(objOne);
            }
            return ltgRole;
        }

        public List<RO_SysPromissionMTR> GetPermissionByUser(UserMTR user)
        {
            UserStructDataContext UserStruct = new UserStructDataContext(_conn);
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            //List<RO_Permission> ltg = new List<RO_Permission>();

            var Rolelist = (
                        from ur in UserStruct.UserRole
                        where ur.UserID == user.ID
                        select ur).ToList();

            var list = (
                from r in Rolelist
                from rp in RoleStruct.RolePromission
                from p in RoleStruct.RO_SysPromissionMTR
                where r.RoleID == rp.RoleID
               && rp.RightID == p.ID
                select p).ToList();

            return list;
        }

        public List<SysPromissionMTR> GetAllPermissions(Guid roleId)
        {
            RoleStructDataContext RoleStruct = new RoleStructDataContext(_conn);
            List<SysPromissionMTR> ltg = new List<SysPromissionMTR>();
            //ltg = (from r in RoleStruct.RoleMTR
            //       join rp in RoleStruct.RolePromission
            //         on r.ID equals rp.RoleID
            //       join p in RoleStruct.RO_SysPromissionMTR
            //       on rp.RightID equals p.ID
            //       orderby r.ID
            //       select new
            //         {
            //             RoleID=r.ID,
            //             r.RoleName,
            //             RightID=p.ID,
            //             p.PermissionName,
            //             ModelID=p.ModuleID,
            //             p.RO_SysModuleMTR
            //         }).ToList();
            return ltg;
        }

        public List<SysModuleMTR> GetAllModules()
        {
            ModuleStructDataContext RoleStruct = new ModuleStructDataContext(_conn);
            List<SysModuleMTR> ltg = new List<SysModuleMTR>();
            ltg = (from s in RoleStruct.SysModuleMTR
                   orderby s.ID
                   select s).ToList();
            return ltg;
        }

        public List<PermissionModel> GetAllModuleList()
        {
            ModuleStructDataContext ModuleStruct = new ModuleStructDataContext(_conn);
            var grpPermission = from a in ModuleStruct.SysPromissionMTR
                                join b in ModuleStruct.SysModuleMTR on a.ModuleID equals b.ID
                                into grp
                                from cp in grp.DefaultIfEmpty()
                                orderby a.SysCode,a.Seq
                                select new PermissionModel
                                {
                                    ID = a.ID.ToString()
                                    ,
                                    ModuleID = cp == null ? a.ModuleID.ToString() : cp.ID.ToString()
                                    ,
                                    ModuleName = cp == null ? null : cp.ModuleName
                                    ,
                                    PermissionName = a.PermissionName
                                    ,
                                    Seq = a.Seq.Value
                                    ,
                                    SysCode = a.SysCode
                                };
            return grpPermission.ToList();
        }
    }
}
