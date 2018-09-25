using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Common;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;

namespace ZJB.WX.Controllers
{
    public class ImageUploadController : BaseController
    {
        HashSet<string> exts = new HashSet<string>(StringComparer.OrdinalIgnoreCase){".gif",".jpg",".jpeg",".png",".bmp"};
       
        //
        // GET: /ImgUpload/
        [IgnoreValidate]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 图片上传至服务器temp目录，七牛后台已配置若图片在七牛不存在则自动到fchezi的temp目录获取
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult Add(FormCollection form)
        {
            string domain = ConfigUtility.GetValue("domain");
            string imageType = form["imageType"];
           
            //最大文件大小f
            int maxSize = 1000000;

            HttpPostedFileBase imgFile = Request.Files["file"];
            if (imgFile == null)
            {
                return Content("<script>document.domain='" + domain + "';parent.callback('" + imageType + "', '请选择文件。', null);</script>");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName);

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Content("<script>parent.callback(''" + imageType + "'', '上传文件大小超过限制。', null);</script>");
            }

            if (!exts.Contains(fileExt))
            {
                return Content("<script>parent.callback(''" + imageType + "'', '上传文件扩展名是不允许的扩展名。\n只允许" + (exts.Aggregate((a,b)=>a+", "+b)) + "格式', null);</script>");
            }


            imgFile.InputStream.Position = 0;
            //if (Request.Url.Host.Contains("localhost") || Request.UserHostAddress == "127.0.0.1")
            //{
            //    using (Stream stream = imgFile.InputStream)
            //    {
              //    string imgUrl = QiniuService.PutFileStringStream(stream, "WX", fileExt);
            //        return Content("<script>parent.callback(''" + imageType + "'', '" + imgUrl + "', null);</script>");
            //    }
            //}
            //imgFile.InputStream.Position = 0;
            //MemoryStream target = new MemoryStream();
            //imgFile.InputStream.CopyTo(target);
     
            //byte[] data = target.ToArray();
            //#region 水印

            //int pos = 0;
            //if (form["wmPos"] != null && form["wmPos"].ToString() != "")///有设置水印
            //{
            //    int.TryParse(form["wmPos"].ToString(), out pos);
            //}
            //if (pos > 0)
            //{
            //    if (form["wmUrl"] != null && form["wmUrl"].ToString() != "")///有水印图片
            //    {
            //        string wmUrl = form["wmUrl"].ToString();
            //        string ioPath = this.HttpContext.Server.MapPath(wmUrl);
            //        System.Drawing.Image image = System.Drawing.Image.FromStream(target);
            //        if (image.Width < 300 || image.Height < 200)
            //        {
            //            return Content("<script>parent.alert(''" + imageType + "'', '添加水印时尺寸不能小于300*200！', null);</script>");
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(wmUrl) && System.IO.File.Exists(ioPath))
            //            {
            //                data = addWaterMark(image, ioPath, (WaterPositionMode)pos);
            //            }
            //        }
            //    }
            //}
            //#endregion
            //string md5 = ToMd5String(data);
            //var path = @"Images\temp\";
            //var fullPath = AppDomain.CurrentDomain.BaseDirectory + path;
            //if (!Directory.Exists(fullPath))
            //    Directory.CreateDirectory(fullPath);
            //System.IO.File.WriteAllBytes(fullPath + md5 + fileExt, data);
            //return Content("<script>parent.callback('" + imageType + "', '"
            //    + ConfigUtility.GetValue("QiNiuDomain") + md5 + fileExt + "', null);</script>");


            imgFile.InputStream.Position = 0;
            MemoryStream target = new MemoryStream();
            imgFile.InputStream.CopyTo(target);

            byte[] data = target.ToArray();
            #region 水印

