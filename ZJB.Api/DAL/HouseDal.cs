using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Models;
using ZJB.Api.Entity;
using System.Data.Objects;
using System.Data.Common;
using System.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using ZJB.Api.Models.Community;
using ZJB.Api.Models.Parame;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;
using System.Text.RegularExpressions;
using ZJB.Api.Model;

namespace ZJB.Api.DAL
{
    public class HouseDal : BaseDal
    {
        public HouseDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("HouseDal");
        private NCBaseRule ncBase = new NCBaseRule();
        const string connectionName = "MongoDbHouseCaijiConnection";
        const string databaseName = "HeZi";

        #region 映射EF
        private List<HouseBasicInfoModel> EFToModelList(List<HouseBasicInfo> houseList)
        {
            return (from item in houseList select EFToModel(item)).ToList();
        }
        private HouseBasicInfoModel EFToModel(HouseBasicInfo houseItem)
        {
            return new HouseBasicInfoModel()
            {
                AddDate = houseItem.AddDate,
                Address = houseItem.Address,
                Balcony = houseItem.Balcony,
                BuildArea = houseItem.BuildArea,
                BuildType = houseItem.BuildType,
                CellLabel = houseItem.CellLabel,
                CityID = houseItem.CityID,
                CommunityID = houseItem.CommunityID,
                CommunityName = houseItem.CommunityName,
                CurFloor = houseItem.CurFloor,
                Distrctid = houseItem.Distrctid,
                DeleteTime = houseItem.DeleteTime,
                ExpireDay = houseItem.ExpireDay,
                FitmentStatus = houseItem.FitmentStatus,
                Hall = houseItem.Hall,
                HouseID = houseItem.HouseID,
                HouseImgPath = houseItem.HouseImgPath,
                HouseLabel = houseItem.HouseLabel,
                InternalNum = houseItem.InternalNum,
                IP = houseItem.IP,
                Kitchen = houseItem.Kitchen,
                LookHouseTime = houseItem.LookHouseTime,
                MaxFloor = houseItem.MaxFloor,
                Note = houseItem.Note,
                PayType = houseItem.PayType,
                PicNum = houseItem.PicNum,
                PointTo = houseItem.PointTo,
                PostTime = houseItem.PostTime,
                Price = houseItem.Price,
                PriceUnit = houseItem.PriceUnit,
                PushTime = houseItem.PushTime,
                RegionID = houseItem.RegionID,
                Room = houseItem.Room,
                Status = houseItem.Status,
                Tag = houseItem.Tag,
                Title = houseItem.Title,
                Toilet = houseItem.Toilet,
                TradeType = houseItem.TradeType,
                UnitPrice = houseItem.UnitPrice,
                UsedArea = houseItem.UsedArea,
                UsedYear = houseItem.UsedYear,
                UserID = houseItem.UserID,
                YiJuHua = houseItem.YiJuHua,
                LowPay = houseItem.LowPay
            };
        }
        #endregion

