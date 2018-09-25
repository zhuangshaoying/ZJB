using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Policy;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
namespace ZJB.Api.DAL
{
    public class SiteManageDal : BaseDal
    {
        public SiteManageDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("SiteManageDal");
        private NCBaseRule ncBase = new NCBaseRule();

        #region 获取全部站点
        public virtual List<SiteManageModel> GetSiteList(string buildType, int cityId)
        {

            DbCommand cmd = GetStoredProcCommand("P_User_GetSiteList");
            AddInParameter(cmd, "@buildType", DbType.String, buildType);
            AddInParameter(cmd, "@cityId", DbType.Int32, cityId);
            DataSet ds = ExecuteDataSet(cmd);
            List<SiteManageModel> siteManageList = new List<SiteManageModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    siteManageList.Add(new SiteManageModel()
                    {

                        CityID = To<Int32>(dr, "CityID"),
                        Logo = To<string>(dr, "Logo"),
                        SiteID = To<Int32>(dr, "SiteID"),
                        SiteName = To<string>(dr, "SiteName"),
                        Status = To<byte>(dr, "Status"),
                        YunRefresh = To<bool>(dr, "YunRefresh"),
                        LoginUrl = To<string>(dr, "LoginUrl"),
                        RegisterUrl = To<string>(dr, "RegisterUrl"),


                    });
                }
            }
            return siteManageList;
         //List<SiteManage> siteManageList = (buildType == "" ? ncBase.CurrentEntities.SiteManage.Where(s => s.Status == 1 && s.CityID == cityId).ToList() : ncBase.CurrentEntities.SiteManage.Where(s => s.Status == 1 && s.CityID == cityId && (!s.NoSupportBuildType.Contains(buildType) || s.NoSupportBuildType == null || s.NoSupportBuildType == "")).ToList());
         //return EFMapToModelList(siteManageList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="noSupportBuildTyeList">不支持的类型</param>
        /// <param name="buildType">要比较的类型</param>
        /// <returns></returns>
        //private bool isSupportBuildType(string noSupportBuildTyeList,int buildType = 0)
        //{
        //    List<string> noSupportList = string.IsNullOrEmpty(noSupportBuildTyeList)?new List<string>() : noSupportBuildTyeList.Split(',').ToList();
        //    return !noSupportList.Contains(buildType.ToString());
        //}
        #endregion
        #region EF关系映射
        private List<SiteManageModel> EFMapToModelList(List<SiteManage> siteManageList)
        {
            return (from siteItem in siteManageList select EFMapToModel(siteItem)).ToList();
        }
        private SiteManageModel EFMapToModel(SiteManage siteManage)
        {
            return new SiteManageModel()
            {
                CityID = siteManage.CityID,
                Logo = siteManage.Logo,
                SiteID = siteManage.SiteID,
                SiteName = siteManage.SiteName,
                Status = siteManage.Status,
                YunRefresh = siteManage.YunRefresh,
                LoginUrl=siteManage.LoginUrl ,
                RegisterUrl = siteManage.RegisterUrl
            };
        }
        #endregion
    }
}