            int pos = 0;
            if (form["wmPos"] != null && form["wmPos"].ToString() != "")///有设置水印
            {
                int.TryParse(form["wmPos"].ToString(), out pos);
            }
            if (pos > 0)
            {
                if (form["wmUrl"] != null && form["wmUrl"].ToString() != "")///有水印图片
                {
                    string wmUrl = form["wmUrl"].ToString();
                    string ioPath = this.HttpContext.Server.MapPath(wmUrl);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(target);
                    if (image.Width < 300 || image.Height < 200)
                    {
                        return Content("<script>parent.alert(''" + imageType + "'', '添加水印时尺寸不能小于300*200！', null);</script>");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(wmUrl) && System.IO.File.Exists(ioPath))
                        {
                            data = addWaterMark(image, ioPath, (WaterPositionMode)pos);
                        }
                    }
                }
            }
            #endregion

            string md5 = ToMd5String(data);
           
       
                string imgUrl = QiniuService.PutFile("zhujia", md5+ fileExt  , data);
                return Content("<script>parent.callback(''" + imageType + "'', '" + imgUrl + "', null);</script>");
          

          
   
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult uploadimage(FormCollection form)
        {
            string domain = ConfigUtility.GetValue("domain");


            //最大文件大小f
            int maxSize = 1000000;

            HttpPostedFileBase imgFile = Request.Files["photoimg"];
            if (imgFile == null)
            {
                return JsonReturnValue(new { Success = 0, imgurl = "",msg= "请选择文件" }, JsonRequestBehavior.AllowGet);
         
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName);

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return JsonReturnValue(new { Success = 0, imgurl = "", msg = "上传文件大小超过限制" }, JsonRequestBehavior.AllowGet);
            
            }

            if (!exts.Contains(fileExt))
            {
                return JsonReturnValue(new { Success = 0, imgurl = "", msg = "上传文件扩展名是不允许的扩展名。\n只允许" + (exts.Aggregate((a, b) => a + ", " + b)) + "格式" }, JsonRequestBehavior.AllowGet);

            }


            imgFile.InputStream.Position = 0;
          

            imgFile.InputStream.Position = 0;
            MemoryStream target = new MemoryStream();
            imgFile.InputStream.CopyTo(target);

            byte[] data = target.ToArray();
            #region 水印

            int pos = 0;
            if (form["wmPos"] != null && form["wmPos"].ToString() != "")//有设置水印
            {
                int.TryParse(form["wmPos"].ToString(), out pos);
            }
            if (pos > 0)
            {
                if (form["wmUrl"] != null && form["wmUrl"].ToString() != "")//有水印图片
                {
                    string wmUrl = form["wmUrl"].ToString();
                    string ioPath = this.HttpContext.Server.MapPath(wmUrl);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(target);
                    if (image.Width < 300 || image.Height < 200)
                    {
                        return JsonReturnValue(new { Success = 0, imgurl = "", msg = "添加水印时尺寸不能小于300*200" }, JsonRequestBehavior.AllowGet);
                        
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(wmUrl) && System.IO.File.Exists(ioPath))
                        {
                            data = addWaterMark(image, ioPath, (WaterPositionMode)pos);
                        }
                    }
                }
            }
            #endregion

            string md5 = ToMd5String(data);


            string imgUrl = QiniuService.PutFile("zhujia", md5 + fileExt, data);
            return JsonReturnValue(new { Success = 0, imgurl = imgUrl, msg = "上传成功" }, JsonRequestBehavior.AllowGet);
  

        }

        [HttpPost]
        public ContentResult MuAdd(FormCollection form)
        {
            HttpFileCollectionBase files = Request.Files;

             //最大文件大小f
            int maxSize = 2048000;
            List<MyImageFormat> json = new List<MyImageFormat>();

            if (files.Count > 0)
            {
                try
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (files[i]==null)continue;
                        string fileName = files[i].FileName;
                        string ext = Path.GetExtension(fileName);
                       int size = files[i].ContentLength;

                        if (exts.Contains(ext) && files[i].InputStream != null && files[i].InputStream.Length <= maxSize)
                        {
                            files[i].InputStream.Position = 0;
                            using (Stream stream = files[i].InputStream)
                            {
                                string imgUrl = QiniuService.PutFileStringStream(stream, "zhujia", ext);
                                MyImageFormat item = new MyImageFormat()
                                {
                                    name = fileName,
                                    size = size,
                                    url = imgUrl
                                };
                                json.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return WriteJs(string.Format("UploadFailed('{0}')", ex.Message));
                }
            }
            System.Web.Script.Serialization.JavaScriptSerializer jss=new System.Web.Script.Serialization.JavaScriptSerializer();
            //ResizeSettings 
            // ImageResizer
            string jsonData = jss.Serialize(json);
            return WriteJs(string.Format("UploadComplete({0})",jsonData));
        }
        protected ContentResult WriteJs(string jsContent)
        {
            ViewBag.Url = jsContent;
            return Content("<script type='text/javascript'> function UploadComplete(data) { if (parent.uploadFileHandler.UploadComplete) { parent.uploadFileHandler.UploadComplete(data); } }" + jsContent + "</script>");
        }
        public class MyImageFormat
        {
            public string name { get; set; }
            public int size { get; set; }
            public string url  { get; set; }
        }

        private static string ToMd5String(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashResult = md5.ComputeHash(data);
            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }
       
        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="path">原图片绝对地址</param>
        /// <param name="suiyi">水印文件</param>
        /// <param name="pos">水印位置</param>
        private static byte[] addWaterMark(Image image, string suiyi, WaterPositionMode pos)
        {
            try
            {
                
                Bitmap b = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.White);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
                System.Drawing.Image watermark = new Bitmap(suiyi);
                System.Drawing.Imaging.ImageAttributes imageAttributes = new System.Drawing.Imaging.ImageAttributes();
                System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                System.Drawing.Imaging.ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                float[][] colorMatrixElements = {
             new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
             new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
             new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
             new float[] {0.0f, 0.0f, 0.0f, 1.0f, 0.0f},//设置透明度
             new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
            };
                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix, System.Drawing.Imaging.ColorMatrixFlag.Default, System.Drawing.Imaging.ColorAdjustType.Bitmap);
                int xpos = 0;
                int ypos = 0;
                Point position = SetMarkPoint(pos, watermark, image);
                xpos = position.X; // ((image.Width - watermark.Width) - 50);//水印位置
                ypos = position.Y; //image.Height - watermark.Height - 50;//水印位置

                g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);
                watermark.Dispose();
                imageAttributes.Dispose();

                MemoryStream ms = new MemoryStream();

                b.Save(ms, ImageFormat.Bmp);
                byte[] byteImage = ms.ToArray();
                b.Dispose();
                image.Dispose();
                g.Dispose();
                return byteImage;
            }
            catch (Exception ex)
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
            
        }
        private static Point SetMarkPoint(WaterPositionMode position,Image watermak,Image orImage)
        {
            int x = 0;
            int y = 0;
            switch (position)
            {
                case WaterPositionMode.LeftTop:
                    x = 8;
                    y = 8;
                    break;
                case WaterPositionMode.LeftBottom:
                    x = 8;
                    y = orImage.Height - watermak.Height;
                    break;
                case WaterPositionMode.RightTop:
                    x = orImage.Width * 1 - watermak.Width;
                    y = 8;
                    break;
                case WaterPositionMode.RightBottom:
                    x = orImage.Width - watermak.Width;
                    y = orImage.Height - watermak.Height;
                    break;
                case WaterPositionMode.Center:
                    x = orImage.Width / 3;
                    y = orImage.Height / 2 - watermak.Height / 2;
                    break;
            }
            return new Point(x, y);
        }
 
        public enum WaterPositionMode
        {
            LeftTop=1,
            LeftBottom=2,
            Center=3,
            RightTop=4,
            RightBottom=5
        }
    }
}