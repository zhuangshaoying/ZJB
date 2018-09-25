using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using ZJB.Core.Data;


namespace ZJB.Api.DAL
{
    public class BaseDal: BaseData
    {
        public BaseDal(string connectionStringName)
            : base(connectionStringName, DbConfig.Load().DictionaryConfigurationSource)
        {
        }
        public MongoServer GetMongoClient(string connectName)
        {
            return new MongoClient(System.Configuration.ConfigurationManager.AppSettings[connectName]).GetServer();

        }
    }
}