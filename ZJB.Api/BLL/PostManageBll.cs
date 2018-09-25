using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
namespace ZJB.Api.BLL
{
     public class PostManageBll
    {
         private readonly PostManageDal postManageDal = Container.Instance.Resolve<PostManageDal>();

         #region 新增发布和预约发布

         public virtual int BatchPostManageAdd(int userId,int releaseType,List<PostManageModel> postList,List<PostLogModel> postLogList=null)
         {
             StringBuilder postData =new StringBuilder();
             postData.Append(XmlUtility.Serialize(postList, Encoding.UTF8, "PostManageList"));
            
             return postManageDal.BatchPostManageAdd(userId, releaseType, postData.ToString(), postLogList);
         }

         #endregion

        #region 预约管理列表
         public virtual List<PostManageModel> GetAppointLogList(AppointLogListReq parame, ref int totalSize)
         {
             return postManageDal.GetAppointLogList(parame,ref totalSize);
         }
        #endregion

        #region 预约删除
         public virtual int DeleteAppoint(int userId, string houseIds)
         {
             return postManageDal.DeleteAppoint(userId, houseIds);
         }
        #endregion
    }
}