        #region 获取房源列表
        public virtual List<HouseBasicInfoModel> GetHouseList(HouseListReq parames, ref int totalSize)
        {
            #region 暂时不考虑用该方法
            //  List<HouseBasicInfo> houseBasicInfoList = ncBase.CurrentEntities.HouseBasicInfo.Where(
            //     h =>
            //         parames.userId == h.UserID
            //         &&h.TradeType==(parames.postType==0?1:parames.postType)
            //         &&h.BuildType==(parames.buildingType==0?h.BuildType:parames.buildingType)
            //         &&h.CommunityName == (string.IsNullOrEmpty(parames.cell)?h.CommunityName:parames.cell)
            //         &&h.HouseID==(parames.houseId<=0?h.HouseID:parames.houseId)
            //         &&h.Title.Contains((string.IsNullOrEmpty(parames.title)?h.Title:parames.title))///支持房源编号或者标题搜索
            //         &&h.Status == (parames.buildingStatus==null||parames.buildingStatus <= 0 ? h.Status : parames.buildingStatus)
            //         )
            //         .OrderByDescending(s => s.PostTime)
            //         .Skip((parames.page-1)*parames.pageSize)
            //         .Take(parames.pageSize)
            //         .ToList();
            //int recorSize = houseBasicInfoList.Count;
            //if (houseBasicInfoList != null && houseBasicInfoList.Count>0&&parames.sort >= 1)
            // {
            //     switch (parames.sort)
            //     {
            //         case 1:
            //             houseBasicInfoList = houseBasicInfoList.OrderByDescending(s => s.PushTime).ToList();//推送时间排序
            //             break;
            //         case 2:
            //             houseBasicInfoList = houseBasicInfoList.OrderByDescending(s => s.Price).ToList();//价格从高到低排序
            //             break;
            //         case 3:
            //             houseBasicInfoList = houseBasicInfoList.OrderBy(s => s.Price).ToList();//价格从低到高排序
            //             break;
            //         case 4:
            //             houseBasicInfoList = houseBasicInfoList.OrderByDescending(s => s.BuildArea).ToList();//建筑面积从高到底
            //             break;
            //         case 5:
            //             houseBasicInfoList = houseBasicInfoList.OrderBy(s => s.BuildArea).ToList();//建筑面积从低到高
            //             break;
            //         case 6:
            //             houseBasicInfoList = houseBasicInfoList.OrderBy(s => s.Tag).ToList();//标签排序
            //             break;
            //         default:
            //             houseBasicInfoList = houseBasicInfoList.OrderByDescending(s => s.PostTime).ToList();//更新时间排序
            //             break;
            //     }
            // }
            // return EFToModelList(houseBasicInfoList);
            #endregion
            DbCommand cmd = GetStoredProcCommand("P_Api_GetUserHouseList");
            AddInParameter(cmd, "@TradeType", DbType.Int32, parames.postType);
            AddInParameter(cmd, "@buildingType", DbType.Int32, parames.buildingType);
            AddInParameter(cmd, "@buildingStatus", DbType.Int32, parames.buildingStatus);
            AddInParameter(cmd, "@cell", DbType.String, parames.cell);
            AddInParameter(cmd, "@sort", DbType.Int32, parames.sort);
            AddInParameter(cmd, "@houseId", DbType.Int32, parames.houseId);
            AddInParameter(cmd, "@title", DbType.String, parames.title);
            AddInParameter(cmd, "@tags", DbType.String, parames.tags);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, parames.page);
            AddInParameter(cmd, "@pageSize", DbType.Int32, parames.pageSize);
            AddInParameter(cmd, "@userId", DbType.Int32, parames.userId);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidModelList(ds.Tables[0].Select());
            }
            return new List<HouseBasicInfoModel>();
        }
        public virtual string SetInterview(int houseid, int userid, int tpye, string tel)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_Set_HouseInterview");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@type", DbType.Int32, tpye);
            AddInParameter(cmd, "@tel", DbType.String, tel);
            AddOutParameter(cmd, "@msg", DbType.Int32, 500);
            int isuccess = ExecuteNonQuery(cmd);
            var msg = cmd.Parameters["@msg"].Value.ToString();
            return msg;
        }
        public virtual int SetHouseImages(int houseid, string imgurls, int imgtype, int communityid, int userid)
        {
            DbCommand cmd = GetStoredProcCommand("P_Add_Images");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@imgtype", DbType.Int32, imgtype);
            AddInParameter(cmd, "@communityid", DbType.Int32, communityid);
            AddInParameter(cmd, "@imgurls", DbType.String, imgurls);
            return ExecuteNonQuery(cmd);

        }
        public virtual int AddAccusationLog(int aid = 0, int userid = 0, int houseid = 0, int type = 1, string contents = "", string tel = "")
        {
            DbCommand cmd = GetSqlStringCommand(@" INSERT INTO dbo.AccusationLog( A_ID ,UserID ,HouseID ,Type , Contents ,AddDate,tel)
                                                   VALUES  ( @aid ,@userid  , @houseid , @type ,@contents , GETDATE() ,@tel )");

            AddInParameter(cmd, "@aid", DbType.Int32, aid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@type", DbType.Int32, type);
            AddInParameter(cmd, "@contents", DbType.String, contents);
            AddInParameter(cmd, "@tel", DbType.String, tel);
            return ExecuteNonQuery(cmd);
        }

        public virtual int SetHouseHits(int houseid)
        {
            DbCommand cmd = GetSqlStringCommand(@"UPDATE HouseBasicInfo SET Hits=ISNULL(Hits,0)+1 WHERE HouseID=@houseid");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            return ExecuteNonQuery(cmd);

        }

        public virtual DataSet GetInterview(int houseid)
        {
            DbCommand cmd = GetSqlStringCommand(@"SELECT b.UserID,c.Name,c.NickName,b.CommunityName,b.Title,b.Address,c.Tel ,a.HouseID,a.AddTime as AddDate
                               ,b.Price,b.UnitPrice,b.Room,b.Hall,b.Toilet,b.BuildArea,b.PriceUnit  FROM dbo.HouseInterview AS a WITH(NOLOCK)
                               INNER JOIN dbo.HouseBasicInfo AS b WITH(NOLOCK)  ON b.HouseID = a.HouseID
                               INNER JOIN dbo.PublicUser AS c WITH(NOLOCK) ON c.UserID = a.UserID where a.houseid=@houseid");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }


        public virtual DataSet GetConsult(int houseid)
        {
            DbCommand cmd = GetSqlStringCommand(@"  SELECT b.UserID,a.HouseID,c.Name,c.NickName,b.CommunityName,b.Title,b.Address,c.Tel ,a.AddTime as AddDate
                               ,b.Price,a.Price AS cprice,b.UnitPrice,b.Room,b.Hall,b.Toilet,b.BuildArea,b.PriceUnit 
							    FROM dbo.HouseConsult AS a WITH(NOLOCK)
                               INNER JOIN dbo.HouseBasicInfo AS b WITH(NOLOCK)  ON b.HouseID = a.HouseID
                               INNER JOIN dbo.PublicUser AS c WITH(NOLOCK) ON c.UserID = a.UserID where a.houseid=@houseid");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }
        public virtual DataSet GetConsultInterview(int houseid)
        {
            DbCommand cmd = GetSqlStringCommand(@" SELECT  b.UserID,a.HouseID,c.Name,c.NickName,b.CommunityName,b.Title,b.Address,c.Tel ,a.AddTime as AddDate
,b.Price,a.Price AS cprice,b.UnitPrice,b.Room,b.Hall,b.Toilet,b.BuildArea,b.PriceUnit  FROM (						
SELECT HouseID,UserID,Price,AddTime,Itype FROM dbo.HouseConsult WITH(NOLOCK) WHERE HouseID=@houseid
UNION ALL 
SELECT HouseID,UserID,0 AS Price,AddTime,Tpye FROM dbo.HouseInterview  WITH(NOLOCK)WHERE HouseID=@houseid) AS a 
INNER JOIN dbo.HouseBasicInfo AS b WITH(NOLOCK)  ON b.HouseID = a.HouseID
INNER JOIN dbo.PublicUser AS c WITH(NOLOCK) ON c.UserID = a.UserID where a.houseid=@houseid
");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            DataSet ds = ExecuteDataSet(cmd);
            return ds;
        }
        /// <summary>
        /// 修改收藏信息
        /// </summary>
        /// <param name="houseid"></param>
        /// <param name="userid"></param>
        /// <param name="type">1 为添加收藏，2.删除收藏</param>
        /// <returns></returns>
        public virtual int SetCollect(int houseid, int userid, int type)
        {
            string sql = "";
            if (type == 1)
            {
                sql = "  INSERT INTO dbo.HouseCollect(HouseID, UserID)VALUES(@houseid,@userid) ";
            }
            else
            {
                sql = " DELETE dbo.Housecollect WHERE UserId = @userid AND houseid = @houseid ";
            }
            DbCommand cmd = GetSqlStringCommand(sql);
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            int success = ExecuteNonQuery(cmd);
            return success;
        }




        /// <summary>
        /// 添加谈价磋商
        /// </summary>
        /// <param name="houseid"></param>
        /// <param name="userid"></param>
        /// <param name="tpye">0:申请，1：通知业主，2：磋商成功</param>
        /// <param name="tel"></param>
        /// <param name="price">价钱</param>
        /// <returns></returns>
        public virtual string AddConsult(int houseid, int userid, int tpye, string tel, double price)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_Set_AddConsult");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@type", DbType.Int32, tpye);
            AddInParameter(cmd, "@tel", DbType.String, tel);
            AddInParameter(cmd, "@price", DbType.String, price);
            AddOutParameter(cmd, "@msg", DbType.Int32, 500);
            int isuccess = ExecuteNonQuery(cmd);
            var msg = cmd.Parameters["@msg"].Value.ToString();
            return msg;
        }

        public virtual int AddBrowse(int houseid, int userid, int tpye)
        {
            DbCommand cmd = GetStoredProcCommand("P_Add_Browse");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@type", DbType.Int32, tpye);
            return ExecuteNonQuery(cmd);
        }
        public virtual List<PublicUserModel> GetBrowse(int houseid, int pageIndex, int pageSize, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Get_Browse");
            AddInParameter(cmd, "@houseid", DbType.Int32, houseid);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, pageIndex);
            AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from item in ds.Tables[0].Select()
                        select new PublicUserModel()
                        {
                            UserID = ds.Tables[0].Columns.Contains("UserID") ? To<int>(item, "UserID") : 0,
                            NickName = ds.Tables[0].Columns.Contains("NickName") ? To<string>(item, "NickName") : "",
                            Portrait = ds.Tables[0].Columns.Contains("Portrait") ? To<string>(item, "Portrait") : "",
                        }).ToList(); ;
            }
            return null;
        }

        public virtual List<HouseBasicInfoModel> GetEsfHouseList(HouseListReq parames, ref int totalSize)
        {
            string spName = "P_Api_GetEsfHouseList";
            if (parames.IsBrowse > 0)
            {
                spName = "P_Api_GetEsfBrowseHouseList"; //浏览过的
            }

            DbCommand cmd = GetStoredProcCommand(spName);

            AddInParameter(cmd, "@tradetype", DbType.Int32, parames.postType);
            AddInParameter(cmd, "@buildtype", DbType.Int32, parames.buildingType);
            AddInParameter(cmd, "@status", DbType.Int32, parames.buildingStatus);
            AddInParameter(cmd, "@cell", DbType.String, parames.cell);
            AddInParameter(cmd, "@sort", DbType.Int32, parames.sort);

            AddInParameter(cmd, "@room", DbType.Int32, parames.roomType);
            AddInParameter(cmd, "@districtid", DbType.Int32, parames.districtId);
            AddInParameter(cmd, "@regionid", DbType.Int32, parames.regionId);

            AddInParameter(cmd, "@maxprice", DbType.Int32, parames.maxPrice);
            AddInParameter(cmd, "@minprice", DbType.Int32, parames.minPrice);

            AddInParameter(cmd, "@houseId", DbType.Int32, parames.houseId);
            AddInParameter(cmd, "@title", DbType.String, parames.title);
            AddInParameter(cmd, "@tags", DbType.String, parames.tags);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, parames.page);
            AddInParameter(cmd, "@pageSize", DbType.Int32, parames.pageSize);
            AddInParameter(cmd, "@userId", DbType.Int32, parames.userId);

            AddInParameter(cmd, "@scuserid", DbType.Int32, parames.collectuserid);
            AddInParameter(cmd, "@line", DbType.Int32, parames.line);
            AddInParameter(cmd, "@metroid", DbType.Int32, parames.metroid);
            AddInParameter(cmd, "@ismetro", DbType.Int32, parames.ismetro);
            AddInParameter(cmd, "@iscollect", DbType.Int32, parames.iscollect);

            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidModelList(ds.Tables[0].Select());
            }
            return new List<HouseBasicInfoModel>();
        }



        #region 绑定实体
        private List<HouseBasicInfoModel> BulidModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BulidModel(row)).ToList();
        }
        private HouseBasicInfoModel BulidModel(DataRow dr)
        {
            return new HouseBasicInfoModel()
            {
                AddDate = To<DateTime>(dr, "AddDate"),
                Address = To<string>(dr, "Address"),
                Balcony = To<Int32>(dr, "Balcony"),
                BuildArea = To<decimal>(dr, "BuildArea"),
                BuildType = To<Int32>(dr, "BuildType"),
                CellLabel = To<string>(dr, "CellLabel"),
                CityID = To<Int32>(dr, "CityID"),
                CommunityID = To<Int32>(dr, "CommunityID"),
                CommunityName = To<string>(dr, "CommunityName"),
                CurFloor = To<Int32>(dr, "CurFloor"),
                Distrctid = To<Int32>(dr, "Distrctid"),
                DeleteTime = To<DateTime>(dr, "DeleteTime"),
                ExpireDay = To<DateTime>(dr, "ExpireDay"),
                FitmentStatus = To<string>(dr, "FitmentStatus"),
                Hall = To<Int32>(dr, "Hall"),
                HouseID = To<Int32>(dr, "HouseID"),
                HouseImgPath = To<string>(dr, "HouseImgPath"),
                HouseLabel = To<string>(dr, "HouseLabel"),
                InternalNum = To<string>(dr, "InternalNum"),
                IP = To<string>(dr, "IP"),
                Kitchen = To<Int32>(dr, "Kitchen"),
                LookHouseTime = To<string>(dr, "LookHouseTime"),
                MaxFloor = To<Int32>(dr, "MaxFloor"),
                Note = To<string>(dr, "Note"),
                PayType = To<string>(dr, "PayType"),
                PicNum = To<Int32>(dr, "PicNum"),
                PointTo = To<string>(dr, "PointTo"),
                PostTime = To<DateTime>(dr, "PostTime"),
                Price = To<decimal>(dr, "Price"),
                PriceUnit = To<string>(dr, "PriceUnit"),
                PushTime = To<DateTime>(dr, "PushTime"),
                RegionID = To<Int32>(dr, "RegionID"),
                Room = To<Int32>(dr, "Room"),
                Status = To<Int32>(dr, "Status"),
                Tag = To<string>(dr, "Tag"),
                Title = To<string>(dr, "Title"),
                Toilet = To<Int32>(dr, "Toilet"),
                TradeType = To<Int32>(dr, "TradeType"),
                UnitPrice = To<decimal>(dr, "UnitPrice"),
                UsedArea = To<decimal>(dr, "UsedArea"),
                UsedYear = To<Int32>(dr, "UsedYear"),
                UserID = To<Int32>(dr, "UserID"),
                YiJuHua = To<string>(dr, "YiJuHua"),
                LowPay = To<decimal>(dr, "LowPay"),
                PostSites = dr.Table.Columns.Contains("PostSites") ? To<string>(dr, "PostSites") : "",
                OrderSites = dr.Table.Columns.Contains("OrderSites") ? To<string>(dr, "OrderSites") : "",
                OrderStatus = dr.Table.Columns.Contains("OrderStatus") ? To<int>(dr, "OrderStatus") : 0,
                WebCount = dr.Table.Columns.Contains("WebCount") ? To<int>(dr, "WebCount") : 0,
                ShareTel = dr.Table.Columns.Contains("ShareTel") ? To<string>(dr, "ShareTel") : "",
                ShareName = dr.Table.Columns.Contains("ShareName") ? To<string>(dr, "ShareName") : "",
                ShareCompanyId = dr.Table.Columns.Contains("ShareCompanyId") ? To<Int32>(dr, "ShareCompanyId") : 0,
                ShareCompanyStoreId = dr.Table.Columns.Contains("ShareCompanyStoreId") ? To<Int32>(dr, "ShareCompanyStoreId") : 0,
                ShareCompanyName = dr.Table.Columns.Contains("ShareCompanyName") ? To<string>(dr, "ShareCompanyName") : "",
                ShareCompanyStoreName = dr.Table.Columns.Contains("ShareCompanyStoreName") ? To<string>(dr, "ShareCompanyStoreName") : "",
                ShareExpireDay = dr.Table.Columns.Contains("ShareExpireDay") ? To<DateTime>(dr, "ShareExpireDay") : DateTime.Now,
                ShareCount = dr.Table.Columns.Contains("ShareCount") ? To<Int32>(dr, "ShareCount") : 0,
                ShareIsClone = dr.Table.Columns.Contains("ShareIsClone") ? To<Int32>(dr, "ShareIsClone") : 0,
                ShareUserId = dr.Table.Columns.Contains("ShareUserId") ? To<Int32>(dr, "ShareUserId") : 0,
                BeColneHouseID = dr.Table.Columns.Contains("BeColneHouseID") ? To<Int32>(dr, "BeColneHouseID") : 0,
                IsClone = dr.Table.Columns.Contains("IsClone") ? To<Int32>(dr, "IsClone") : 0,
                IsShare = dr.Table.Columns.Contains("IsShare") ? To<Int32>(dr, "IsShare") : 0,
                Hits = dr.Table.Columns.Contains("Hits") ? To<int>(dr, "Hits") : 0,
                StatusName = dr.Table.Columns.Contains("sname") ? To<string>(dr, "sname") : "",
                Lat = dr.Table.Columns.Contains("Lat") ? To<string>(dr, "Lat") : "",
                Lng = dr.Table.Columns.Contains("Lng") ? To<string>(dr, "Lng") : "",
                PeiTao = dr.Table.Columns.Contains("PeiTao") ? To<string>(dr, "PeiTao") : "",
                Traffic = dr.Table.Columns.Contains("Traffic") ? To<string>(dr, "Traffic") : "",
                OpenId = dr.Table.Columns.Contains("OpenID") ? To<string>(dr, "OpenID") : "",
                Collect = dr.Table.Columns.Contains("Collect") ? To<Int32>(dr, "Collect") : 0,
                DistrctName = dr.Table.Columns.Contains("DistrctName") ? To<string>(dr, "DistrctName") : "",
                RegionName = dr.Table.Columns.Contains("RegionName") ? To<string>(dr, "RegionName") : "",
                SiteName = dr.Table.Columns.Contains("SiteName") ? To<string>(dr, "SiteName") : "",
                Line = dr.Table.Columns.Contains("Line") ? To<int>(dr, "Line") : 0,
                Tel = dr.Table.Columns.Contains("Tel") ? To<string>(dr, "Tel") : "",
                Contacts = dr.Table.Columns.Contains("Contacts") ? To<string>(dr, "Contacts") : "",
                ConsultNum = dr.Table.Columns.Contains("ConsultNum") ? To<int>(dr, "ConsultNum") : 0,
                InterviewNum = dr.Table.Columns.Contains("InterviewNum") ? To<int>(dr, "InterviewNum") : 0,
                UserName = dr.Table.Columns.Contains("EnrolnName") ? To<string>(dr, "EnrolnName") : "",
                UserPic = dr.Table.Columns.Contains("Portrait") ? To<string>(dr, "Portrait") : "",
            };
        }
        #endregion

        #endregion

        #region 前台房源
        /// <summary>
        /// 获取房源列表
        /// </summary>
        /// <param name="houseInfoParameter">搜索参数</param>
        /// <returns></returns>
        public virtual List<HouseBasicInfoModel> GetHouseBasicInfoList(HouseParame houseInfoParameter, out int rows)
        {
            rows = 0;
            var dbcommand =
                 GetStoredProcCommand("P_GetHouseList");
            AddInParameter(dbcommand, "@CommunityId", DbType.Int32, houseInfoParameter.CommunityId);
            AddInParameter(dbcommand, "@CityID", DbType.Int32, houseInfoParameter.CityId);
            AddInParameter(dbcommand, "@TradeType", DbType.Int32, houseInfoParameter.TradeType);
            AddInParameter(dbcommand, "@DistrctId", DbType.String, houseInfoParameter.Distrctid);
            AddInParameter(dbcommand, "@keyWord", DbType.String, houseInfoParameter.Title);
            AddInParameter(dbcommand, "@regionId", DbType.String, houseInfoParameter.RegionId);
            AddInParameter(dbcommand, "@MinPrice", DbType.Decimal, houseInfoParameter.MinPrice);
            AddInParameter(dbcommand, "@MaxPrice", DbType.Decimal, houseInfoParameter.MaxPrice);
            AddInParameter(dbcommand, "@MinArea", DbType.Int32, houseInfoParameter.MinArea);
            AddInParameter(dbcommand, "@MaxArea", DbType.Int32, houseInfoParameter.MaxArea);
            AddInParameter(dbcommand, "@Room", DbType.Int32, houseInfoParameter.Room);
            AddInParameter(dbcommand, "@PointTo", DbType.String, houseInfoParameter.PointTo);
            AddInParameter(dbcommand, "@UsedYear", DbType.Int32, houseInfoParameter.UsedYear);
            AddInParameter(dbcommand, "@CurFloor", DbType.Int32, houseInfoParameter.CurFloor);
            AddInParameter(dbcommand, "@tag", DbType.String, houseInfoParameter.Tag);
            AddInParameter(dbcommand, "@iOrderBy", DbType.Int32, houseInfoParameter.HouseOrder);
            AddInParameter(dbcommand, "@UserId", DbType.Int32, houseInfoParameter.UserId);
            AddInParameter(dbcommand, "@pageIndex", DbType.Int32, houseInfoParameter.PageIndex);
            AddInParameter(dbcommand, "@pageSize", DbType.Int32, houseInfoParameter.PageSize);
            AddInParameter(dbcommand, "@BuildType", DbType.Int32, houseInfoParameter.BuildType);
            AddOutParameter(dbcommand, "@total", DbType.Int32, rows);
            var dataSet = ExecuteDataSet(dbcommand);
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return new List<HouseBasicInfoModel>();
            else
            {
                rows = GetOutputParameter<int>(dbcommand, "@total");
                return BuildHouseBasicInfoList(dataSet.Tables[0]);
            }

        }

        private List<HouseBasicInfoModel> BuildHouseBasicInfoList(DataTable dataTable)
        {
            var houseBasicInfoModelList = new List<HouseBasicInfoModel>();
            if (dataTable != null)
            {
                houseBasicInfoModelList.AddRange(from DataRow row in dataTable.Rows
                                                 select new HouseBasicInfoModel
                                                 {
                                                     HouseID = row.Table.Columns.Contains("HouseID") ? To<int>(row, "HouseID") : 0,
                                                     UserID = row.Table.Columns.Contains("UserID") ? To<int>(row, "UserID") : 0,
                                                     TradeType = row.Table.Columns.Contains("TradeType") ? To<int>(row, "TradeType") : 0,
                                                     CityID = row.Table.Columns.Contains("CityID") ? To<int>(row, "CityID") : 0,
                                                     Distrctid = row.Table.Columns.Contains("Distrctid") ? To<int>(row, "Distrctid") : 0,
                                                     RegionID = row.Table.Columns.Contains("RegionID") ? To<int>(row, "RegionID") : 0,
                                                     CommunityID = row.Table.Columns.Contains("CommunityID") ? To<int>(row, "CommunityID") : 0,
                                                     CommunityName = row.Table.Columns.Contains("CommunityName") ? To<string>(row, "CommunityName") : "",
                                                     BuildType = row.Table.Columns.Contains("BuildType") ? To<int>(row, "BuildType") : 0,
                                                     BuildArea = row.Table.Columns.Contains("BuildArea") ? To<int>(row, "BuildArea") : 0,
                                                     UsedArea = row.Table.Columns.Contains("UsedArea") ? To<int>(row, "UsedArea") : 0,
                                                     PointTo = row.Table.Columns.Contains("PointTo") ? To<string>(row, "PointTo") : "",
                                                     UnitPrice = row.Table.Columns.Contains("UnitPrice") ? To<decimal>(row, "UnitPrice") : 0,
                                                     Price = row.Table.Columns.Contains("Price") ? To<decimal>(row, "Price") : 0,
                                                     PriceUnit = row.Table.Columns.Contains("PriceUnit") ? To<string>(row, "PriceUnit") : "",
                                                     CurFloor = row.Table.Columns.Contains("CurFloor") ? To<int>(row, "CurFloor") : 0,
                                                     MaxFloor = row.Table.Columns.Contains("MaxFloor") ? To<int>(row, "MaxFloor") : 0,
                                                     UsedYear = row.Table.Columns.Contains("UsedYear") ? To<int>(row, "UsedYear") : 0,
                                                     Title = row.Table.Columns.Contains("Title") ? To<string>(row, "Title") : "",
                                                     Status = row.Table.Columns.Contains("Status") ? To<int>(row, "Status") : 0,
                                                     IP = row.Table.Columns.Contains("IP") ? To<string>(row, "IP") : "",
                                                     AddDate = row.Table.Columns.Contains("AddDate") ? To<DateTime>(row, "AddDate") : DateTime.Now,
                                                     PostTime = row.Table.Columns.Contains("PostTime") ? To<DateTime>(row, "PostTime") : DateTime.Now,
                                                     Address = row.Table.Columns.Contains("Address") ? To<string>(row, "Address") : "",
                                                     HouseLabel = row.Table.Columns.Contains("HouseLabel") ? To<string>(row, "HouseLabel") : "",
                                                     //LookHouseTime = row.Table.Columns.Contains("LookHouseTime") ? To<DateTime>(row, "LookHouseTime") : DateTime.Now,
                                                     Tag = row.Table.Columns.Contains("Tag") ? To<string>(row, "Tag") : "",
                                                     //InternalNum = row.Table.Columns.Contains("InternalNum") ? To<int>(row, "InternalNum") : 0,
                                                     CellLabel = row.Table.Columns.Contains("CellLabel") ? To<string>(row, "CellLabel") : "",
                                                     YiJuHua = row.Table.Columns.Contains("YiJuHua") ? To<string>(row, "YiJuHua") : "",
                                                     Room = row.Table.Columns.Contains("Room") ? To<int>(row, "Room") : 0,
                                                     Hall = row.Table.Columns.Contains("Hall") ? To<int>(row, "Hall") : 0,
                                                     Kitchen = row.Table.Columns.Contains("Kitchen") ? To<int>(row, "Kitchen") : 0,
                                                     Toilet = row.Table.Columns.Contains("Toilet") ? To<int>(row, "Toilet") : 0,
                                                     Balcony = row.Table.Columns.Contains("Balcony") ? To<int>(row, "Balcony") : 0,
                                                     HouseImgPath = row.Table.Columns.Contains("HouseImgPath") ? To<string>(row, "HouseImgPath") : "http://img.ZJB.com/",
                                                     LowPay = row.Table.Columns.Contains("LowPay") ? To<int>(row, "LowPay") : 0,
                                                     PushTime = row.Table.Columns.Contains("PushTime") ? To<DateTime>(row, "PushTime") : DateTime.Now,
                                                     DeleteTime = row.Table.Columns.Contains("DeleteTime") ? To<DateTime>(row, "DeleteTime") : DateTime.Now,
                                                     PicNum = row.Table.Columns.Contains("PicNum") ? To<int>(row, "PicNum") : 0,
                                                     Hits = row.Table.Columns.Contains("Hits") ? To<int>(row, "Hits") : 0,
                                                     UserName = row.Table.Columns.Contains("EnrolnName") ? To<string>(row, "EnrolnName") : "",
                                                 });
            }

            return houseBasicInfoModelList;
        }



        #endregion

        #region 批量更改房源状态
        //public virtual int ChangeHouseStatus(ChangeHouseStatusReq parames)
        //{
        //    List<HouseBasicInfo> houseList = ncBase.CurrentEntities.HouseBasicInfo.Where(myHouseList => myHouseList.UserID == parames.UserId).Join
        //         (
        //         parames.HouseIds,
        //         dbHouseList => dbHouseList.HouseID,//数据库的房源
        //         targetHouseList => targetHouseList.HouseID,//要修改的房源
        //         (dbHouseList, targetHouseList) => new { dbHouseList, targetHouseList }
        //         ).Select(o => o.dbHouseList)
        //         .ToList();


        //    houseList.ForEach(e =>
        //    {
        //        e.Status = parames.ChangeToStatus;
        //        e.DeleteTime = (parames.ChangeToStatus == 3 ? DateTime.Now : e.DeleteTime);
        //    });
        //    return ncBase.CurrentEntities.SaveChanges(); //保存完成修改
        //}

        public virtual int ChangeHouseStatus(ChangeHouseStatusReq parames)
        {
            int rowscount = 0;
            DbCommand cmd = GetStoredProcCommand("P_Api_ChangeHouseStatus");
            AddInParameter(cmd, "@houseIds", DbType.String, parames.HouseIdsStr);
            AddInParameter(cmd, "@userid", DbType.Int32, parames.UserId);
            AddInParameter(cmd, "@changeStauts", DbType.Int32, parames.ChangeToStatus);
            AddOutParameter(cmd, "@rowcount", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            int.TryParse(cmd.Parameters["@rowcount"].Value.ToString(), out rowscount);

            return rowscount;
        }
        #endregion

        #region 获取用户房源的所有小区名
        public virtual List<CommunityModel> GetUserHouseCommunityList(GetUserHouseCommunityListReq parames)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetUserHouseCommunityList");
            AddInParameter(cmd, "@TradeType", DbType.Int32, parames.PostType);
            AddInParameter(cmd, "@BudlingType", DbType.Int32, parames.BudlingType);
            AddInParameter(cmd, "@BudlingStatus", DbType.Int32, parames.BudlingStatus);
            AddInParameter(cmd, "@UserId", DbType.Int32, parames.UserId);
            DataSet ds = ExecuteDataSet(cmd);
            List<CommunityModel> communityList = new List<CommunityModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    communityList.Add(new CommunityModel()
                    {
                        CommunityID = To<Int32>(dr, "CommunityID"),
                        Name = To<string>(dr, "CommunityName")
                    });
                }
            }
            return communityList;
        }

        #endregion

        #region 房源数据总数统计
        public virtual List<HouseNumSumModel> GetHouseNumSumData(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_HouseDataSum");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<HouseNumSumModel> sumList = new List<HouseNumSumModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sumList.Add(new HouseNumSumModel()
                    {
                        TradeType = To<Int32>(dr, "TradeType"),
                        BuildType = To<Int32>(dr, "BuildType"),
                        Status = To<Int32>(dr, "Status"),
                        TotalSize = To<Int32>(dr, "TotalSize")
                    });
                }
            }
            return sumList;
        }
        #endregion

        #region 更改房源标签
        public virtual int UpdateHouseTags(int userId, string HouseIds, string tags)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_UpdateHouseTags");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@houseIds", DbType.String, HouseIds);
            AddInParameter(cmd, "@tags", DbType.String, tags);
            return ExecuteNonQuery(cmd);
        }
        #endregion

        #region 删除房源图片
        public virtual int DelHouseImageByHouseID(int houseId, int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_DelHouseImageByHouseID");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@houseId", DbType.Int32, houseId);
            return ExecuteNonQuery(cmd);
        }
        #endregion

        #region 管理后台 每日房源新增统计

        public virtual List<StatModel> GetHouseAddStat(StatReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetHouseAddStat");
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@currentTime", DbType.Date, parame.currentTime);
            DataSet ds = ExecuteDataSet(cmd);
            List<StatModel> statList = new List<StatModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statList.Add(new StatModel()
                    {
                        Count = dr.Table.Columns.Contains("Counts") ? To<Int32>(dr, "Counts") : 0,
                        Time = dr.Table.Columns.Contains("Times") ? To<DateTime>(dr, "Times") : DateTime.Now
                    });
                }
            }
            return statList;
        }
        #endregion

        #region 获取采集房源列表页面
        public virtual MongoCursor<HouseCrawler> GetHouseCollectList(HouseListReq parames, ref int totalSize)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient("MongoDbHouseCaijiConnection");
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase("HeZi");
            int cityId = parames.cityId == 0 ? 592 : parames.cityId;
            string dbCollection = "Chushou_" + cityId;
            switch (parames.postType)
            {
                case 1:
                    dbCollection = "Chushou_" + cityId;
                    break;
                case 2:
                    dbCollection = "Qiugou_" + cityId;
                    break;
                case 3:
                    dbCollection = "Chuzu_" + cityId;
                    break;
                case 4:
                    dbCollection = "Qiuzu_" + cityId;
                    break;
                default:
                    dbCollection = "Chushou_" + cityId;
                    break;
            }
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            IMongoQuery query = new QueryDocument();
            if (parames.buildingType > 0) { query = Query.And(query, Query.EQ("BuildingType", parames.buildingType)); }
            if (parames.districtId > 0) { query = Query.And(query, Query.EQ("Distrctid", parames.districtId)); }
            if (parames.regionId > 0) { query = Query.And(query, Query.EQ("RegionID", parames.regionId)); }
            if (parames.roomType > 0) { query = Query.And(query, Query.EQ("Room", parames.roomType)); }
            if (parames.minPrice > 0) { query = Query.And(query, Query.GTE("Price", parames.minPrice)); }
            if (parames.maxPrice > 0) { query = Query.And(query, Query.LTE("Price", parames.maxPrice)); }
            if (!string.IsNullOrEmpty(parames.webName)) { query = Query.And(query, Query.EQ("Source", parames.webName)); }
            if (!string.IsNullOrEmpty(parames.title)) { query = Query.And(query, Query.Or(Query.Matches("CommunityName", parames.title), Query.Matches("RegionName", parames.title), Query.Matches("Address", parames.title), Query.Matches("Title", parames.title))); }
            IMongoSortBy sort = new SortByDocument();
            switch (parames.sort)
            {
                case 0:
                    sort = SortBy.Descending("ReleaseTime");
                    break;
                case 1:
                    sort = SortBy.Descending("UpdateTime");
                    break;
                default:
                    sort = SortBy.Descending("ReleaseTime");
                    break;
            }
            MongoCursor<HouseCrawler> dataList = houseCollection.FindAs<HouseCrawler>(query).SetSortOrder(sort).SetSkip((parames.page - 1) * parames.pageSize).SetLimit(parames.pageSize);
            totalSize = (int)dataList.Count(); ;
            return dataList;
        }

        public virtual HouseCrawler GetHouseCollect(string id, string dbCollection)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient("MongoDbHouseCaijiConnection");
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase("HeZi");


            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            IMongoQuery query = new QueryDocument();

            if (!string.IsNullOrEmpty(id)) { query = Query.And(query, Query.EQ("_id", id)); }

            HouseCrawler dataList = houseCollection.FindAs<HouseCrawler>(query).FirstOrDefault();

            return dataList;
        }
        /// <summary>
        /// 采集的房源 的网站来源字典表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual MongoCursor<HouseCollectSource> GetHouseCollectSite(int cityId)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection("Sites");
            IMongoQuery query = new QueryDocument();
            query = Query.And(query, Query.And(Query.Or(Query.EQ("City", cityId), Query.EQ("City", 0))));
            MongoCursor<HouseCollectSource> dataList = houseCollection.FindAs<HouseCollectSource>(query);
            return dataList;
        }
        /// <summary>
        /// 添加 已读
        /// </summary>
        /// <returns></returns>
        public virtual void HouseCollectReadAdd(string id, int userId, string userName)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "HouseViewLog";

            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            var item = houseCollection.FindOneAs<HouseCollectViewLog>(Query.EQ("_id", id));
            List<Agent> agentsList = new List<Agent>();
            agentsList.Add(new Agent()
            {
                Id = userId,
                Name = userName
            });
            if (item == null)
            {
                HouseCollectViewLog itemLog = new HouseCollectViewLog()
                {
                    id = id,
                    Agents = agentsList,
                    AddTime = ZJB.Core.Utilities.DateTimeUtility.ToUnixTime(DateTime.Now).ToString()
                };
                houseCollection.Insert(itemLog);
            }
            else
            {
                if (!(item.Agents != null && item.Agents.Count > 0 && item.Agents.Where(a => a.Id == userId).FirstOrDefault() != null))//不存在阅读记录
                {
                    UpdateBuilder update = Update.Push("Agents", BsonValue.Create(new Dictionary<string, object> { { "_id", userId }, { "Name", userName } }));
                    houseCollection.Update(Query.EQ("_id", id), update);
                }
            }
        }
        /// <summary>
        /// 采集房源阅读数
        /// </summary>
        public virtual MongoCursor<HouseCollectViewLog> GetHouseCollectReadLogByIds(List<string> ids)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "HouseViewLog";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            MongoCursor<HouseCollectViewLog> viewList = houseCollection.FindAs<HouseCollectViewLog>(Query.In("_id", BsonArray.Create(ids)));
            return viewList;
        }
        #endregion

        #region 房源提醒关键字设置


        //增加关键词及对应的用户id
        public virtual void AddKeyword(string keyword, int cityId, int userId, int tradeType)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "Keywords";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            var id = keyword + "_" + cityId + "_" + tradeType;
            var query = Query.EQ("_id", id);
            var exists = houseCollection.FindOneAs<HouseCrawlerKeyword>(query);
            if (exists != null)
            {
                houseCollection.Update(query, Update.AddToSet("user_ids", userId));
            }
            else
            {
                var newKeyword = new HouseCrawlerKeyword(id, keyword, tradeType, cityId, new[] { userId });
                houseCollection.Insert(newKeyword);
            }
        }
        //从关键词的user_ids中删除某个userid
        public virtual void RemoveKeyword(string keyword, int cityId, int userId, int tradeType)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "Keywords";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            var id = keyword + "_" + cityId + "_" + tradeType;
            var query = Query.EQ("_id", id);
            var exists = houseCollection.FindOneAs<HouseCrawlerKeyword>(query);
            if (exists != null)
            {
                houseCollection.Update(query, Update.Pull("user_ids", userId));
            }
        }


        #endregion

        #region 房源发布/编辑
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="houseInfoParame"></param>
        /// <param name="villaInfoParame"></param>
        /// <param name="shopInfoParame"></param>
        /// <param name="officeInfoParame"></param>
        /// <param name="factoryInfoParame"></param>
        /// <returns></returns>
        public virtual int OperateHouse(HouseParame basicInfo, HouseInfoParame houseInfoParame, VillaInfoParame villaInfoParame, ShopInfoParame shopInfoParame, OfficeInfoParame officeInfoParame, FactoryInfoParame factoryInfoParame)
        {
            int houseId = basicInfo.HouseId;
            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
            if (houseId > 0)
            {
                houseBasicInfo = ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == houseId && o.UserID == basicInfo.UserId).FirstOrDefault();
                if (houseBasicInfo.IsNull())
                {
                    return 0;
                }
            }
            //房源图片相关
            string imgUrlCover = "";
            int picNum = basicInfo.HouseImgs.Count;
            if (basicInfo.HouseImgs.IsNoNull() && basicInfo.HouseImgs.Count > 0)
            {
                var isCoverImg = basicInfo.HouseImgs.Where(o => o.IsCover == true).FirstOrDefault();
                if (isCoverImg.IsNoNull())
                {
                    imgUrlCover = isCoverImg.imageUrl;
                    isCoverImg.IsCover = true;
                }
                else
                {
                    var firstCoverImg = basicInfo.HouseImgs.FirstOrDefault();
                    imgUrlCover = firstCoverImg.imageUrl;
                    firstCoverImg.IsCover = true;
                }

            }

            //价格相关
            decimal lowpay = 0; //最低首付
            string priceUnit;  //价格单位
            decimal unitPrice = 0;

            switch (basicInfo.TradeType)
            {
                case (int)TradeType.Sell:
                    priceUnit = "万";
                    unitPrice = basicInfo.BuildArea > 0 ? basicInfo.Price * 10000 / basicInfo.BuildArea : 0;  //出售计算单价
                    lowpay = basicInfo.Price * 10000 * Convert.ToDecimal(0.30);  //最低首付计算
                    break;
                case (int)TradeType.Rent:
                    switch (basicInfo.BuildType)
                    {
                        case (int)BuildingType.House:
                        case (int)BuildingType.Villa:
                            priceUnit = "元/月"; break;
                        default:
                            priceUnit = basicInfo.PriceUnit; break;
                    }
                    break;

                default: priceUnit = basicInfo.PriceUnit; break;
            }
            Regex replare = new Regex("\\&[lr]dquo;");
            houseBasicInfo.UserID = basicInfo.UserId;  //用户ID
            houseBasicInfo.TradeType = BitConverter.GetBytes(basicInfo.TradeType)[0];
            houseBasicInfo.CityID = basicInfo.CityId;
            houseBasicInfo.Distrctid = basicInfo.Distrctid;
            houseBasicInfo.RegionID = basicInfo.RegionId;
            houseBasicInfo.CommunityID = basicInfo.CommunityId;
            houseBasicInfo.CommunityName = basicInfo.CommunityName;
            houseBasicInfo.BuildType = BitConverter.GetBytes(basicInfo.BuildType)[0];
            houseBasicInfo.BuildArea = basicInfo.BuildArea;
            houseBasicInfo.UsedArea = basicInfo.UsedArea; ;
            houseBasicInfo.PointTo = basicInfo.PointTo;
            houseBasicInfo.Price = basicInfo.Price;
            houseBasicInfo.UnitPrice = unitPrice;
            houseBasicInfo.PriceUnit = priceUnit;
            houseBasicInfo.LowPay = lowpay;
            houseBasicInfo.CurFloor = Convert.ToInt16(basicInfo.CurFloor);
            houseBasicInfo.MaxFloor = Convert.ToInt16(basicInfo.MaxFloor); ;
            houseBasicInfo.UsedYear = basicInfo.UsedYear;
            houseBasicInfo.ExpireDay = DateTime.Now.AddDays(30);
            houseBasicInfo.FitmentStatus = basicInfo.FitmentStatus;

            houseBasicInfo.PicNum = BitConverter.GetBytes(picNum)[0];
            houseBasicInfo.Title = basicInfo.Title;
            houseBasicInfo.Note = replare.Replace(basicInfo.Note, "\"");
            houseBasicInfo.Status = BitConverter.GetBytes((int)HouseStatus.Release)[0];
            houseBasicInfo.IP = IpUtility.GetIp();
            houseBasicInfo.PostTime = DateTime.Now;
            houseBasicInfo.Address = basicInfo.Address;
            houseBasicInfo.LookHouseTime = basicInfo.LookHouseTime;
            houseBasicInfo.HouseLabel = basicInfo.HouseLabel;
            houseBasicInfo.InternalNum = basicInfo.InternalNum;
            houseBasicInfo.CellLabel = basicInfo.CellLabel;
            houseBasicInfo.YiJuHua = basicInfo.YiJuHua;
            houseBasicInfo.Room = BitConverter.GetBytes(basicInfo.Room)[0];
            houseBasicInfo.Hall = BitConverter.GetBytes(basicInfo.Hall)[0];
            houseBasicInfo.Kitchen = BitConverter.GetBytes(basicInfo.Kitchen)[0];
            houseBasicInfo.Toilet = BitConverter.GetBytes(basicInfo.Toilet)[0];
            houseBasicInfo.Balcony = BitConverter.GetBytes(basicInfo.Balcony)[0];
            houseBasicInfo.PayType = basicInfo.PayType ?? "";

            houseBasicInfo.HouseImgPath = imgUrlCover;

            houseBasicInfo.Source = basicInfo.Source;

            if (houseId.Equals(0)) //添加操作时候
            {
                houseBasicInfo.IsClone = basicInfo.IsClone.IsNull() ? false : basicInfo.IsClone;
                houseBasicInfo.BeColneHouseID = basicInfo.BeColneHouseId ?? 0; //被克隆房源ID
                houseBasicInfo.BeColneID = basicInfo.BeColneID;
                houseBasicInfo.Tag = "";
                houseBasicInfo.AddDate = DateTime.Now;
                houseBasicInfo.PushTime = Convert.ToDateTime("1900-1-1");
                houseBasicInfo.DeleteTime = Convert.ToDateTime("1900-1-1");
                ncBase.CurrentEntities.AddToHouseBasicInfo(houseBasicInfo);
                ncBase.CurrentEntities.SaveChanges();
            }
            houseId = houseBasicInfo.HouseID;

            if (houseId > 0)
            {
                if (basicInfo.HouseImgs.IsNoNull() && basicInfo.HouseImgs.Count > 0)
                {
                    HouseBll houseBll = new HouseBll();
                    houseBll.DelHouseImageByHouseID(houseId, basicInfo.UserId);

                    int i = 1;
                    foreach (HouseImgParame item in basicInfo.HouseImgs)
                    {
                        HouseImage houseImage = new HouseImage();
                        houseImage.HouseID = houseId;
                        houseImage.ImagePath = item.imageUrl;
                        houseImage.ImagePos = item.imgDescribe;
                        houseImage.ImageType = item.imageType;
                        houseImage.IsCover = item.IsCover;
                        houseImage.OrderID = i;
                        houseImage.CommunityID = houseBasicInfo.CommunityID;
                        houseImage.UserID = basicInfo.UserId;
                        houseImage.AddTime = DateTime.Now;
                        houseImage.Status = BitConverter.GetBytes(1)[0]; ;
                        ncBase.CurrentEntities.AddToHouseImage(houseImage);
                        ncBase.CurrentEntities.SaveChanges();
                        i++;
                    }
                }

                #region 房屋类型扩展信息
                bool isAdd = false;
                switch (basicInfo.BuildType)
                {
                    case (int)BuildingType.House:

                        #region 住宅信息

                        if (houseInfoParame.IsNull()) break;
                        HouseInfo houseInfo =
                            ncBase.CurrentEntities.HouseInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                        if (houseInfo.IsNull())
                        {
                            houseInfo = new HouseInfo();
                            isAdd = true; //标记为新增加
                        }


                        houseInfo.HouseID = houseId;
                        houseInfo.HouseType = houseInfoParame.HouseType;
                        houseInfo.HouseSubType = houseInfoParame.HouseSubType;
                        houseInfo.HouseProperty = houseInfoParame.HouseProperty;
                        houseInfo.LandYear = houseInfoParame.LandYear;
                        houseInfo.HouseStructure = houseInfoParame.HouseStructure;
                        houseInfo.FiveYears = houseInfoParame.FiveYears;
                        houseInfo.OnlyHouse = houseInfoParame.OnlyHouse;
                        houseInfo.BasicEquip = houseInfoParame.BasicEquipHouse;
                        houseInfo.AdvEquip = houseInfoParame.AdvEquip;

                        if (isAdd)
                        {
                            ncBase.CurrentEntities.AddToHouseInfo(houseInfo);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion

                        break;

                    case (int)BuildingType.Villa:

                        #region 别墅信息

                        if (villaInfoParame.IsNull()) break;
                        VillaInfo villaInfo =
                            ncBase.CurrentEntities.VillaInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                        if (villaInfo.IsNull())
                        {
                            villaInfo = new VillaInfo();
                            isAdd = true; //标记为新增加
                        }

                        villaInfo.HouseID = houseId;
                        villaInfo.VillaType = villaInfoParame.VillaType;
                        villaInfo.HallType = villaInfoParame.HallType;
                        villaInfo.LandYear = villaInfoParame.LandYear;
                        villaInfo.OnlyHouse = villaInfoParame.OnlyHouse;
                        villaInfo.FiveYears = villaInfoParame.FiveYears;
                        villaInfo.Basement = villaInfoParame.Basement;
                        villaInfo.Garden = villaInfoParame.Garden;
                        villaInfo.Garage = villaInfoParame.Garage;
                        villaInfo.ParkLot = villaInfoParame.ParkLot;
                        villaInfo.BasicEquip = villaInfoParame.BasicEquip;
                        villaInfo.AdvEquip = villaInfoParame.AdvEquip;

                        if (isAdd)
                        {
                            ncBase.CurrentEntities.AddToVillaInfo(villaInfo);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion

                        break;

                    case (int)BuildingType.Shop:

                        #region 商铺信息

                        if (shopInfoParame.IsNull()) break;
                        ShopInfo shopInfo =
                            ncBase.CurrentEntities.ShopInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                        if (shopInfo.IsNull())
                        {
                            shopInfo = new ShopInfo();
                            isAdd = true; //标记为新增加
                        }

                        shopInfo.HouseID = houseId;
                        shopInfo.ShopType = shopInfoParame.ShopType;
                        shopInfo.ShopStatus = shopInfoParame.ShopStatus;
                        shopInfo.TargetField = shopInfoParame.TargetField;
                        shopInfo.Fee = shopInfoParame.Fee;
                        shopInfo.Divide = shopInfoParame.Divide;
                        shopInfo.BasicEquip = shopInfoParame.BasicEquip;

                        if (isAdd)
                        {
                            ncBase.CurrentEntities.AddToShopInfo(shopInfo);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion

                        break;

                    case (int)BuildingType.Office:

                        #region 写字楼信息

                        if (officeInfoParame.IsNull()) break;
                        OfficeInfo officeInfo =
                            ncBase.CurrentEntities.OfficeInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                        if (officeInfo.IsNull())
                        {
                            officeInfo = new OfficeInfo();
                            isAdd = true; //标记为新增加
                        }

                        officeInfo.HouseID = houseId;
                        officeInfo.OfficeType = officeInfoParame.OfficeType;
                        officeInfo.OfficeLevel = officeInfoParame.OfficeLevel;
                        officeInfo.BasicEquip = officeInfoParame.BasicEquip;
                        officeInfo.Fee = officeInfoParame.Fee;
                        officeInfo.Divide = officeInfoParame.Divide;

                        if (isAdd)
                        {
                            ncBase.CurrentEntities.AddToOfficeInfo(officeInfo);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion

                        break;
                    case (int)BuildingType.Factory:

                        #region 厂房信息

                        if (factoryInfoParame.IsNull()) break;
                        FactoryInfo factoryInfo =
                            ncBase.CurrentEntities.FactoryInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                        if (factoryInfo.IsNull())
                        {
                            factoryInfo = new FactoryInfo();
                            isAdd = true; //标记为新增加
                        }

                        factoryInfo.HouseID = houseId;
                        factoryInfo.BasicEquip = factoryInfoParame.BasicEquip;
                        factoryInfo.FactoryType = factoryInfoParame.FactoryType;

                        if (isAdd)
                        {
                            ncBase.CurrentEntities.AddToFactoryInfo(factoryInfo);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion

                        break;
                }
                #endregion


            }

            return houseId;
        }
        #endregion

        #region 获取我的收藏
        public List<HouseCollection> GetCollectionList(HouseListReq parame, ref int totalSize)
        {
            System.Linq.Expressions.Expression<Func<HouseCollection, bool>> query = h =>
                      (h.UserId == parame.userId || parame.userId == 0)
                       && h.TradeType == (parame.postType > 0 ? parame.postType : h.TradeType)
                       && h.Distrctid == (parame.districtId > 0 ? parame.districtId : h.Distrctid)
                       && h.RegionID == (parame.regionId > 0 ? parame.regionId : h.RegionID)
                       && h.BuildingType == (parame.buildingType > 0 ? parame.buildingType : h.BuildingType)
                       && h.Room == (parame.roomType > 0 ? parame.roomType : (int)h.Room)
                       && h.Price >= (parame.minPrice > 0 ? (decimal)parame.minPrice : h.Price)
                       && h.Price <= (parame.maxPrice > 0 ? (decimal)parame.maxPrice : h.Price)
                       && h.Source == (string.IsNullOrEmpty(parame.webName) ? h.Source : parame.webName)
                       && h.CityId == (parame.cityId > 0 ? parame.cityId : h.CityId)
                       && (string.IsNullOrEmpty(parame.title) ? true : (h.Title.Contains(parame.title) || h.CommunityName.Contains(parame.title) || h.Address.Contains(parame.title) || h.RegionName.Contains(parame.title)))
                       && h.CollectStatus == 1;
            List<HouseCollection> houseList = ncBase.CurrentEntities.HouseCollection
                .Where(query).OrderByDescending(h => h.AddTime).Skip((parame.page - 1) * parame.pageSize).Take(parame.pageSize)
                .ToList();
            totalSize = ncBase.CurrentEntities.HouseCollection.Where(query).Count();
            return houseList;
        }
        #endregion

        public virtual List<ImpHouseResultModel> ImpHouseBatch(List<HouseParame> basicInfoList, List<HouseInfoParame> houseInfoParameList, List<VillaInfoParame> villaInfoParameList, List<ShopInfoParame> shopInfoParameList, List<OfficeInfoParame> officeInfoParameList, List<FactoryInfoParame> factoryInfoParameList, List<HouseImgParame> houseImgParameList, int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_House_BatchSave");
            string basicInfoListXml = ZJB.Core.Utilities.XmlUtility.Serialize(basicInfoList, Encoding.UTF8, "HouseParameList");
            string houseInfoParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(houseInfoParameList, Encoding.UTF8, "HouseInfoParameList");
            string villaInfoParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(villaInfoParameList, Encoding.UTF8, "VillaInfoParameList");
            string shopInfoParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(shopInfoParameList, Encoding.UTF8, "ShopInfoParameList");
            string officeInfoParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(officeInfoParameList, Encoding.UTF8, "OfficeInfoParameList");
            string factoryInfoParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(factoryInfoParameList, Encoding.UTF8, "FactoryInfoParameList");
            string houseImgParameListXml = ZJB.Core.Utilities.XmlUtility.Serialize(houseImgParameList, Encoding.UTF8, "HouseImgParameList");
            AddInParameter(cmd, "@basicInfoXml", DbType.Xml, basicInfoListXml);
            AddInParameter(cmd, "@houseInfoXml", DbType.Xml, houseInfoParameListXml);
            AddInParameter(cmd, "@villaInfoXml", DbType.Xml, villaInfoParameListXml);
            AddInParameter(cmd, "@shopInfoXml", DbType.Xml, shopInfoParameListXml);
            AddInParameter(cmd, "@officeInfoXml", DbType.Xml, officeInfoParameListXml);
            AddInParameter(cmd, "@factoryInfoXml", DbType.Xml, factoryInfoParameListXml);
            AddInParameter(cmd, "@houseImageXml", DbType.Xml, houseImgParameListXml);
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<ImpHouseResultModel> resultList = new List<ImpHouseResultModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    resultList.Add(new ImpHouseResultModel
                    {
                        BeCloneId = To<string>(dr, "BeColneID"),
                        HouseId = To<Int32>(dr, "HouseID")
                    });
                }
            }
            return resultList;
        }

        #region 获取小区列表
        public virtual List<Community> GetAllHouseCommunityList()
        {
            List<Community> cList = ncBase.CurrentEntities.Community.ToList();
            return cList;
        }
        #endregion

        #region 获取小区历史价格列表
        public virtual List<CommunityHistoryPrice> GetLpHistoryPriceList(int communityID)
        {
            
            var cmd = GetStoredProcCommand("P_Api_GetLpHistoryPriceListById");
            AddInParameter(cmd, "@communityID", DbType.Int32, communityID);
        
            List<CommunityHistoryPrice> result = new List<CommunityHistoryPrice>();
            var ds = ExecuteDataSet(cmd);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CommunityHistoryPrice l = new CommunityHistoryPrice
                {
                    CommunityID = To<int>(dr, "CommunityId"),
                    UpdateTime = To<DateTime>(dr, "UpdateTime"),
                    SellPrice = To<decimal>(dr, "SellPrice"),
                   
                };
                result.Add(l);
            }
            return result;
        }
        #endregion

        #region 小区匹配

        public virtual List<TargetLoupan> GetTargetLoupanList(int index, int size, int siteId, int cityId, string name, out int totalCount)
        {
            var cmd = GetStoredProcCommand("T_GetCommunityForMapping");
            AddInParameter(cmd, "@pageIndex", DbType.Int32, index);
            AddInParameter(cmd, "@pageSize", DbType.Int32, size);
            AddInParameter(cmd, "@SiteId", DbType.Int32, siteId);
            AddInParameter(cmd, "@CityId", DbType.Int32, cityId);
            AddInParameter(cmd, "@Name", DbType.String, name);
            AddOutParameter(cmd, "@TotalCount", DbType.Int32, 0);
            List<TargetLoupan> result = new List<TargetLoupan>();
            var ds = ExecuteDataSet(cmd);
            totalCount = GetOutputParameter<int>(cmd, "@TotalCount");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TargetLoupan l = new TargetLoupan
                {
                    Id = To<string>(dr, "Id"),
                    Name = To<string>(dr, "Name"),
                    RegionName = To<string>(dr, "RegionName"),
                    Address = To<string>(dr, "Address"),
                    DistrictName = To<string>(dr, "DistrictName")
                };
                result.Add(l);
            }
            return result;
        }

        public virtual List<SimilarCommunity> GetCommunityTargetBySite(int siteId)
        {
            var cmd = GetStoredProcCommand("GetCommunityTargetBySite");
            AddInParameter(cmd, "@siteId", DbType.Int32, siteId);
            List<SimilarCommunity> result = new List<SimilarCommunity>();
            var ds = ExecuteDataSet(cmd);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SimilarCommunity l = new SimilarCommunity
                {
                    CommunityID = To<string>(dr, "CommunityID"),
                    Name = To<string>(dr, "Name")
                };
                result.Add(l);
            }
            return result;
        }

        public virtual List<SimilarCommunity> GetSimilarCommunityByIdList(string idList)
        {
            var cmd = GetStoredProcCommand("GetCommunityTargetByIdList");
            AddInParameter(cmd, "@IdList", DbType.String, idList);
            List<SimilarCommunity> result = new List<SimilarCommunity>();
            var ds = ExecuteDataSet(cmd);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SimilarCommunity l = new SimilarCommunity
                {
                    CommunityID = To<string>(dr, "CommunityID"),
                    Name = To<string>(dr, "Name"),
                    DistrictName = To<string>(dr, "DistrctName"),
                    RegionName = To<string>(dr, "RegionName"),
                    Address = To<string>(dr, "Address"),
                };
                result.Add(l);
            }
            return result;
        }
        public virtual void MappingCircle(int siteId, int id, string communityId)
        {
            var cmd = GetStoredProcCommand("MappingCommunityToTargetid");
            AddInParameter(cmd, "@siteId", DbType.String, siteId);
            AddInParameter(cmd, "@Id", DbType.Int32, id);
            AddInParameter(cmd, "@communityId", DbType.String, communityId);
            var ds = ExecuteNonQuery(cmd);
        }

        public virtual void FinishMapping(string id)
        {
            var cmd = GetStoredProcCommand("FinishCommunitySiteMapping");
            AddInParameter(cmd, "@Id", DbType.String, id);
            var ds = ExecuteNonQuery(cmd);
        }

        #endregion

        #region 小区管理
        public virtual List<Community> GetCircleListForManage(string circleName, int index, int size, int status, out int totalCount)
        {
            var cmd = GetStoredProcCommand("T_CommunityGetByPage");
            AddInParameter(cmd, "@CircleName", DbType.String, circleName);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, index);
            AddInParameter(cmd, "@Status", DbType.Int32, status);
            AddInParameter(cmd, "@pageSize", DbType.Int32, size);
            AddOutParameter(cmd, "@TotalCount", DbType.Int32, 0);
            List<Community> result = new List<Community>();
            var ds = ExecuteDataSet(cmd);
            totalCount = GetOutputParameter<int>(cmd, "@TotalCount");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Community l = new Community
                {
                    CommunityID = To<int>(dr, "CommunityId"),
                    Name = To<string>(dr, "Name"),
                    CityName = To<string>(dr, "CityName"),
                    DistrctName = To<string>(dr, "DistrctName"),
                    KaiFaShang = To<string>(dr, "KaiFaShang"),
                    WuyeCompany = To<string>(dr, "WuyeCompany"),
                    Lat = To<string>(dr, "Lat"),
                    Lng = To<string>(dr, "Lng"),
                    PeiTao = To<string>(dr, "PeiTao"),
                    RegionName = To<string>(dr, "RegionName"),
                    Address = To<string>(dr, "Address")
                };
                result.Add(l);
            }
            return result;
        }
        public virtual Community GetCommunityById(int id)
        {
            var cmd = GetStoredProcCommand("T_CommunityGetById");
            AddInParameter(cmd, "@Id", DbType.Int32, id);

            Community result = new Community();
            var ds = ExecuteDataSet(cmd);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return result;
            DataRow dr = ds.Tables[0].Rows[0];
            result = new Community
            {
                CommunityID = To<int>(dr, "CommunityId"),
                Name = To<string>(dr, "Name"),
                CityID = To<int>(dr, "CityID"),
                CityName = To<string>(dr, "CityName"),
                DistrctName = To<string>(dr, "DistrctName"),
                Distrctid = To<int>(dr, "Distrctid"),
                KaiFaShang = To<string>(dr, "KaiFaShang"),
                WuyeCompany = To<string>(dr, "WuyeCompany"),
                Lat = To<string>(dr, "Lat"),
                Lng = To<string>(dr, "Lng"),
                PeiTao = To<string>(dr, "PeiTao"),
                RegionID = To<int>(dr, "RegionID"),
                RegionName = To<string>(dr, "RegionName"),
                Address = To<string>(dr, "Address"),
                Status = (byte?)To<int>(dr, "Status"),
                Traffic = dr.Table.Columns.Contains("Traffic") ? To<string>(dr, "Traffic") : "",
                CoverImg = dr.Table.Columns.Contains("CoverImg") ? To<string>(dr, "CoverImg") : "",
                SellPrice = dr.Table.Columns.Contains("SellPrice") ? To<decimal>(dr, "SellPrice") :0,
                Recommend = dr.Table.Columns.Contains("Recommend") ? To<int>(dr, "Recommend") : 0,
                CompleteDate = dr.Table.Columns.Contains("CompleteDate") ? To<string>(dr, "CompleteDate") : "",

            };
            return result;
        }

        public virtual int UpdateCommunityDetail(Community circle)
        {
            var cmd = GetStoredProcCommand("T_CommunityAddOrUpdate");
            AddInParameter(cmd, "@Id", DbType.Int32, circle.CommunityID);
            AddInParameter(cmd, "@Name", DbType.String, circle.Name);
            AddInParameter(cmd, "@Address", DbType.String, circle.Address);
            AddInParameter(cmd, "@Lng", DbType.Decimal, WebConvert.ToDecial(circle.Lng));
            AddInParameter(cmd, "@Lat", DbType.Decimal, WebConvert.ToDecial(circle.Lat));
            AddInParameter(cmd, "@Kaifashang", DbType.String, circle.KaiFaShang);
            AddInParameter(cmd, "@WuyeCompany", DbType.String, circle.WuyeCompany);
            AddInParameter(cmd, "@Peitao", DbType.String, circle.PeiTao);
            AddInParameter(cmd, "@Type", DbType.Int32, circle.BuildType);
            AddInParameter(cmd, "@RegionId", DbType.String, circle.RegionID);
            AddInParameter(cmd, "@Status", DbType.Int32, circle.Status);

            AddInParameter(cmd, "@Traffic", DbType.String, circle.Traffic);
            AddInParameter(cmd, "@CoverImg", DbType.String, circle.CoverImg);
            AddInParameter(cmd, "@SellPrice", DbType.Decimal, circle.SellPrice);
            AddInParameter(cmd, "@Recommend ", DbType.Int32, circle.Recommend);
            AddInParameter(cmd, "@CompleteDate", DbType.String, circle.CompleteDate);

          AddOutParameter(cmd, "@NewId", DbType.Int32, 8);
            ExecuteNonQuery(cmd);
            return GetOutputParameter<int>(cmd, "@NewId");
        }
        #endregion


        #region 获取共享房源列表
        public virtual List<HouseBasicInfoModel> GetHouseShareList(HouseListReq parames, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetUserHouseShareList");
            AddInParameter(cmd, "@TradeType", DbType.Int32, parames.postType);
            AddInParameter(cmd, "@districtId", DbType.Int32, parames.districtId);
            AddInParameter(cmd, "@companyId", DbType.Int32, parames.shareCompanyId);
            AddInParameter(cmd, "@storeId", DbType.Int32, parames.shareStoreId);
            AddInParameter(cmd, "@cell", DbType.String, parames.cell);
            AddInParameter(cmd, "@sort", DbType.Int32, parames.sort);
            AddInParameter(cmd, "@tags", DbType.String, parames.tags);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, parames.page);
            AddInParameter(cmd, "@pageSize", DbType.Int32, parames.pageSize);
            AddInParameter(cmd, "@tel", DbType.String, parames.tel);
            AddInParameter(cmd, "@userId", DbType.Int32, parames.userId);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidModelList(ds.Tables[0].Select());
            }
            return new List<HouseBasicInfoModel>();
        }
        #endregion


        #region 报备客户
        public virtual int AddBaoBei(int hid = 0, string name = "", int userId = 0,
                                                string tel = "", decimal price = 0, int sex = 1, string appointmentTime = "", string notes = "")
        {
            DbCommand cmd = GetStoredProcCommand("P_House_AddBaoBei");
            AddInParameter(cmd, "@hid", DbType.Int32, hid);
            AddInParameter(cmd, "@name", DbType.String, name);
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@tel", DbType.String, tel);
            AddInParameter(cmd, "@price", DbType.Decimal, price);
            AddInParameter(cmd, "@appointmentTime", DbType.String, appointmentTime);
            AddInParameter(cmd, "@notes", DbType.String, notes);
            AddInParameter(cmd, "@sex", DbType.Int32, sex);
            AddOutParameter(cmd, "@tid", DbType.Int32, 0);
            ExecuteNonQuery(cmd);
            int tid = GetOutputParameter<Int32>(cmd, "@tid");
            return tid;
        }

        #endregion

        #region 客户列表
        public virtual List<HousingSalesModel> GetBaobeiList(int userid, int state, int pageindex, int pagesize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetTuiJianList");
            AddInParameter(cmd, "@state", DbType.Int32, state);
            AddInParameter(cmd, "@pageindex", DbType.Int32, pageindex);
            AddInParameter(cmd, "@pagesize", DbType.String, pagesize);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            var ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildBaoBeiList(ds.Tables[0]);
            }
            return null;
        }
        public virtual List<HousingSalesModel> BuildBaoBeiList(DataTable dt)
        {
            var list = new List<HousingSalesModel>();
            foreach (DataRow row in dt.Rows)
            {
                var tj = new HousingSalesModel();


                if (dt.Columns.Contains("Id"))
                {
                    tj.Id = To<Int32>(row, "Id");
                }
              


                list.Add(tj);
            }
            return list;
        }
        #endregion

    }
}
