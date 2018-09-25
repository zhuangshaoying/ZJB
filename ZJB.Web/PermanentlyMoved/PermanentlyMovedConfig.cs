using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using ZJB.Core.Utilities;

namespace ZJB.Web.PermanentlyMoved
{
    [XmlRoot("PermanentlyMoveds")]
    public class PermanentlyMovedConfig : List<PermanentlyMoved>
    {
        private static PermanentlyMovedConfig permanentlyMovedConfig;

        public static PermanentlyMovedConfig GetPermanentlyMovedConfig()
        {
                if (permanentlyMovedConfig == null)
                {
                    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PermanentlyMoved.xml");

                    string xml = string.Empty;

                    using(StreamReader reader = new StreamReader(configPath,Encoding.UTF8))
                    {
                        xml = reader.ReadToEnd();
                    }

                    permanentlyMovedConfig = XmlUtility.Deserialize<PermanentlyMovedConfig>(xml);
                }

                return permanentlyMovedConfig;
        }
    }


   public  class PermanentlyMoved
   {
       [XmlAttribute]
        public bool WithQuery{get;set;}
       
       public string SourceUrl{get;set;}

       public string TargetUrl{get;set;}
   }
}
