using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using ZJB.Api.Common;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common
{
    public static class ImageHelper
    {
        public static string GrabPic(string imgUrl,bool isLocalHost=false)
        {
            string fileExt = string.Empty;
            string newUrl = imgUrl;
            byte[] buffer = Core.Utilities.HttpUtility.GetData(imgUrl, null, false);
            int extIndex = imgUrl.LastIndexOf(".");
            fileExt = imgUrl.Substring(extIndex, imgUrl.Length - extIndex);
            using (MemoryStream target = new MemoryStream(buffer))
            {
                if (isLocalHost)
                {
                    newUrl = QiniuService.PutFileStringStream(target, "WX", fileExt);
                }
                else
                {
                    byte[] data = target.ToArray();
                    string md5 = ToMd5String(data);
                    var path = @"Images\temp\";
                    var fullPath = AppDomain.CurrentDomain.BaseDirectory + path;
                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);
                    System.IO.File.WriteAllBytes(fullPath + md5 + fileExt, data);
                    newUrl = ConfigUtility.GetValue("QiNiuDomain") + md5 + fileExt;
                }
            }
            return newUrl;
        }
        private static string ToMd5String(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashResult = md5.ComputeHash(data);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }
    }
}