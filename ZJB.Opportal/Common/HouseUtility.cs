using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using ZJB.Core.Utilities;

namespace ZJB.Opportal.Common
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
                case "IMG_O":  //房型图
                    type = 2; break;
                case "IMG_M":  //小区图
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
                   type = "IMG_O"; break;
               case 3:  //小区图
                   type = "IMG_M"; break;
               default: break;
           }

           return type;
       }
      
    }
}
