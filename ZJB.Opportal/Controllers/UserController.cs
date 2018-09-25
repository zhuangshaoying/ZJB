using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Models;

namespace ZJB.Opportal.Controllers
{

   [Authorization]
    public class UserController : Controller
    {
        private NCBaseRule ncBase = new NCBaseRule();
        UserBll user = new UserBll();
        protected readonly SmsApi smsApi = new SmsApi();
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /User/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Register(string code="")
        {

             
            return View();
        }
    

        public ActionResult LoginOut()
        {
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXAdminLoginedUser";
            }
            if (Session[saveKey] != null)
            {
                Session[saveKey] = null;
                Session.Remove(saveKey);
            }
            if (Request.Cookies[saveKey] != null)
            {
                Request.Cookies[saveKey].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies[saveKey]);
            }
            return Redirect("/User/Login");
           
        }
        /// <summary>
        /// 登陆提交
        /// </summary>
        /// <param name="phone">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult DoLogin(string phone, string userPwd)
        {
            UserBll userApi = new UserBll();
            PublicUserModel user = userApi.PublicUserAdminLogin(phone, userPwd);
            if (user != null && user.UserID > 0)
            {
                String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
                if (String.IsNullOrEmpty(saveKey))
                {
                    saveKey = "WXAdminLoginedUser";
                }
                Session[saveKey] = user;
                HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                loginUserCookie.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(loginUserCookie);
                return Json(new { status = 0 });
            }
            return Json(new { status = 1, msg = "用户名或者密码错误" });
        }
        #region 邀请码管理
        [AllowAnonymous]
        public ActionResult InvitationCodeList()
        {

            List<InvitationCode> invitationCode = new List<InvitationCode>();
       

         
                invitationCode =
                  ncBase.CurrentEntities.InvitationCode.Where(o =>  o.IsUsed == false).ToList();
            
            return View(invitationCode);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddInvitationCode(FormCollection form)
        {
            string tels = form["tel"];
            string names = form["name"];
            if (!string.IsNullOrEmpty(tels))
            {
                string[] arrs = tels.Split(',');
                string[] arrnames=!string.IsNullOrEmpty(names)?names.Split(','): null;

                for (int i = 0; i < arrs.LongLength; i++)
                {
                    
              
                    try
                    {
                        InvitationCode invitationCode = new InvitationCode();
                        invitationCode.Code = StringUtility.GetSubfix();
                        invitationCode.BindTel = arrs[i];
                        invitationCode.Invitee = (arrnames.IsNoNull()&&arrnames.Length > i ? arrnames[i] : "");
                        invitationCode.IsUsed = false;
                        invitationCode.UsedTime =Convert.ToDateTime("1900-1-1");
                        invitationCode.CreateTime = DateTime.Now;
                        invitationCode.VipType = 2;
                        ncBase.CurrentEntities.AddToInvitationCode(invitationCode);
                        ncBase.CurrentEntities.SaveChanges();
                    }
                    catch (Exception)
                    {
                        InvitationCode invitationCode = new InvitationCode();
                        invitationCode.Code = StringUtility.GetSubfix();
                        invitationCode.BindTel = arrs[i];
                        invitationCode.Invitee = (arrnames.IsNoNull() && arrnames.Length > i ? arrnames[i] : "");
                        invitationCode.IsUsed = false;
                        invitationCode.UsedTime = Convert.ToDateTime("1900-1-1");
                        invitationCode.CreateTime = DateTime.Now;
                        invitationCode.VipType = 2;
                        ncBase.CurrentEntities.AddToInvitationCode(invitationCode);
                        ncBase.CurrentEntities.SaveChanges();
                    }
                   }
                
            }


            return RedirectToAction("InvitationCodeList");
        }
         [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendInvitationCode(int codeId)
         {
             string smsContent = "现诚邀{0}注册使用“房产盒子”_免费房产网络营销软件,邀请码：{1} ,访问 http://fcHeZi.com 开通使用。";
           
                        InvitationCode invitationCode = ncBase.CurrentEntities.InvitationCode.Where(o=>o.CodeId==codeId).FirstOrDefault();
                        if (invitationCode.IsNoNull() && !string.IsNullOrEmpty(invitationCode.BindTel))
             {
                 SendResult sendResult = smsApi.SendSms(invitationCode.BindTel, string.Format(smsContent, invitationCode.Invitee, invitationCode.Code), (Purpose)8, "【房产盒子】");
                 if (sendResult.Status == Status.Success)
                 {
                     invitationCode.IsSend = true;
                     ncBase.CurrentEntities.SaveChanges();
                     return Content("1");
                 }
             }


                        return Content("0");
         }

        #endregion

        #region 中介门店管理

         
         public ActionResult CompanyList(int cityId=592)
         {

             List<Company>  companies = new List<Company>();

             companies =
               ncBase.CurrentEntities.Company.Where(o => o.CityID == cityId).ToList();

             return View(companies);
         }
       
         public JsonResult GetCompany(int cityId)
         {

             List<Company> companies = new List<Company>();

             companies =
               ncBase.CurrentEntities.Company.Where(o => o.CityID == cityId).ToList();

             return Json(companies);
         }
     
         public JsonResult GetStore(int companyId)
         {

             List<CompanyStore> companyStores = new List<CompanyStore>();

             companyStores =
               ncBase.CurrentEntities.CompanyStore.Where(o => o.CompanyId == companyId).ToList();

             return Json(companyStores);
         }
         [HttpPost]
         [ValidateInput(false)]
         public ActionResult SaveCompany(int companyId, int cityId, string companyname, string companytel)
         {
             bool isnew=false;
             if (string.IsNullOrEmpty(companyname)  || cityId == 0)
             return Content("0");
             Company company = ncBase.CurrentEntities.Company.Where(o=>o.CompanyId==companyId).FirstOrDefault();
             if (company.IsNull())
             {
                 isnew = true;
         company=new Company();
                 company.Address = "";
             }
             company.CityID = cityId;
             company.Tel = companytel;
             company.Name = companyname;

             if (isnew)
             {
                 ncBase.CurrentEntities.AddToCompany(company);
             }
             ncBase.CurrentEntities.SaveChanges();

             return Content(company.CompanyId.ToString());
         }

         [HttpPost]
         [ValidateInput(false)]
         public ActionResult SaveStore(int companyId, int storeId, string storename, string storetel, int cityId)
         {
             bool isnew = false;
             if (string.IsNullOrEmpty(storename) || companyId == 0)
                 return Content("0");
             CompanyStore companystore = ncBase.CurrentEntities.CompanyStore.Where(o => o.StoreId == storeId).FirstOrDefault();
             if (companystore.IsNull())
             {
                 isnew = true;
                 companystore = new CompanyStore();
                 companystore.Address = "";
             }
             companystore.CityID = cityId;
             companystore.Tel = storetel;
             companystore.CompanyId = companyId;
             companystore.StoreName = storename;

             if (isnew)
             {
                 ncBase.CurrentEntities.AddToCompanyStore(companystore);
             }
             ncBase.CurrentEntities.SaveChanges();

             return Content(companystore.StoreId.ToString());
         }
        
        #endregion

       #region 用户管理
         public ActionResult UserList(int pageIndex = 1, int pageSize = 20,string keyWord="")
         {
             string sqlwhere = "1==1";

             if (!string.IsNullOrEmpty(keyWord))
             {
                 int i = 0;
                 int.TryParse(keyWord, out i);
                 if (i > 0)
                 {
                     sqlwhere += " and it.UserID==" + keyWord;
                 }
                 else
                 {
                     sqlwhere += " and (it.Name like '%" + keyWord + "%' or it.tel like '%" + keyWord + "%' )";
                 }
             }
             
             List<VPublicUser> publicUsers =
                 ncBase.CurrentEntities.VPublicUser.Where(sqlwhere)
                     .OrderByDescending(o => o.UserID)
                     .Skip(pageSize*(pageIndex - 1))
                     .Take(pageSize)
                     .ToList();
             ViewBag.Count = ncBase.CurrentEntities.PublicUser.Where(sqlwhere).Count();
            
             ViewBag.PageIndex = pageIndex;
             ViewBag.PageSize = pageSize;
             ViewBag.KeyWord = keyWord;
             ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
             return View(publicUsers);
         }

         public ActionResult AddNewUser(string tel)
       {
           InvitationCode invitationCode = ncBase.CurrentEntities.InvitationCode.Where(o => o.BindTel == tel &&o.IsUsed==false).FirstOrDefault();
             if (invitationCode.IsNull())
             {
                 invitationCode = new InvitationCode();
                 invitationCode.Code = StringUtility.GetSubfix();
                 invitationCode.BindTel = tel;
                 invitationCode.Invitee = "";
                 invitationCode.IsUsed = false;
                 invitationCode.UsedTime = Convert.ToDateTime("1900-1-1");
                 invitationCode.CreateTime = DateTime.Now;
                 invitationCode.VipType = 2;
                 ncBase.CurrentEntities.AddToInvitationCode(invitationCode);
                 ncBase.CurrentEntities.SaveChanges();
             }
             ViewBag.Code = invitationCode.Code;
             return View();
       }
        [HttpGet]
         public JsonResult GetUserDetailInfo(int id)
         {
             VPublicUser user = ncBase.CurrentEntities.VPublicUser.Where(u => u.UserID == id).FirstOrDefault();
             return Json(user,JsonRequestBehavior.AllowGet);
         }
       [HttpPost]
        public JsonResult AddUserPoint(int id = 0, int points = 0, int taskType=0, string pointDesc="")
        {
           
            if (taskType > 0)//关联任务的
            {
                UserTaskLogBll userTaskBll = new UserTaskLogBll();
                int point = userTaskBll.UserTaskLogAdd(id, taskType, pointDesc);
                if (point > 0)
                {
                    return Json(new { code = 1, msg = "操作成功增加" + point + "分" });
                }
                else
                {
                    return Json(new { code = -100, msg = "未知错误" });
                }
            }
            else
            {
                if (points == 0)
                    return Json(new { code = -1, msg = "改动积分不能为0" });
                if (string.IsNullOrEmpty(pointDesc))
                    return Json(new { code = -1, msg = "请添加积分备注" });
                int row = user.AddUserPoint(id, points, pointDesc);
                if (row > 0)
                {
                    return Json(new { code = 1, msg = "操作成功" });
                }
                return Json(new { code = -100, msg = "未知错误" });
            }
        }
       [HttpPost]
       public JsonResult EditCompany(int id = 0, int companyId = 0, int companyStoreId = 0, int districtId = 0, int regionId = 0,int cityId=0)
       {
           PublicUser publicUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == id).FirstOrDefault();
           if(publicUser.IsNull())
               return Json(new { code = -1, msg = "该用户不存在" });
          if(cityId<1 || companyId<1)
              return Json(new { code = -2, msg = "城市或公司ID出错" });
           publicUser.CityID = cityId;
           publicUser.CompanyId = companyId;
           publicUser.StoreId = companyStoreId;
           publicUser.DistrictId = districtId;
           publicUser.RegionId = regionId;
           ncBase.CurrentEntities.SaveChanges();
         return Json(new { code = 1, msg = "操作成功" });
       
       }
       [AllowAnonymous]
       public JsonResult GetUserCompanyByKey(string act, string key, int cityId = 0, int companyId = 0)
       {
           if (act == "companyStore")
           {
               List<CompanyStore> companyList = ncBase.CurrentEntities.CompanyStore.Where(c => c.StoreName.Contains(key) && c.CompanyId == companyId && c.CityID == cityId).ToList();
               if (companyList != null && companyList.Count > 0)
               {
                   var resultList = companyList.Select(c => new
                   {
                       Name = c.StoreName,
                       CompanyId = c.StoreId
                   }).Take(10);
                   return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
               }
           }
           else
           {
               List<Company> companyList = ncBase.CurrentEntities.Company.Where(c => c.Name.Contains(key) && c.CityID == cityId).ToList();
               if (companyList != null && companyList.Count > 0)
               {
                   var resultList = companyList.Select(c => new
                   {
                       c.Name,
                       c.CompanyId
                   }).Take(10);
                   return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
               }
           }
           return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
       }


       [AllowAnonymous]
       public JsonResult GetRegionList(int cityId = 0, int districtId = 0)
       {
           List<Regions> regionList = ncBase.CurrentEntities.Regions.Where(r => r.CityID == cityId && r.DistrctID == districtId).ToList();
           if (regionList != null && regionList.Count > 0)
           {
               var resultList = regionList.Select(r => new
               {
                   r.RegionID,
                   r.Name
               });
               return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
           }
           return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
       }
        #endregion
      
       #region 发送短信

       public ActionResult Sendsms()
       {
           List<SendsmsLog> sendsmsLogs = new List<SendsmsLog>();



           sendsmsLogs =
             ncBase.CurrentEntities.SendsmsLog.OrderByDescending(p=>p.Id).Take(50).ToList();

           return View(sendsmsLogs);
         
       }
       [HttpPost]
       [ValidateInput(false)]
       public ActionResult Sendsms(FormCollection form)
       {

           string tels = form["tel"];
           string content = form["content"];
           var name = this.GetLoginUser().Name;
           if (!string.IsNullOrEmpty(tels))
           {
               string[] arrs = tels.Split(',');
             

               for (int i = 0; i < arrs.LongLength; i++)
               {

                  SendResult sendResult = smsApi.SendSms(arrs[i], content, (Purpose)8, "【房产盒子】");
            //SendResult sendResult= new SendResult();
                   sendResult.Status = Status.Success;
                   SendsmsLog sendsmsLog= new SendsmsLog();
                   sendsmsLog.Tel = arrs[i];
                   sendsmsLog.SmsContent = content;
                   sendsmsLog.Sender = name;
                   sendsmsLog.SendTime = DateTime.Now;
                   sendsmsLog.SendResult = sendResult.Status.ToString();
                  ncBase.CurrentEntities.AddToSendsmsLog(sendsmsLog);
                   ncBase.CurrentEntities.SaveChanges();

               }

           }


           return RedirectToAction("Sendsms");

          
       }
        #endregion

       #region 加密
       public ActionResult GetPwd(String pwd)
       {

           string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
           return Content(StringUtility.ToMd5String(passWordAdorn + pwd));

       }
       #endregion

       #region 加密
       public JsonResult ResetPwd(int id = 0)
       {
           PublicUser publicUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == id).FirstOrDefault();
           if (publicUser.IsNull())
           {
               return Json(new { status = 1, msg = "没有找到用户！（用户ID："+id+"）" });
           }
           string passWordAdorn = ConfigUtility.GetValue("PassWordAdorn");
           string passWord = string.Empty;
           if (string.IsNullOrEmpty(publicUser.Tel))
           {
               passWord = Convert.ToString(publicUser.UserID);
           }
             PublicUserModel loginUser = this.GetLoginUser();
           passWord = publicUser.Tel;
           publicUser.Password = StringUtility.ToMd5String(passWordAdorn + passWord);
           ncBase.CurrentEntities.SaveChanges();
           OperatorLog operatorLog=new OperatorLog();
           operatorLog.Note = "重置密码,被重置用户ID:" + publicUser.UserID + " 新密码：" + passWord;
           operatorLog.Type = 1;
           operatorLog.Uid = loginUser.UserID;
           operatorLog.AddTime = DateTime.Now;
           ncBase.CurrentEntities.AddToOperatorLog(operatorLog);
           ncBase.CurrentEntities.SaveChanges();

           return Json(new { status = 0, msg = "密码重置成功， 新密码：" + passWord });
       }
       #endregion

       #region 积分日志


       public ActionResult PointsLog(int pageIndex = 1, int pageSize = 20, string keyWord = "")
       {

           string sqlwhere = "1==1";

           if (!string.IsNullOrEmpty(keyWord))
           {
               int i = 0;
               int.TryParse(keyWord, out i);
               if (i > 0)
               {
                   sqlwhere += " and it.UserID==" + keyWord;
               }
               else
               {
                   sqlwhere += " and (it.Name like '%" + keyWord + "%' or it.tel like '%" + keyWord + "%'  or it.Description like '%" + keyWord + "%' )";
               }
           }

           List<VPublicUserPointLog> publicUserPointsLog =
           ncBase.CurrentEntities.VPublicUserPointLog.Where(sqlwhere)
               .OrderByDescending(o => o.Id)
               .Skip(pageSize * (pageIndex - 1))
               .Take(pageSize)
               .ToList();
           ViewBag.Count = ncBase.CurrentEntities.VPublicUserPointLog.Where(sqlwhere).Count();

           ViewBag.PageIndex = pageIndex;
           ViewBag.PageSize = pageSize;
           ViewBag.KeyWord = keyWord;
           ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);


           return View(publicUserPointsLog);
       }
        #endregion

    
    }
}
