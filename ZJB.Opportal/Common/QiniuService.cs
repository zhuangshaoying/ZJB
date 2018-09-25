using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using ZJB.Core.Utilities;

namespace ImageApi.Service.Qiniu
{
    public class QiniuService
    {
        static QiniuService()
        {
            //七牛初始化
            Config.ACCESS_KEY = "gmgMXoXcXKp3goNGYCM5KKnJxBvebTKv6SD_gQn3";
            Config.SECRET_KEY = "srZp4stHptlWZIwYOLhadh8qPq0cYO5Jo5ftEfvq";
        }

        public static string PutFileString(string path, string bucket = "loubaapp")
        {
            var result = PutFile(path, bucket);
            var imageUrl = "";
            if (result != null && result.Count > 0)
            {
                foreach (var r in result)
                {
                    imageUrl = r.Key;
                    break;
                }
            }
            return imageUrl;
        }

        public static Dictionary<string, Tuple<int, int>> PutFile(string path, string bucket = "loubaapp")
        {
            if (path == null)
            {
                return null;
            }

            byte[] data;
            try
            {
                data = new WebClient().DownloadData(path);
            }
            catch (Exception)
            {
                return null;
            }
            if (data == null || data.Length < 30)
            {
                return null;
            }
            string md5 = ToMd5String(data);
            Stream stream = new MemoryStream(data);
            int count = 1;
            var imageUrl = "";
            while (count < 5)
            {
                try
                {
                    imageUrl = PutFile(bucket, md5, stream);
                    break;
                }
                catch (Exception e)
                {
                    log4net.LogManager.GetLogger("ImageUpload").Error(e.Message);
                    count++;
                }
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                stream = new MemoryStream(data);
                System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                var obj = new Dictionary<string, Tuple<int, int>>()
                    {
                        {imageUrl, new Tuple<int, int>(image.Width, image.Height)}
                    };
                return obj;
            }
            return null;
        }

        public static string PutFileStringStream(Stream stream, string bucket = "loubaapp")
        {
            var result = PutFile(stream, bucket);
            var imageUrl = "";
            if (result != null && result.Count > 0)
            {
                foreach (var r in result)
                {
                    imageUrl = r.Key;
                    break;
                }
            }
            return imageUrl;
        }

        public static Dictionary<string, Tuple<int, int>> PutFile(Stream stream, string bucket = "loubaapp")
        {
            var br = new BinaryReader(stream);
            byte[] data = br.ReadBytes((int)stream.Length);
            if (data == null || data.Length < 30)
            {
                return null;
            }
            Stream imgStream = new MemoryStream(data);
            string md5 = ToMd5String(data);
            int count = 1;
            var imageUrl = "";
            while (count < 5)
            {
                try
                {
                    imageUrl = PutFile(bucket, md5, imgStream);
                    break;
                }
                catch (Exception e)
                {
                    log4net.LogManager.GetLogger("ImageUpload").Error(e.Message);
                    count++;
                }
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                imgStream = new MemoryStream(data);
                System.Drawing.Image image = System.Drawing.Image.FromStream(imgStream);
                var obj = new Dictionary<string, Tuple<int, int>>()
                    {
                        {imageUrl, new Tuple<int, int>(image.Width, image.Height)}
                    };
                return obj;
            }
            return null;
        }

        public static string PutFile(string bucket, string key, Stream stream)
        {
            try
            {
                var scope = bucket + ":" + key;
                var policy = new PutPolicy(scope);
                string upToken = policy.Token();
                var extra = new PutExtra();
                var client = new IOClient();
                var ret = client.Put(upToken, key, stream, extra);
                if (ret.OK)
                {
                    return string.Format(ConfigUtility.GetValue("QiNiuDomain"), bucket) + ret.key;
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
