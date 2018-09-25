using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using ZJB.Core.Utilities;

namespace ZJB.Api.Common
{
    public class HouseUtility
    {
       public static int GetHouseImgType(string typeName)
        {
            int type = 1;
            switch (typeName)
            {
                case "IMG_I" :  //室内图
                    type = 1; break;
                case "IMG_M":  //房型图
                    type = 2; break;
                case "IMG_O":  //小区图
                    type = 3; break;
                default: break;
            }

            return type;
        }
       public static string GetHouseImgType(int typeID)
       {
           string type = "IMG_I";
           switch (typeID)
           {
               case 1:  //室内图
                   type ="IMG_I"; break;
               case 2:  //房型图
                   type = "IMG_M"; break;
               case 3:  //小区图
                   type = "IMG_O"; break;
               default: break;
           }

           return type;
       }
       public static string GetHouseImgTypeName(int typeID)
       {
           string type = "室内图";
           switch (typeID)
           {
               case 1:  //室内图
                   type = "室内图"; break;
               case 2:  //房型图
                   type = "房型图"; break;
               case 3:  //小区图
                   type = "小区图"; break;
               default: break;
           }

           return type;
       }
    }
}
