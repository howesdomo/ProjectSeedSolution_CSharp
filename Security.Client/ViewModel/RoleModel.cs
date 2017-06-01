using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Security.Client.SecurityServer;


namespace Security.Client.ViewModel
{
   public class RoleModel : RoleMTR
    {

       public RoleModel(): base()
        {

        }

       public RoleMTR ToRole()
       {
           return new RoleMTR()
           {
               Effectiveness = this.Effectiveness,
               ID = this.ID ,
               IsAdminstrator = this.IsAdminstrator,
               LastUpdateDatetime = this.LastUpdateDatetime ,
               LastUpdateUserID = this.LastUpdateUserID,
               RoleName = this.RoleName,
               RolePromission = this.RolePromission
           };
       }

       public RoleModel Clone()
       {
           RoleModel r = new RoleModel
           {
               Effectiveness = this.Effectiveness,
               ID = this.ID,
               IsAdminstrator = this.IsAdminstrator,
               LastUpdateDatetime = this.LastUpdateDatetime,
               LastUpdateUserID = this.LastUpdateUserID,
               RoleName = this.RoleName,
           };
           RolePromission[] temp = new RolePromission[this.RolePromission.Count()];
           this.RolePromission.CopyTo(temp, 0);
           if (r.RolePromission == null)
               r.RolePromission = new RolePromission[]{};
           r.RolePromission = temp;
           return r;
       }
       public static RoleModel ConvertToChild(RoleMTR r)
       {
           RoleModel re = new RoleModel();
           re.Effectiveness = r.Effectiveness;
           re.ID = r.ID;
           re.IsAdminstrator = r.IsAdminstrator;
           re.LastUpdateDatetime = r.LastUpdateDatetime;
           re.LastUpdateUserID = r.LastUpdateUserID;
           re.RoleName = r.RoleName;
           re.RolePromission = r.RolePromission;
           return re;
       }
       public static ObservableCollection<RoleModel> ConvertToList(List<RoleMTR> l)
       {
           ObservableCollection<RoleModel> list = new ObservableCollection<RoleModel>();
           foreach (RoleMTR r in l)
           {
               list.Add(RoleModel.ConvertToChild(r));
           }
           return list;
       }

       public static ObservableCollection<RO_User> ConvertROUser(List<RO_User> listUser)
       {
           ObservableCollection<RO_User> list = new ObservableCollection<RO_User>();
           foreach (RO_User r in listUser)
           {
               list.Add(r);
           }
           return list;
       }

       public bool Vis()
       {
           if (string.IsNullOrEmpty(this.RoleName))
           {
               SetError("RoleName", "请输入组别名称");
               return false;
           }
           else
           {
               ClearAllErrors();
           }
           if (this.RoleName.Length >= 25)
           {
               SetError("RoleName", "名字过长！");
               return false;
           }
           else
           {
               ClearAllErrors();
               return true;
           }
       }

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
    }
}
