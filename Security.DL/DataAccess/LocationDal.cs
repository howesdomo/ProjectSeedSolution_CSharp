using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Security.Model;
using EML.Util;

namespace Security.DataAccess
{
    public class LocationDal
    {
        #region Location ControlKey


        public List<Location> GetLocationProvideLocationID()
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationProvideLocationID";

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            model.ProvideLocationID = dr["ProvideLocationID"].ToString();

                            list.Add(model);
                        }
                    }
                    return list;
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


        public List<Location> GetLocationProvideLocationIDByTypeControlKey(string key)
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationProvideLocationIDByTypeControlKey";
                    cmd.Parameters.Add(new SqlParameter("@ControlKey", key));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            model.ProvideLocationID = dr["ProvideLocationID"].ToString();

                            list.Add(model);
                        }
                    }
                    return list;
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


        public List<LocationType> GetLocationTypeByTypeControlKey(string key)
        {
            List<LocationType> list = new List<LocationType>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rSysLocationTypeMTRByTypeControlKey";
                    cmd.Parameters.Add(new SqlParameter("@ControlKey", key));
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            LocationType model = new LocationType();
                            model.ID = string.IsNullOrEmpty(dr["ID"].ToString()) == true ? 0 : Convert.ToInt32(dr["ID"].ToString());
                            model.Name = dr["Name"].ToString();
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

        public List<Location> GetLocationByTypeControlKey(string key)
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByTypeControlKey";
                    cmd.Parameters.Add(new SqlParameter("@ControlKey", key));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            list.Add(model);
                        }
                    }
                    return list;
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

        public List<Location> GetLocationByTypeControlKeyParentID(string key, string parentID)
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByTypeControlKeyParentID";

                    cmd.Parameters.Add(new SqlParameter("@ControlKey", key));
                    cmd.Parameters.Add(new SqlParameter("@ParentID", SqlDbType.UniqueIdentifier) { Value = new Guid(parentID) });
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            model.Capacity = Convert.ToDecimal(dr["Capacity"]);
                            model.UM = dr["UM"].ToString();
                            model.UMName = dr["UMName"].ToString();
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

        #endregion

        public List<Location> GetLoctionByCompanyCode(string companyCode)
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByCompanyCode";
                    cmd.Parameters.AddWithValue("@CompanyCode", companyCode);
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            list.Add(model);
                        }
                        dr.Close();
                    }
                    return list;
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

        public List<Location> GetAllLocationListByUser(string userId)
        {
            List<Location> list = new List<Location>();
            if (string.IsNullOrEmpty(userId))
            {
                return list;
            }
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rAllLocationByUser";
                    cmd.Parameters.Add(new SqlParameter("@UserID", userId));
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
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


        public List<Location> GetLocationTreeList()
        {
            List<Location> list = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocation";
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            list.Add(model);
                        }
                        dr.Close();
                    }
                    return list;
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

        public List<Location> GetLocationListByUser(string userId, string companyCode)
        {
            List<Location> listItems = new List<Location>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.SecurityConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByUser";
                    cmd.Parameters.Add(new SqlParameter("@UserID", userId));
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            Location model = new Location();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Code = dr["LocationCode"].ToString();
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

        public bool DeleteLocation(string id, string locationCode)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dLocation";
                    cmd.Parameters.Add(new SqlParameter("@ID", id));
                    cmd.Parameters.Add(new SqlParameter("@LocationCode", locationCode));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                    return val > 0;
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

        public bool UpdateLocation(Location model)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "uLocation";
                    cmd.Parameters.Add(new SqlParameter("@ID", model.ID));
                    cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                    cmd.Parameters.Add(new SqlParameter("@Code", model.Code));
                    cmd.Parameters.Add(new SqlParameter("@LocationTypeID", model.LocationTypeID));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
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

        public bool CheckLocationIsExist(string parentId, string code)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByParentIDNameCode";
                    cmd.Parameters.Add(new SqlParameter("@ParentID", parentId));
                    cmd.Parameters.Add(new SqlParameter("@Code", code));
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    int recordCount = 0;
                    int.TryParse(val.ToString(), out recordCount);

                    return recordCount > 0;
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

        public bool Insert(Location model)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "cLocation";
                    cmd.Parameters.Add(new SqlParameter("@ID", model.ID));
                    cmd.Parameters.Add(new SqlParameter("@ParentID", model.ParentID));
                    cmd.Parameters.Add(new SqlParameter("@LocationTypeID", model.LocationTypeID));
                    cmd.Parameters.Add(new SqlParameter("@Name", model.Name));
                    cmd.Parameters.Add(new SqlParameter("@Code", model.Code));
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", model.CompanyCode));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
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

        public bool InsertLocationCapacity(string companyCode, string provideLocationID, decimal capacity, string UM)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "cLocationCapacity";
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));
                    cmd.Parameters.Add(new SqlParameter("@ProvideLocationID", provideLocationID));
                    cmd.Parameters.Add(new SqlParameter("@Capacity", capacity));
                    cmd.Parameters.Add(new SqlParameter("@UM", UM));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                    return val > 0;
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

        public bool UpdateLocatorAndCapacity(string locationID, string name, decimal capacity, string UM)
        {

            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "uLocatorAndCapacity";
                    cmd.Parameters.Add(new SqlParameter("@ID", locationID));
                    cmd.Parameters.Add(new SqlParameter("@Name", name));
                    cmd.Parameters.Add(new SqlParameter("@Capacity", capacity));
                    cmd.Parameters.Add(new SqlParameter("@UM", UM));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();

                    return val > 0;
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

        public bool DeleteLocatorAndCapacity(string locationID)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dLocatorAndCapacity";
                    cmd.Parameters.Add(new SqlParameter("@ID", locationID));
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    conn.Close();
                    return val > 0;
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

        /// <summary>
        /// 检查库位是否有库存
        /// </summary>
        /// <param name="locationCode">库位ID</param>
        /// <param name="locationTypeID">库位类型</param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public bool CheckrContainerRelationForLocator(string locationCode, int locationTypeID, string companyCode)
        {
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rExistContainerRelationForLocator";
                    cmd.Parameters.Add(new SqlParameter("@LocationCode", locationCode));
                    cmd.Parameters.Add(new SqlParameter("@LocationTypeID", locationTypeID));
                    cmd.Parameters.Add(new SqlParameter("@CompanyCode", companyCode));
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    conn.Close();
                    int recordCount = 0;
                    int.TryParse(val.ToString(), out recordCount);

                    return recordCount > 0;

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

        public List<LocationType> GetLocationTypeList()
        {
            List<LocationType> listItems = new List<LocationType>();
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rSysLocationTypeMTR";
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            LocationType model = new LocationType();
                            model.ID = string.IsNullOrEmpty(dr["ID"].ToString()) == true ? 0 : Convert.ToInt32(dr["ID"].ToString());
                            model.Name = dr["Name"].ToString();
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

        public Location GetLocationByCode(string companyCode, string locationCode, int locationTypeID)
        {
            Location model = null;
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByCode";
                    cmd.Parameters.AddWithValue("@CompanyCode", companyCode);
                    cmd.Parameters.AddWithValue("@LocationCode", locationCode);
                    cmd.Parameters.AddWithValue("@LocationTypeID", locationTypeID);
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
                        }
                        dr.Close();
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


        // Add By howe
        public Location GetLocationByName(string companyCode, string locationName, int locationTypeID)
        {
            Location model = null;
            using (SqlConnection conn = new SqlConnection(SQLHelper.LogisticsConnString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "rLocationByName";
                    cmd.Parameters.AddWithValue("@CompanyCode", companyCode);
                    cmd.Parameters.AddWithValue("@LocationName", locationName);
                    cmd.Parameters.AddWithValue("@LocationTypeID", locationTypeID);
                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            model = new Location();
                            model.ID = dr["ID"].ToString();
                            model.ParentID = dr["ParentID"].ToString();
                            if (!string.IsNullOrEmpty(dr["LocationTypeID"].ToString()))
                            {
                                model.LocationTypeID = Convert.ToInt32(dr["LocationTypeID"].ToString());
                            }
                            model.Name = dr["Name"].ToString();
                            model.Code = dr["Code"].ToString();
                            model.CompanyCode = dr["CompanyCode"].ToString();
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
