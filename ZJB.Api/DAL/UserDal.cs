using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Api.DAL
{
    public class UserDal : BaseDal
    {
        public UserDal()
            : base("WX")
        { }
        private readonly ICacheManager cache = CacheFactory.GetInstance();
        private ILog logger = LogManager.GetLogger("UserDal");
        private NCBaseRule ncBase = new NCBaseRule();
        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>UserID大于0：成功 ，小于0：各种失败</returns>
        public virtual int addPublicUser(PublicUserModel entity)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetStoredProcCommand("P_Api_addPublicUser");
            AddOutParameter(cmd, "@UserID", DbType.Int32, 4);
            AddInParameter(cmd, "@Name", DbType.String, entity.Name);
            AddInParameter(cmd, "@Password", DbType.String, StringUtility.ToMd5String(passWordAdorn + entity.Password));
            AddInParameter(cmd, "@Portrait", DbType.String, entity.Portrait);
            AddInParameter(cmd, "@Molblie", DbType.String, entity.Molblie);
            AddInParameter(cmd, "@Tel", DbType.String, entity.Tel);
            AddInParameter(cmd, "@NickName", DbType.String, entity.NickName);
            AddInParameter(cmd, "@EnrolnName", DbType.String, entity.EnrolnName);
            AddInParameter(cmd, "@Sex", DbType.Int32, entity.Sex);
            AddInParameter(cmd, "@Address", DbType.String, entity.Address);
            AddInParameter(cmd, "@Email", DbType.String, entity.Email);
            AddInParameter(cmd, "@QQ", DbType.String, entity.QQ);
            AddInParameter(cmd, "@IP", DbType.String, entity.IP);
            AddInParameter(cmd, "@CityID", DbType.Int32, entity.CityID);
            AddInParameter(cmd, "@CompanyId", DbType.Int32, entity.CompanyId);
            AddInParameter(cmd, "@StoreId", DbType.Int32, entity.StoreId);
            AddInParameter(cmd, "@LastLoginIP", DbType.String, entity.LastLoginIP);
            AddInParameter(cmd, "@VipType", DbType.Int32, entity.VipType);
            AddInParameter(cmd, "@DistrictId", DbType.Int32, entity.DistrictId);
            AddInParameter(cmd, "@RegionId", DbType.Int32, entity.RegionId);
            AddInParameter(cmd, "@Points", DbType.Int32, 100); //初始赠送积分
            AddInParameter(cmd, "@Remarks", DbType.String, entity.Remarks);
            ExecuteNonQuery(cmd);
            int outUserId = 0;
            int.TryParse(cmd.Parameters["@UserID"].Value.ToString(), out outUserId);
            return outUserId;
        }
        /// <summary>
        /// 修改电话号码
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Tel"></param>
        /// <returns></returns>
        public virtual int UpdayePublicUserTel(int UserID, string Tel)
        {
            DbCommand cmd = GetSqlStringCommand("UPDATE PublicUser  SET Tel=@Tel WHERE UserID=@UserID");
            AddInParameter(cmd, "@UserID", DbType.Int32, UserID);
            AddInParameter(cmd, "@Tel", DbType.String, Tel);
            int isuccess = ExecuteNonQuery(cmd);
            return isuccess;
        }



        public virtual int addPublicUserThirdInfo(int UserID, int AppID, string OpenID, string UnionID)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetSqlStringCommand("INSERT INTO PublicUserThirdInfo(UserID, AppID, OpenID, UnionID) VALUES(@UserID, @AppID, @OpenID, @UnionID)");
            AddInParameter(cmd, "@UserID", DbType.Int32, UserID);
            AddInParameter(cmd, "@AppID", DbType.Int32, AppID);
            AddInParameter(cmd, "@OpenID", DbType.String, OpenID);
            AddInParameter(cmd, "@UnionID", DbType.String, UnionID);
            int isuccess = ExecuteNonQuery(cmd);
            return isuccess;
        }



        #endregion


        #region 用户登录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <param name="isAddLog">是否记录登陆明细 默认是</param>
        /// <returns></returns>
        public virtual PublicUserModel PublicUserLogin(string loginName, string pwd, int isAddLog, int userid)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetStoredProcCommand("P_Api_PublicUserLogin");
            AddInParameter(cmd, "@loginName", DbType.String, loginName);
            AddInParameter(cmd, "@userid", DbType.Int32, userid);
            AddInParameter(cmd, "@ip", DbType.String, IpUtility.GetIp());
            AddInParameter(cmd, "@pwd", DbType.String, StringUtility.ToMd5String(passWordAdorn + pwd));
            AddInParameter(cmd, "@isAddLog", DbType.Int32, isAddLog);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidPublicUser(ds.Tables[0].Rows[0]);
            }
            return null;

        }
        public virtual PublicUserModel PublicUserLoginByToken(string token)
        {

            DbCommand cmd = GetStoredProcCommand("P_Client_GetUserByToken");
            AddInParameter(cmd, "@token", DbType.String, token);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidPublicUser(ds.Tables[0].Rows[0]);
            }
            return null;

        }
        public virtual PublicUserModel PublicUserAdminLogin(string loginName, string pwd, int isAddLog = 1)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetStoredProcCommand("P_Api_PublicUserAdminLogin");
            AddInParameter(cmd, "@loginName", DbType.String, loginName);
            AddInParameter(cmd, "@ip", DbType.String, IpUtility.GetIp());
            AddInParameter(cmd, "@pwd", DbType.String, StringUtility.ToMd5String(passWordAdorn + pwd));
            AddInParameter(cmd, "@isAddLog", DbType.Int32, isAddLog);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidPublicUser(ds.Tables[0].Rows[0]);
            }
            return null;

        }
        private List<PublicUserModel> BuildPublucUserList(IEnumerable<DataRow> rows)
        {
            return (from item in rows select BulidPublicUser(item)).ToList();
        }
        private PublicUserModel BulidPublicUser(DataRow dr)
        {
            return new PublicUserModel()
            {
                Address = dr.Table.Columns.Contains("Address") ? To<string>(dr, "Address") : "",
                Email = dr.Table.Columns.Contains("Email") ? To<string>(dr, "Email") : "",
                EnrolnName = dr.Table.Columns.Contains("EnrolnName") ? To<string>(dr, "EnrolnName") : "",
                IP = dr.Table.Columns.Contains("IP") ? To<string>(dr, "IP") : "",
                LastLoginIP = dr.Table.Columns.Contains("LastLoginIP") ? To<string>(dr, "LastLoginIP") : "",
                LastLoginTime = dr.Table.Columns.Contains("LastLoginTime") ? To<DateTime>(dr, "LastLoginTime") : DateTime.Now,
                LoginTimes = dr.Table.Columns.Contains("LoginTimes") ? To<Int32>(dr, "LoginTimes") : 1,
                Molblie = dr.Table.Columns.Contains("Molblie") ? To<string>(dr, "Molblie") : "",
                Name = dr.Table.Columns.Contains("Name") ? To<string>(dr, "Name") : "",
                NickName = dr.Table.Columns.Contains("NickName") ? To<string>(dr, "NickName") : "",
                Portrait = dr.Table.Columns.Contains("Portrait") ? To<string>(dr, "Portrait") : "",
                QQ = dr.Table.Columns.Contains("QQ") ? To<string>(dr, "QQ") : "",
                RegisterTime = dr.Table.Columns.Contains("RegisterTime") ? To<DateTime>(dr, "RegisterTime") : DateTime.Now,
                Status = dr.Table.Columns.Contains("Status") ? To<Int32>(dr, "Status") : 1,
                Sex = dr.Table.Columns.Contains("Sex") ? To<Int32>(dr, "Sex") : 1,
                Tel = dr.Table.Columns.Contains("Tel") ? To<string>(dr, "Tel") : "",
                UserID = dr.Table.Columns.Contains("UserID") ? To<Int32>(dr, "UserID") : 0,
                PublishNum = dr.Table.Columns.Contains("PublishNum") ? To<Int32>(dr, "PublishNum") : 100,
                MaxStock = dr.Table.Columns.Contains("MaxStock") ? To<Int32>(dr, "MaxStock") : 50,
                VipType = dr.Table.Columns.Contains("VipType") ? To<Int32>(dr, "VipType") : 0,
                ExpirationTime = dr.Table.Columns.Contains("ExpirationTime") ? To<DateTime>(dr, "ExpirationTime") : DateTime.Now,
                CityID = dr.Table.Columns.Contains("CityID") ? To<Int32>(dr, "CityID") : 0,
                CompanyId = dr.Table.Columns.Contains("CompanyId") ? To<Int32>(dr, "CompanyId") : 0,
                StoreId = dr.Table.Columns.Contains("StoreId") ? To<Int32>(dr, "StoreId") : 0,
                DistrictId = dr.Table.Columns.Contains("DistrictId") ? To<Int32>(dr, "DistrictId") : 0,
                RegionId = dr.Table.Columns.Contains("RegionId") ? To<Int32>(dr, "RegionId") : 0,
                AccessCount = dr.Table.Columns.Contains("AccessCount") ? To<Int32>(dr, "AccessCount") : 0,
                CompanyName = dr.Table.Columns.Contains("CompanyName") ? To<string>(dr, "CompanyName") : "",
                StoreName = dr.Table.Columns.Contains("StoreName") ? To<string>(dr, "StoreName") : "",
                VipTypeName = dr.Table.Columns.Contains("VipTypeName") ? To<string>(dr, "VipTypeName") : "",
                Points = dr.Table.Columns.Contains("Points") ? To<Int32>(dr, "Points") : 0
            };
        }

        public virtual Credential PostLogin(string loginName, string password, int loginType, out int returnValue)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetStoredProcCommand("P_Client_Login");
            AddInParameter(cmd, "LoginName", DbType.String, loginName);
            AddInParameter(cmd, "Password", DbType.String, StringUtility.ToMd5String(passWordAdorn + password));
            AddInParameter(cmd, "Token", DbType.String, StringUtility.NewGuidString());
            AddInParameter(cmd, "ExpirationDate", DbType.DateTime, DateTime.Now.AddMonths(6));
            AddInParameter(cmd, "IP", DbType.String, IpUtility.GetIp());
            AddInParameter(cmd, "LoginType", DbType.Int32, loginType);
            AddOutParameter(cmd, "@returnValue", DbType.Int32, 0);
            var ds = ExecuteDataSet(cmd);
            returnValue = GetOutputParameter<Int32>(cmd, "@ReturnValue");
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            return BuildCredential(ds.Tables[0]);
        }
        public virtual Credential PostLoginByToken(string token)
        {

            DbCommand cmd = GetStoredProcCommand("P_Client_GetUserByToken");
            AddInParameter(cmd, "@token", DbType.String, token);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildCredential(ds.Tables[0]);
            }
            return null;

        }
        private Credential BuildCredential(DataTable dataTable)
        {
            Credential credential = new Credential();
            credential.Name = dataTable.Columns.Contains("Name") ? To<string>(dataTable.Rows[0], "Name") : "";
            credential.ExpirationDate = dataTable.Columns.Contains("ExpirationDate") ? To<DateTime>(dataTable.Rows[0], "ExpirationDate") : DateTime.Now;
            credential.UserID = dataTable.Columns.Contains("UserID") ? To<int>(dataTable.Rows[0], "UserID") : 0;
            credential.CityId = dataTable.Columns.Contains("CityId") ? To<int>(dataTable.Rows[0], "CityId") : 0;
            credential.Token = To<string>(dataTable.Rows[0], "Token");
            credential.Icon = To<string>(dataTable.Rows[0], "Portrait");
            credential.Email = dataTable.Columns.Contains("Email") ? To<string>(dataTable.Rows[0], "Email") : "";
            credential.Phone = dataTable.Columns.Contains("Tel") ? To<string>(dataTable.Rows[0], "Tel") : "";

            return credential;
        }
        #endregion

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private PublicUserModel EFMapToModel(VPublicUser userInfo)
        {
            return new PublicUserModel()
            {
                Address = userInfo.Address,
                Email = userInfo.Email,
                EnrolnName = userInfo.EnrolnName,
                IP = userInfo.IP,
                LastLoginIP = userInfo.LastLoginIP,
                LastLoginTime = userInfo.LastLoginTime,
                LoginTimes = userInfo.LoginTimes,
                Molblie = userInfo.Molblie,
                Name = userInfo.Name,
                NickName = userInfo.NickName,
                Portrait = userInfo.Portrait,
                QQ = userInfo.QQ,
                RegisterTime = userInfo.RegisterTime,
                Status = userInfo.Status,
                Sex = userInfo.Sex,
                Tel = userInfo.Tel,
                UserID = userInfo.UserID,
                PublishNum = userInfo.PublishNum,
                MaxStock = userInfo.MaxStock,
                VipType = userInfo.VipType,
                ExpirationTime = userInfo.ExpirationTime,
                CityID = userInfo.CityID,
                CompanyId = userInfo.CompanyId,
                StoreId = userInfo.StoreId,
                DistrictId = userInfo.DistrictId,
                RegionId = userInfo.RegionId,
                Points = userInfo.Points,
                CompanyName = userInfo.CompanyName,
                StoreName = userInfo.StoreName,
                OpenID = userInfo.OpenID
            };
        }
        #region 获取用户信息
        public virtual PublicUserModel GetUserById(int userid)
        {
            var userList = ncBase.CurrentEntities.VPublicUser.Where(u => u.UserID == userid);
            if (userList != null && userList.FirstOrDefault() != null)
            {
                VPublicUser userInfo = userList.FirstOrDefault();
                return EFMapToModel(userInfo);
            }
            return null;
        }
        public virtual PublicUserModel GetUserByOpenId(string openid)
        {
            var userList = ncBase.CurrentEntities.VPublicUser.Where(u => u.OpenID == openid);
            if (userList != null && userList.FirstOrDefault() != null)
            {
                VPublicUser userInfo = userList.FirstOrDefault();
                return EFMapToModel(userInfo);
            }
            return null;
        }


        public virtual string GetUserName(int uid)
        {
            string name = string.Empty;

            PublicUser userInfo = ncBase.CurrentEntities.PublicUser.Where(s => s.UserID == uid).FirstOrDefault();  //获取单条数据


            //int pageIndex = 0; //第几页 0 第一页
            //int pageSize = 10;
            //List<PublicUser> userList =
            //    ncBase.CurrentEntities.PublicUser.Where(s => s.Status == 1)
            //        .OrderByDescending(s => s.RegisterTime)
            //        .Skip(pageIndex*pageSize)
            //        .Take(pageSize)
            //        .ToList();

            //if (userList.IsNoNull())
            //{
            //  //ToDo:业务逻辑
            //}

            if (userInfo.IsNoNull())
            {
                name = userInfo.Name;
            }
            return name;
        }

        public virtual PublicUserModel getUserByTel(string tel)
        {
            var userList = ncBase.CurrentEntities.VPublicUser.Where(u => u.Tel == tel);
            if (userList != null && userList.FirstOrDefault() != null)
            {
                VPublicUser userInfo = userList.FirstOrDefault();
                return EFMapToModel(userInfo);
            }
            return null;

        }
        public virtual PublicUserModel getUserByName(string name)
        {
            var userList = ncBase.CurrentEntities.VPublicUser.Where(u => u.Name == name);
            if (userList != null && userList.FirstOrDefault() != null)
            {
                VPublicUser userInfo = userList.FirstOrDefault();
                return EFMapToModel(userInfo);
            }
            return null;

        }

        public virtual List<PublicUserModel> getadminUserOpenid()
        {
            DbCommand cmd = GetSqlStringCommand(@"   SELECT OpenID,c.NickName  FROM dbo.PublicUserManager AS a WITH(NOLOCK)
               INNER JOIN  dbo.PublicUserThirdInfo AS b WITH(NOLOCK) ON b.UserID = a.UserID AND a.Status = 1
               INNER JOIN  dbo.PublicUser AS c WITH(NOLOCK) ON a.UserID=c.UserID  AND c.Status = 1");
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return (from item in ds.Tables[0].Select()
                        select new PublicUserModel()
                        {
                            OpenID = ds.Tables[0].Columns.Contains("OpenID") ? To<string>(item, "OpenID") : "",
                            Name = ds.Tables[0].Columns.Contains("NickName") ? To<string>(item, "NickName") : "",
                        }).ToList();
            }


            return null;

        }


        #endregion

        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public virtual int UpdateUserPassword(int userId, string pwd)
        {
            string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
            DbCommand cmd = GetStoredProcCommand("P_Api_UserPasswordUpdate");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@pwd", DbType.String, StringUtility.ToMd5String(passWordAdorn + pwd));
            return ExecuteNonQuery(cmd);
        }
        #endregion

        #region 使用简报
        /// <summary>
        /// 使用简报
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual UserReportModel GetUserReport(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetUserReport");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                UserReportModel report = new UserReportModel()
                {
                    ErrorUserSiteCount = To<Int32>(ds.Tables[0].Rows[0], "ErrorUserSiteCount"),
                    HouseCount = To<Int32>(ds.Tables[0].Rows[0], "HouseCount"),
                    TodayHouseCount = To<Int32>(ds.Tables[0].Rows[0], "TodayHouseCount"),
                    TodayPostCount = To<Int32>(ds.Tables[0].Rows[0], "TodayPostCount"),
                    UserSiteCount = To<Int32>(ds.Tables[0].Rows[0], "UserSiteCount"),
                };
                return report;
            }
            return new UserReportModel();
        }
        #endregion

        #region 管理后台 每日用户登陆统计
        public virtual List<StatModel> GetLoginStat(StatReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetLoginStat");
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
        #region 每天访问登陆后的首页就增加一次统计
        public virtual int HomeIndexAccessStat(int userId, string name)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_HomeIndexAccessStat");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@name", DbType.String, name);
            AddInParameter(cmd, "@ip", DbType.String, IpUtility.GetIp());
            return ExecuteNonQuery(cmd);
        }
        #endregion
        #endregion 

        #region 获取登录信息
        public virtual Credential GetCredentialByToken(string token)
        {
            return cache.Get<Credential>(String.Format(CacheItemConstant.UserCredentialItem, token), 1440, () =>
            {
                DbCommand cmd = GetStoredProcCommand("P_Client_GetUserByToken");
                AddInParameter(cmd, "token", DbType.String, token);
                var ds = ExecuteDataSet(cmd);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return null;

                return BuildCredential(ds.Tables[0]);
            });
        }
        #endregion

        #region 首页 是否签到 和签到天数
        public UserTaskSignStat UserSingStat(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTask_SignStat");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            UserTaskSignStat signStat = new UserTaskSignStat();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                signStat.SignCount = To<Int32>(ds.Tables[0].Rows[0], "SignCount");
                signStat.TodaySign = To<Int32>(ds.Tables[0].Rows[0], "TodaySign");
                signStat.TodaySign_2 = To<Int32>(ds.Tables[0].Rows[0], "TodaySign_2");
                signStat.AllSignCount = To<Int32>(ds.Tables[0].Rows[0], "AllSignCount");
            }
            return signStat;
        }
        #endregion

        #region 用户访问排行
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<PublicUserModel> GetUserAccessStatList(UserAccessListReq parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_ActionLog_UserAccessList");
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@beginHour", DbType.Int32, parame.beginHour);
            AddInParameter(cmd, "@endHour", DbType.Int32, parame.endHour);
            AddInParameter(cmd, "@IpAddress", DbType.String, parame.IpAddress);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, totalSize);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            List<PublicUserModel> statList = new List<PublicUserModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildPublucUserList(ds.Tables[0].Select());
            }
            return statList;

        }
        #endregion

        #region 获取通讯录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<PublicUserModel> GetUserContacts(string keyword, int companyId, int storeId, int pi, int ps, string letter, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_User_GetUserContactsList");
            AddInParameter(cmd, "@pageIndex", DbType.Int32, pi);
            AddInParameter(cmd, "@pageSize", DbType.Int32, ps);
            AddInParameter(cmd, "@companyId", DbType.Int32, companyId);
            AddInParameter(cmd, "@storeId", DbType.Int32, storeId);
            AddInParameter(cmd, "@points", DbType.Int32, 0);
            AddInParameter(cmd, "@keyword", DbType.String, keyword);
            AddInParameter(cmd, "@letter", DbType.String, letter);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, totalSize);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            List<PublicUserModel> statList = new List<PublicUserModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildPublucUserList(ds.Tables[0].Select());
            }
            return statList;

        }
        #endregion

        #region 生成验证码
        public virtual int AddCaptcha(string phone, string captcha, DateTime expirationDate, int type)
        {
            DbCommand cmd = GetStoredProcCommand("P_AddCaptcha");
            AddInParameter(cmd, "Phone", DbType.String, phone);
            AddInParameter(cmd, "Type", DbType.Int32, type);
            AddInParameter(cmd, "Captcha", DbType.Int32, captcha);
            AddInParameter(cmd, "ExpirationDate", DbType.DateTime, expirationDate);

            return ExecuteNonQuery(cmd);



        }
        #endregion

        #region 验证验证码
        public virtual int CheckCaptcha(string phone, string captcha, int type)
        {
            int totalSize = 0;
            DbCommand cmd = GetStoredProcCommand("P_CheckCaptcha");
            AddInParameter(cmd, "Phone", DbType.String, phone);
            AddInParameter(cmd, "Type", DbType.Int32, type);
            AddInParameter(cmd, "Captcha", DbType.Int32, captcha);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, totalSize);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            return totalSize;

        }
        #endregion

        #region 用户积分操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="points"></param>
        /// <param name="pointDesc"></param>
        /// <returns></returns>
        public virtual int AddUserPoint(int uid, int points, string pointDesc)
        {

            DbCommand cmd = GetStoredProcCommand("P_User_AddPoint");
            AddInParameter(cmd, "@UserId", DbType.Int32, uid);
            AddInParameter(cmd, "@Points", DbType.Int32, points);
            AddInParameter(cmd, "@PointDesc", DbType.String, pointDesc);
            return ExecuteNonQuery(cmd);
        }
        #endregion 

        #region 获取用户的相关的总店门店相关信息
        public List<CompanyModel> GetUserCompanyList(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_User_CompanyList");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<CompanyModel> companyList = new List<CompanyModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    companyList.Add(new CompanyModel()
                    {
                        CompanyId = To<Int32>(dr, "CompanyId"),
                        Name = To<string>(dr, "Name"),
                        ParentId = To<Int32>(dr, "ParentId"),
                    });
                }
            }
            return companyList;
        }
        #endregion

        #region 今日签到排行
        public List<UserSignTop> GetTodayUserSignTop(int pi, int ps, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_User_SignTop_Test");
            AddInParameter(cmd, "@pi", DbType.Int32, pi);
            AddInParameter(cmd, "@ps", DbType.Int32, ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            List<UserSignTop> topList = new List<UserSignTop>();
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    topList.Add(new UserSignTop()
                    {
                        UserId = To<Int32>(dr, "UserId"),
                        Name = To<string>(dr, "Name"),
                        AddTime = To<DateTime>(dr, "AddTime"),
                        Portrait = To<string>(dr, "Portrait"),
                        SignCount = To<Int32>(dr, "SignCount"),
                        TopIndex = To<Int32>(dr, "TopIndex")
                    });
                }
            }
            return topList;
        }

        public SignRightStat GetSignRightStat(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserSignRightStat");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            SignRightStat signStat = new SignRightStat();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                signStat = new SignRightStat()
                {
                    MySignCount = To<int>(ds.Tables[0].Rows[0], "MySignCount"),
                    TodaySignCount = To<int>(ds.Tables[0].Rows[0], "TodaySignCount"),
                    YesterdaySignCount = To<int>(ds.Tables[0].Rows[0], "YesterdaySignCount")
                };
            }
            return signStat;
        }
        #endregion

        #region 邀请排行榜
        public List<UserSignTop> GetInviteList(int pi, int ps, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_User_GetInviteList");
            AddInParameter(cmd, "@pi", DbType.Int32, pi);
            AddInParameter(cmd, "@ps", DbType.Int32, ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            List<UserSignTop> topList = new List<UserSignTop>();
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    topList.Add(new UserSignTop()
                    {
                        UserId = To<Int32>(dr, "UserId"),
                        Name = To<string>(dr, "Name"),
                        Portrait = To<string>(dr, "Portrait"),
                        SignCount = To<Int32>(dr, "InviteCount"),
                        TopIndex = To<Int32>(dr, "rownum")
                    });
                }
            }
            return topList;
        }
        #endregion
    }
}