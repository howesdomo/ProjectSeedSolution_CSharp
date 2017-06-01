using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DL.Model
{
   public static class SQLConvert
   {
       #region APP 收货扫描

       //public static DataTable CreateDataTable_ReceiptOrderScan_Carton()
       //{
       //    DataTable dt = new DataTable();

       //    dt.Columns.Add("LineNo");
       //    dt.Columns.Add("CartonNo");
       //    dt.Columns.Add("Qty", typeof(decimal));
       //    dt.Columns.Add("SNP", typeof(decimal));
       //    dt.Columns.Add("ProductCode");
       //    dt.Columns.Add("ProductName");
       //    dt.Columns.Add("VendorCode");
       //    dt.Columns.Add("OrderNo");
       //    dt.Columns.Add("BatchNo");
       //    dt.Columns.Add("ProductionDate");
       //    dt.Columns.Add("QRCode");

       //    return dt;
       //}

       //public static DataTable GetDataTable_ReceiptOrderScan_Carton(this IEnumerable<APP_Carton> l)
       //{
       //    DataTable dt = CreateDataTable_ReceiptOrderScan_Carton();
       //    foreach (var item in l)
       //    {
       //        DataRow dr = dt.NewRow();

       //        dr["LineNo"] = item.LineNo;
       //        dr["CartonNo"] = item.CartonNo;
       //        dr["Qty"] = item.Qty;
       //        dr["SNP"] = item.SNP;
       //        dr["ProductCode"] = item.ProductCode;
       //        dr["ProductName"] = item.ProductName;
       //        dr["VendorCode"] = item.VendorCode;
       //        dr["OrderNo"] = item.ReceiptOrderNo;
       //        dr["BatchNo"] = item.BatchNo;
       //        dr["ProductionDate"] = item.ProductionDate;
       //        dr["QRCode"] = item.QRCode;

       //        dt.Rows.Add(dr);
       //    }
       //    return dt;
       //}

       //public static DataTable CreateDataTable_ReceiptOrderDetailCarton()
       //{
       //    DataTable dt = new DataTable();

       //    dt.Columns.Add("ReceiptOrderDetailID");
       //    dt.Columns.Add("CartonNo");

       //    return dt;
       //}

       //public static DataTable GetDataTable_ReceiptOrderDetailCarton(this IEnumerable<APP_Carton> l)
       //{
       //    DataTable dt = CreateDataTable_ReceiptOrderDetailCarton();
       //    foreach (var item in l)
       //    {
       //        DataRow dr = dt.NewRow();

       //        dr["ReceiptOrderDetailID"] = item.ReceiptOrderDetailID;
       //        dr["CartonNo"] = item.CartonNo;

       //        dt.Rows.Add(dr);
       //    }
       //    return dt;
       //}

       #endregion

    }
}
