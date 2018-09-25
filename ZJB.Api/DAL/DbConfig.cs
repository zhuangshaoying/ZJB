using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using ImageApi.Service.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;



namespace ZJB.Api.DAL
{
    public class DbConfig
    {
        public List<Connection> Connections { get; set; }
        public List<Provider> Providers { get; set; }


        private DbConfig()
        {
        }

        private static DbConfig apiConfig;

        [XmlIgnore]
        public DictionaryConfigurationSource DictionaryConfigurationSource
        {
            get;
            private set;
        }

        private void InitDictionaryConfigurationSource()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            if (Connections != null)
            {
                ConnectionStringsSection section = new ConnectionStringsSection();
                foreach (Connection conn in Connections)
                {
                    section.ConnectionStrings.Add(new ConnectionStringSettings(conn.Name, conn.ConnectionString, conn.ProviderName));
                }
                configurationSource.Add("connectionStrings", section);
            }

            if (Providers != null)
            {
                DatabaseSettings settings = new DatabaseSettings();
                foreach (Provider provider in Providers)
                {
                    DbProviderMapping providerMapping = new DbProviderMapping(provider.Name, provider.DatabaseType);
                    settings.ProviderMappings.Add(providerMapping);
                }
                configurationSource.Add(DatabaseSettings.SectionName, settings);
            }

            DictionaryConfigurationSource = configurationSource;
        }

        public static DbConfig Load()
        {
            if (apiConfig != null) return apiConfig;
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config\WXDb.config");
            if (!File.Exists(configPath))
            {
                configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"WXDb.config");
            }
            if (!File.Exists(configPath))
            {
                configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\WXDb.config");
            }
            using (StreamReader reader = new StreamReader(configPath, Encoding.UTF8))
            {
                string xml = reader.ReadToEnd();
                apiConfig = XmlUtility.Deserialize<DbConfig>(xml);
                apiConfig.InitDictionaryConfigurationSource();
            }

            return apiConfig;
        }
    }

    public class Connection
    {
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string ConnectionString { get; set; }
    }

    public class Provider
    {
        public string Name { get; set; }
        public string DatabaseType { get; set; }
    }
}