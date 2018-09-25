using System;
using System.Configuration;

namespace ZJB.Web.Session
{
    public class PartitionResolver : System.Web.IPartitionResolver
    {
        private string[] partitions;

        public void Initialize()
        {
            string listString = ConfigurationManager.AppSettings["StateServerList"];
            if (string.IsNullOrEmpty(listString))
            {
                partitions = new []{"tcpip=192.168.0.36:42424"};
            }
            else
            {
                partitions = listString.Split(new [] {';'}, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public string ResolvePartition(object key)
        {
            string sessionId = key as string;
            int index = Math.Abs(sessionId.GetHashCode()) % partitions.Length;
            return partitions[index];
        }
    }
}
