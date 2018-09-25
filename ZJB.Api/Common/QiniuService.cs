using System;
using System.IO;
using System.Security.Cryptography;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using ZJB.Core.Utilities;


namespace ZJB.Api.Common
{
    public class QiniuService
    {
        static QiniuService()
        {
            //七牛初始化
            Config.ACCESS_KEY = "ANTo1e2dRn12OnoXNicz9PmXTqsfw2c16kaT8igB";
            Config.SECRET_KEY = "8kAjrJtNInFaY0m2Yvj0QH2c4ThUgsldimKyhs6e";
        }


        public static string PutFileStringStream(Stream stream, string bucket = "zhujia", string ext = "")
        {
            MemoryStream imgStream = new MemoryStream();
            stream.CopyTo(imgStream);
            byte[] data = imgStream.ToArray();

            if (data.Length < 30)
            {
                return null;
            }
            string md5 = ToMd5String(data);
            string key = md5 + ext;
            int count = 1;
            string imageUrl = null;
            while (count < 5)
            {
                try
                {
                    imageUrl = PutFile(bucket, key, data);
                    break;
                }
                catch (Exception e)
                {
                    log4net.LogManager.GetLogger("ImageUpload").Error(e.Message);
                    count++;
                }
            }
            return imageUrl;
        }

        public static string PutFile(string bucket, string key, byte[] data)
        {
            try
            {
                var scope = bucket + ":" + key;
                var policy = new PutPolicy(scope);
                string upToken = policy.Token();
                var extra = new PutExtra();
                var client = new IOClient();
                var ret = client.Put(upToken, key, new MemoryStream(data), extra);
                if (ret.OK)
                {
                    return ConfigUtility.GetValue("QiNiuDomain") + ret.key;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                log4net.LogManager.GetLogger("ImageUpload").Error(e.Message);
                return "";
            }
        }

        private static string ToMd5String(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashResult = md5.ComputeHash(data);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }
    }
}
