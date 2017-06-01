using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using EML.Util;
using Security.Model;
using System.Data;

namespace Security.DataAccess
{
    public class RoleDal
    {
        public List<Role> GetRoleListByUserID(string userID)
        {
            List<Role> list = new List<Role>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rRoleByUserID";
                    cmd.Parameters.Add(new SqlParameter("@UserID", userID));
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            string id = dr["ID"].ToString();
                            string name = dr["RoleName"].ToString();

                            Role model = new Role(id, name);
                            list.Add(model);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
            return list;
        }

    }
}
