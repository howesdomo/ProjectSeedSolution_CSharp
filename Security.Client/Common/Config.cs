using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace Security.Client.Common
{
    public class Config
    {
        public static string LocationTypeControlKey
        {
            get
            {
                return "SecurityMgrUI";
            }
        }

        public static void SetServer(string ip, string port, string app)
        {
             Configuration cfa = Config.GetConfiguration(Config.ServiceSettingsFileName);

            cfa.AppSettings.Settings["ServerIP"].Value = ip;
            cfa.AppSettings.Settings["ServerPort"].Value = port;
            cfa.AppSettings.Settings["ServerApplication"].Value = app;

            cfa.Save(ConfigurationSaveMode.Full);
        }

        public static string GetServerIP()
        {
            return GetValueFromSettings(Config.ServiceSettingsFileName, "ServerIP");
        }

        public static string GetServerPort()
        {
            return GetValueFromSettings(Config.ServiceSettingsFileName, "ServerPort");
        }

        public static string GetServerApplication()
        {
            return GetValueFromSettings(Config.ServiceSettingsFileName, "ServerApplication");
        }

        #region Howe - Config 文件读取 & 保存

        public static string ServiceSettingsFileName
        {
            get { return "ServiceSettings"; }
        }

        private static Configuration GetConfiguration(string configName)
        {
            string m_curPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string m_ConfigFullName = System.IO.Path.Combine(m_curPath, configName.Trim() + ".config");
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = m_ConfigFullName;

            return ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
        }

        public static string GetValueFromSettings(string configName, string key)
        {
            Configuration r = Config.GetConfiguration(configName);
            return r.AppSettings.Settings[key].Value;
        }

        #endregion

    }
}
