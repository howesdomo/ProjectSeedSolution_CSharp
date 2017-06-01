using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

using EML.Util;
using Security.Model;

namespace Security.DataAccess
{
    public class ProvideLocationDal
    {
        public List<ProvideLocation> GetProvideLocationByParent(string id)
        {
            List<ProvideLocation> list = new List<ProvideLocation>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rProvideLocationByParent";
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            ProvideLocation model = new ProvideLocation();
                            model.ID = dr["ID"].ToString();
                            model.LocationCode1 = dr["LocationCode_1"].ToString();
                            model.LocationName1 = dr["LocationName_1"].ToString();
                            model.LocationCode2 = dr["LocationCode_2"].ToString();
                            model.LocationName2 = dr["LocationName_2"].ToString();
                            model.LocationCode3 = dr["LocationCode_3"].ToString();
                            model.LocationName3 = dr["LocationName_3"].ToString();
                            model.LocationCode4 = dr["LocationCode_4"].ToString();
                            model.LocationName4 = dr["LocationName_4"].ToString();
                            model.LocationCode5 = dr["LocationCode_5"].ToString();
                            model.LocationName5 = dr["LocationName_5"].ToString();

                            model.LocationTypeID_1 = CommonDal.ReadInt(dr["LocationTypeID_1"]);
                            model.LocationTypeID_2 = CommonDal.ReadInt(dr["LocationTypeID_2"]);
                            model.LocationTypeID_3 = CommonDal.ReadInt(dr["LocationTypeID_3"]);
                            model.LocationTypeID_4 = CommonDal.ReadInt(dr["LocationTypeID_4"]);
                            model.LocationTypeID_5 = CommonDal.ReadInt(dr["LocationTypeID_5"]);

                            list.Add(model);
                        }
                        dr.Close();
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    throw ex;
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


