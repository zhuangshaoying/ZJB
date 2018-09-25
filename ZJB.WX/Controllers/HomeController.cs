using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Models;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;
using System.IO;
using System.Text;
using ZJB.WX.Filters;

namespace ZJB.WX.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
    
        [ActionLog(CheckPoints = false)]
        public ActionResult Index(string menu="",string url="")
        {
           
      
            PublicUserModel loginUser = this.GetLoginUser();
            if (loginUser.IsNull())
            {
                return   Redirect("/");
            }
            #region （每天一次）每天访问登陆后的首页就增加一次统计,避免cooike自动登录的 统计不到登陆明细
            if (loginUser!=null&&loginUser.UserID > 0)
            {
                userBll.HomeIndexAccessStat(loginUser.UserID, loginUser.Name);
            }
            #endregion

            int companyId =loginUser.IsNoNull()? Convert.ToInt32(loginUser.CompanyId):0;
            Company company = ncBase.CurrentEntities.Company.Where(c => c.CompanyId == companyId).FirstOrDefault();
            if (company != null) {
                ViewBag.CompanyName = company.Name;
            }
            int storeId = loginUser.IsNoNull() ? Convert.ToInt32(loginUser.StoreId) : 0;
            CompanyStore companyStore = ncBase.CurrentEntities.CompanyStore.Where(c => c.StoreId == storeId).FirstOrDefault();
            if (companyStore != null)
            {
                ViewBag.CompanyStoreName = companyStore.StoreName;
            }
            ViewBag.Url = url;
            ViewBag.Menu = menu;
            return View(loginUser);
        }
        [AllowAnonymous]
        public ActionResult City()
        {
        

            return View();
        }
       [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Cooperation()
        {
            return View();
        }
        
        [AllowAnonymous]
       public ActionResult AppDownLoad()
       {
           return View();
       }
        public ActionResult GetMainRight()
        {
            PublicUserModel user = this.GetLoginUser();
            return View(user);
        }
        /// <summary>
        /// 首页->使用简报
        /// </summary>
        /// <returns></returns>
        public ActionResult UseReport()
        {
            UserReportModel report = userBll.GetUserReport(this.GetLoginUser().UserID);
            return View(report);
        }
        [HttpPost]
        public JsonResult AddFeedback(Feedback item)
        {
            item.UserId=this.GetLoginUser().UserID;
            item.CreateTime = DateTime.Now;
            ncBase.CurrentEntities.Feedback.AddObject(item);
            ncBase.CurrentEntities.SaveChanges();
            return Json(new { status = item.FeedbackId });
        }

        #region Ueditor 图片上传服务端
        /// <summary>
        /// Ueditor
        /// </summary>
        /// <returns></returns>
        public ActionResult UeditorContro()
        {
            string jsonpCallback = Request["callback"],
             json = JsonConvert.SerializeObject(null);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {




                return Content(json);
           
            }
            else
            {
                return Content(String.Format("{0}({1});", jsonpCallback, json));
            }

        }
        #endregion

        #region Api Debug
        [AllowAnonymous]
        public ActionResult Debug(int minutes = 1000, bool forceLog = false)
        {

           
            DateTime expire = DateTime.Now.AddMinutes(minutes);

            HttpCookie debugCookie = new HttpCookie("EnableApiDebug", minutes.ToString());
            debugCookie.Expires = expire;
            Response.Cookies.Add(debugCookie);

            if (forceLog)
            {
                HttpCookie forceLogCookie = new HttpCookie("EnableForceLog", minutes.ToString());
                forceLogCookie.Expires = expire;
                Response.Cookies.Add(forceLogCookie);
            }

            return Json(new { Message = string.Format("Enabled for {0} minutes", minutes) }, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Invite(string from = "ZJB", string info = "{\"UserId\":123456,\"Tel\":\"18559313370\",\"UserName\":\"小庄仔\",\"Company\":\"麦田房产\",\"Store\":\"会展店\",\"Portrait\":\"http://img.ZJB.com/MyUpload/InfoImage/20150209/H_201502094310797.jpg\"}")
        {
           string a =
                "9EyPOixJLj3bLkAUNuoVoXgv7/pbEy202/P995aa78azCeU1+9d7/cn4Iu1xBfTDdGKw6qYNuTBg/sxS57M+WEkea6noKpZO/9xgvjL74R8D/8fTAb3NoV3D0Gj0HSj3y9U6cRo0LrWUkRZAB21+zox/KBq0AhctkBFcZfQcqZ45HIloeIytZaWMutF2zPOC6mLyXhT7SC64tTJUtQFORzJOS0WkyxMZf042X3xSXfF8oPqeKSonkhZYnvd/teuzf09s0qimhh/FFFhg9PCqug==";
            info = CryptoUtility.TripleDESDecrypt(a);
         
           // info = CryptoUtility.TripleDESEncrypt(info);
            ViewBag.Info = a;
            ViewBag.From = from;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Test()
        {
                  WeiXinPublic wxPublic = new WeiXinPublic();
                  wxPublic.WxPushMessage("oM9lvxHnTVhtS7qsKpPIbVLMfi1s", "您好，您有一条新的看房预约消息。",
                                     "-" +  "-" 
                                    , " ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "http://www.zhujia001.com/Esf/Bespoke?houseid=1");

            return Content("Okay");
            //string cookie = Loginer_Xms.Login("18030278595", "123456");
            //byte tradeType = 1;
            //int buildingType = 1;
            //int size = 30;
            //var houses = Lister_Xms.GetHouseDetails(tradeType, buildingType, cookie, size);
            //return Content(houses.ToString());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DESDec(string dec)
        {


            return Content(CryptoUtility.TripleDESDecrypt(dec));
          
        }
        public ActionResult HeadScript(string txt="",int count=0)
        {
            ViewBag.Text = txt;
            ViewBag.Count = count;
            if (!string.IsNullOrEmpty(txt) && count > 0)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "/Scripts/HeadLogoScripts.js";
                string content="var logo_text = '{0}';var logo_text_count='{1}';";
                using (StreamWriter reader = new StreamWriter(path,false,Encoding.UTF8))
                {
                    reader.Write(string.Format(content,txt,count));
                }
            }
            return View();
        }
        #endregion

      

  
    }
}
