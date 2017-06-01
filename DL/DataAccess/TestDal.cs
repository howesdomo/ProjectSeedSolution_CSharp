using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DL.DataAccess
{
    public class TestDal
    {
        public DateTime GetDBTime()
        {
            DateTime r = DateTime.MinValue;
            using (SqlConnection conn = new SqlConnection(EML.Util.SQLHelper.LogisticsConnString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select getdate() as DBTime";
                cmd.CommandType = CommandType.Text;
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        r = Util.CommonDal.ReadDateTimeWithNoNullable(dr["DBTime"]);
                    }
                }
            }

            return r;
        }
    }
}