        public bool InsertProvideLocation(ProvideLocation model)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "cProvideLocation";
                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@LocationCode_1", model.LocationCode1);
                    cmd.Parameters.AddWithValue("@LocationCode_2", model.LocationCode2);
                    cmd.Parameters.AddWithValue("@LocationCode_3", model.LocationCode3);
                    cmd.Parameters.AddWithValue("@LocationCode_4", model.LocationCode4);
                    cmd.Parameters.AddWithValue("@LocationCode_5", model.LocationCode5);
                    cmd.Parameters.AddWithValue("@LocationName_1", model.LocationName1);
                    cmd.Parameters.AddWithValue("@LocationName_2", model.LocationName2);
                    cmd.Parameters.AddWithValue("@LocationName_3", model.LocationName3);
                    cmd.Parameters.AddWithValue("@LocationName_4", model.LocationName4);
                    cmd.Parameters.AddWithValue("@LocationName_5", model.LocationName5);
                    if (model.LocationTypeID_1 == 0)
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_1", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_1", model.LocationTypeID_1);
                    }
                    if (model.LocationTypeID_2 == 0)
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_2", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_2", model.LocationTypeID_2);
                    }
                    if (model.LocationTypeID_3 == 0)
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_3", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_3", model.LocationTypeID_3);
                    }
                    if (model.LocationTypeID_4 == 0)
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_4", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_4", model.LocationTypeID_4);
                    }
                    if (model.LocationTypeID_5 == 0)
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_5", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@LocationTypeID_5", model.LocationTypeID_5);
                    }
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    if (val > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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

        public List<ProvideLocation> GetProvideLocationList()
        {
            List<ProvideLocation> listItems = new List<ProvideLocation>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rProvideLocation";
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            ProvideLocation model = new ProvideLocation();
                            model.ID = dr["ID"].ToString();
                            model.LocationCode1 = dr["LocationCode_1"].ToString();
                            model.LocationName1 = dr["LocationName_1"].ToString();
                            model.LocationCode2 = dr["LocationCode_2"].ToString();
                            model.LocationName2 = dr["LocationName_2"].ToString();
                            model.LocationCode3 = dr["LocationCode_3"].ToString();
                            model.LocationName3 = dr["LocationName_3"].ToString();
                            model.LocationCode4 = dr["LocationCode_4"].ToString();
                            model.LocationName4 = dr["LocationName_4"].ToString();
                            model.LocationCode5 = dr["LocationCode_5"].ToString();
                            model.LocationName5 = dr["LocationName_5"].ToString();

                            //model.LocationTypeID_1 = Int32.Parse(dr["LocationTypeID_1"].ToString());
                            //model.LocationTypeID_2 = Int32.Parse(dr["LocationTypeID_2"].ToString());
                            //model.LocationTypeID_3 = Int32.Parse(dr["LocationTypeID_3"].ToString());
                            //model.LocationTypeID_4 = Int32.Parse(dr["LocationTypeID_4"].ToString());
                            //model.LocationTypeID_5 = Int32.Parse(dr["LocationTypeID_5"].ToString());

                            model.LocationTypeID_1 = CommonDal.ReadInt(dr["LocationTypeID_1"]);
                            model.LocationTypeID_2 = CommonDal.ReadInt(dr["LocationTypeID_2"]);
                            model.LocationTypeID_3 = CommonDal.ReadInt(dr["LocationTypeID_3"]);
                            model.LocationTypeID_4 = CommonDal.ReadInt(dr["LocationTypeID_4"]);
                            model.LocationTypeID_5 = CommonDal.ReadInt(dr["LocationTypeID_5"]);

                            model.DisplayName = string.Format("{0}-{1}", model.LocationCode5, model.LocationName5);
                            listItems.Add(model);
                        }
                    }
                    return listItems;
                }
                catch (Exception ex)
                {
                    throw ex;
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

        public ProvideLocation GetProvideLocationByCode(string companyCode, string locationCode, int locationTypeID)
        {
            ProvideLocation model = null;
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rProvideLocationByCode";
                    cmd.Parameters.AddWithValue("@CompanyCode", companyCode);
                    cmd.Parameters.AddWithValue("@LocationCode5", locationCode);
                    cmd.Parameters.AddWithValue("@LocationTypeID5", locationTypeID);
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            model = new ProvideLocation();
                            model.ID = dr["ID"].ToString();
                            model.LocationCode1 = dr["LocationCode_1"].ToString();
                            model.LocationName1 = dr["LocationName_1"].ToString();
                            model.LocationCode2 = dr["LocationCode_2"].ToString();
                            model.LocationName2 = dr["LocationName_2"].ToString();
                            model.LocationCode3 = dr["LocationCode_3"].ToString();
                            model.LocationName3 = dr["LocationName_3"].ToString();
                            model.LocationCode4 = dr["LocationCode_4"].ToString();
                            model.LocationName4 = dr["LocationName_4"].ToString();
                            model.LocationCode5 = dr["LocationCode_5"].ToString();
                            model.LocationName5 = dr["LocationName_5"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID_1"].ToString()))
                            {
                                model.LocationTypeID_1 = Convert.ToInt32(dr["LocationTypeID_1"]);
                            }
                            if (!string.IsNullOrEmpty(dr["LocationTypeID_2"].ToString()))
                            {
                                model.LocationTypeID_2 = Convert.ToInt32(dr["LocationTypeID_2"]);
                            }
                            if (!string.IsNullOrEmpty(dr["LocationTypeID_3"].ToString()))
                            {
                                model.LocationTypeID_3 = Convert.ToInt32(dr["LocationTypeID_3"]);
                            }
                            if (!string.IsNullOrEmpty(dr["LocationTypeID_4"].ToString()))
                            {
                                model.LocationTypeID_4 = Convert.ToInt32(dr["LocationTypeID_4"]);
                            }
                            if (!string.IsNullOrEmpty(dr["LocationTypeID_5"].ToString()))
                            {
                                model.LocationTypeID_5 = Convert.ToInt32(dr["LocationTypeID_5"]);
                            }
                            break;
                        }
                        dr.Close();
                    }
                    return model;
                }
                catch (Exception ex)
                {
                    throw ex;
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
