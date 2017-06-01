using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Security.Model;
using EML.Util;
using System.Data;
namespace Security.DataAccess
{
    public class DepartmentDal
    {
        public List<Department> GetDepartmentTreeList()
        {
            List<Department> lst = new List<Department>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rCustomDataMTRByTypeCode";
                    cmd.Parameters.Add(new SqlParameter("@TypeCode", "Department"));
                    using (SqlDataReader sr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (sr.Read())
                        {
                            Department d = new Department();
                            d.ID = CommonDal.ReadString(sr["ID"]);
                            d.ParentID = CommonDal.ReadString(sr["ParentID"]);
                            d.Name = CommonDal.ReadString(sr["Name"]);
                            d.TypeCode = CommonDal.ReadString(sr["TypeCode"]);
                            lst.Add(d);
                        }

                    }
                    return lst;



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

        public List<Department> GetDepartmentByUser(string ID)
        {
            List<Department> listItems = new List<Department>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rDepartmentRelation";
                    cmd.Parameters.Add(new SqlParameter("@UserID", ID));
                    //cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Department model = new Department();
                            model.ID = CommonDal.ReadString(dr["DepartmentID"]);
                            model.Name = CommonDal.ReadString(dr["DepartmentName"]);
                            listItems.Add(model);
                        }
                    }
                    return listItems;
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
