using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Security.Model;
using System.Data.SqlClient;
using EML.Util;
using System.Data;
using System.Collections;

namespace Security.DataAccess
{
    public class ModuleDal
    {
        public List<Permission> GetPromissionListForLogin(List<Role> roleList)
        {
            List<Permission> result = new List<Permission>();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID"));

            foreach (Role item in roleList)
            {
                dt.Rows.Add(item.ID);
            }

            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.CommandText = "rModuleListForLogin";
                    cmd.Parameters.Add(new SqlParameter("@RoleID", dt));
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            Permission promission = new Permission();
                            promission.ModuleID = dr["ModuleID"].ToString();
                            promission.ModuleName = dr["ModuleName"].ToString();
                            promission.PermissionID = dr["PermissionID"].ToString();
                            promission.Name = dr["PermissionName"].ToString();
                            promission.Code = dr["Code"].ToString();
                            promission.Seq = Convert.ToInt32(dr["Seq"].ToString());
                            promission.ClassName = dr["ClassName"].ToString();
                            result.Add(promission);
                        }
                    }
                }
                catch
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }

            return result;
        }

    }
}
