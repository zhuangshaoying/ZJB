using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.WX.Common;


namespace ZJB.WX.Controllers
{
    public class QRCodeController : Controller
    {
        //
        // GET: /QRCode/

        public ActionResult Index(string url)
        {
            ViewBag.ErcUrl = url;
            return View();

        }
        public ActionResult Erc(string url, int s = 0, int v = 0, int loupanId = 0)
        {

            if (string.IsNullOrWhiteSpace(url))
                return new EmptyResult();
            QrCodes qrCodes = new QrCodes();
            if (s == 0)
                s = 4;
            if (v == 0)
                v = 7;
            Image image = null;
            string logoPath = Server.MapPath("/Logo/") + loupanId + ".png";
            bool hasLogo = System.IO.File.Exists(logoPath);
            if (loupanId > 0 && hasLogo)
            {
                image = qrCodes.CreateQRCodeLogo(url, logoPath, v, s);
            }
            else
            {
                image = qrCodes.CreateQRCode(url, v, s);
            }
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            Response.ContentType = "image/jpeg";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
            ms.Close();
            return new EmptyResult();

        }
    }
}
