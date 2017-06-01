using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using EML.FUTURISTIC.Util;

namespace EML.FUTURISTIC.Production.Server
{
    public class ConfigHandler
    {
        public static string BTWFilePath { get; set; }
        public static string BTWFile { get; set; }
        public static string BTWSmallLabelFile { get; set; }
        public static string PictureFilePath { get; set; }
        public static string PictureFileExe { get; set; }
        public static string BarcodeConfigPath { get; set; }

        public static void InitConfig(HttpServerUtility server)
        {
            SQLHelper.SecurityConnString = ConfigurationManager.ConnectionStrings["Security"].ToString();
            SQLHelper.LogisticsConnString = ConfigurationManager.ConnectionStrings["Logistics"].ToString();
            BTWFilePath = ConfigurationManager.AppSettings["BTWFilePath"].ToString();
            BTWFile = ConfigurationManager.AppSettings["BTWFile"].ToString();
            BTWSmallLabelFile = ConfigurationManager.AppSettings["BTWSmallLabelFile"].ToString();
            PictureFilePath = ConfigurationManager.AppSettings["PictureFilePath"].ToString();
            PictureFileExe = ConfigurationManager.AppSettings["PictureFileExe"].ToString();

            BarcodeConfigPath = ConfigurationManager.AppSettings["BarcodeConfigPath"].ToString();

            string path = server.MapPath("~/" + ConfigHandler.BTWFilePath);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            string configPath = server.MapPath("~/" + ConfigHandler.BarcodeConfigPath);
            if (!System.IO.Directory.Exists(configPath))
            {
                System.IO.Directory.CreateDirectory(configPath);
            }

            //string picpath = server.MapPath("~/" + ConfigHandler.PictureFilePath);
            //if (!System.IO.Directory.Exists(picpath))
            //{
            //    System.IO.Directory.CreateDirectory(picpath);
            //}
        }
    }
}