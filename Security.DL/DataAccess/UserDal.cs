using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Security.Model;
using Security.DataAccess;
using EML.Util;

namespace Security.DataAccess
{
    public class UserDal
    {

        public User GetUserForLogin(string account, string password, string companyCode)
        {
            string cryptPSW = CryptUtil.Encrypt(password);

            User model = null;
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rUserForLogin";
                    cmd.Parameters.Add(new SqlParameter("@Account", account));
                    cmd.Parameters.Add(new SqlParameter("@Password", cryptPSW));
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            string id = dr["ID"].ToString();
                            string userName = dr["UserName"].ToString();
                            //string account = dr["ParentID"].ToString();
                            //string password = dr["Password"].ToString();
                            string accountResult = CommonDal.ReadString(dr["LoginAccount"]);
                            model = new User(id, accountResult, password, userName);
                            model.CompanyCode = companyCode;
                        }
                    }
                    return model;
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
        }


        public int ChangePassword(string account, string oldPassword, string newPassword, string companyCode)
        {
            string cryptOldPSW = CryptUtil.Encrypt(oldPassword);
            string cryptNewPSW = CryptUtil.Encrypt(newPassword);

            int count = -1;
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "uUserPassword";
                    cmd.Parameters.Add(new SqlParameter("@Account", account));
                    cmd.Parameters.Add(new SqlParameter("@OldPassword", cryptOldPSW));
                    cmd.Parameters.Add(new SqlParameter("@NewPassword", cryptNewPSW));
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));

                    count = cmd.ExecuteNonQuery();
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
            return count;
        }

        public List<User> GetUserByUserName(string userName)
        {
            List<User> userList = new List<User>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rUserByUserName";
                    cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            User model = new User();
                            model.ID = dr["ID"].ToString();
                            model.UserName = dr["UserName"].ToString();
                            model.Account = dr["LoginAccount"].ToString();
                            userList.Add(model);
                        }
                    }
                    return userList;
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
        }

    }
}
