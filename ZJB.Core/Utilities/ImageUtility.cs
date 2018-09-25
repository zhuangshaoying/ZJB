using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Net;

namespace ZJB.Core.Utilities
{
    public static class ImageUtility
    {
        public static Image Mark(Image originalImage, Image waterMark, SlantWatermark slantWatermark = null, TextUnderLogo textUnderLogo = null)
        {
            const int minWidthRate = 2;
            const int minHeightRate = 3;

            if (originalImage.Width / waterMark.Width < minWidthRate || originalImage.Height / waterMark.Height < minHeightRate)
            {
                return new Bitmap(originalImage);
            }

            Image newImage = new Bitmap(originalImage);

            using (Graphics gfx = Graphics.FromImage(newImage))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (slantWatermark != null)
                {
                    AddSlantWatermark(gfx, newImage.Width, newImage.Height, slantWatermark);
                }

                int marginBottom = 20;
                int marginLeft = 20;
                int logoMarginBottom = marginBottom;

                if (textUnderLogo != null)
                {
                    SizeF fontSize = gfx.MeasureString(textUnderLogo.Text, textUnderLogo.Font);

                    logoMarginBottom += (int)fontSize.Height;

                    gfx.DrawString(textUnderLogo.Text, textUnderLogo.Font, textUnderLogo.Brush,
                                   originalImage.Width - fontSize.Width - marginLeft, originalImage.Height - fontSize.Height - marginBottom);
                }

                gfx.DrawImage(waterMark,
                              new Rectangle(originalImage.Width - waterMark.Width - marginLeft,
                                            originalImage.Height - waterMark.Height - logoMarginBottom, waterMark.Width, waterMark.Height),
                              0, 0, waterMark.Width, waterMark.Height, GraphicsUnit.Pixel);
            }

            return newImage;
        }

        public static void Mark(string originImagePath, string watermarkImagePath, string outputFolder, string fileName, SlantWatermark slantWatermark = null, TextUnderLogo textUnderLogo = null)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string newImagePath = Path.Combine(outputFolder, fileName);

            Image newImage;

            using (Image originImage = Image.FromFile(originImagePath)
                , watermarkImage = Image.FromFile(watermarkImagePath))
            {
                newImage = Mark(originImage, watermarkImage, slantWatermark, textUnderLogo);
            }

            //放在这里确保newImagePath与originImagePath相同时能正常保存
            newImage.Save(newImagePath, ImageFormat.Jpeg);
            newImage.Dispose();
        }

        public static void Mark(string originImagePath, string watermarkImagePath, string outputFolder, SlantWatermark slantWatermark = null, TextUnderLogo textUnderLogo = null)
        {
            string fileName = Path.GetFileName(originImagePath);
            Mark(originImagePath, watermarkImagePath, outputFolder, fileName, slantWatermark, textUnderLogo);
        }

        public static Image Resize(Image originalImage, int width, int height)
        {
            Image newImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(newImage))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                gfx.Clear(Color.Transparent);

                gfx.DrawImage(originalImage, new Rectangle(0, 0, width, height),
                              new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                              GraphicsUnit.Pixel);
            }
            return newImage;
        }

        public static void Resize(string originImagePath, string outputFolder, int width, int height)
        {
            string fileName = Path.GetFileName(originImagePath);
            Resize(originImagePath, outputFolder, fileName, width, height);
        }

        public static void Resize(string originImagePath, string outputFolder, string fileName, int width, int height)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string newImagePath = Path.Combine(outputFolder, fileName);
            Image newImage;
            using (Image originImage = Image.FromFile(originImagePath))
            {
                newImage = Resize(originImage, width, height);
            }

            newImage.Save(newImagePath, ImageFormat.Jpeg);
            newImage.Dispose();
        }

        public static void Rotate(Image img)
        {
            PropertyItem[] exif = img.PropertyItems;

            if (exif == null || exif.Length == 0) return;

            byte orientation = 0;
            foreach (PropertyItem i in exif)
            {
                if (i.Id == 274)
                {
                    orientation = i.Value[0];
                    i.Value[0] = 1;
                    img.SetPropertyItem(i);
                }
            }

            switch (orientation)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    break;
            }
            foreach (PropertyItem i in exif)
            {
                if (i.Id == 40962)
                {
                    i.Value = BitConverter.GetBytes(img.Width);
                }
                else if (i.Id == 40963)
                {
                    i.Value = BitConverter.GetBytes(img.Height);
                }
            }
        }

        public static byte[] GetBytes(Image image, ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imageFormat);
            return ms.ToArray();
        }

        public static Image Resize(Image originalImage, ImageSize size)
        {
            int width;
            int height;

            switch (size)
            {
                case ImageSize.Fixed50X37:
                    width = 50;
                    height = 37;
                    break;
                case ImageSize.QuarterSize:
                    width = originalImage.Width / 2;
                    height = originalImage.Height / 2;
                    break;
                case ImageSize.SixteenthSize:
                    width = originalImage.Width / 4;
                    height = originalImage.Height / 4;
                    break;
                case ImageSize.FixedWidth150:
                    width = 150;
                    height = originalImage.Height * width / originalImage.Width;
                    break;
                case ImageSize.FixedWidth320: 
                    width = 320;
                    height = originalImage.Height * width / originalImage.Width;
                    break;
                default:
                    width = originalImage.Width;
                    height = originalImage.Height;
                    break;
            }

            if (width == 0) width = 1;
            if (height == 0) height = 1;

            return Resize(originalImage, width, height);
        }

        public static void Resize(string originImagePath, string outputFolder, ImageSize size)
        {
            string fileName = Path.GetFileName(originImagePath);
            Resize(originImagePath, outputFolder, fileName, size);
        }

        public static void Resize(string originImagePath, string outputFolder, string fileName, ImageSize size)
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            string newImagePath = Path.Combine(outputFolder, fileName);

            Image newImage;

            using (Image originImage = Image.FromFile(originImagePath))
            {
                newImage = Resize(originImage, size);
            }

            newImage.Save(newImagePath, ImageFormat.Jpeg);
            newImage.Dispose();
        }

        public static Image ResizeCrop(Image originalImage, int width, int height)
        {
            int resizeWidth;
            int resizeHeight;
            int x;
            int y;
            if (width * originalImage.Height > height * originalImage.Width)
            {
                resizeWidth = width;
                resizeHeight = (int)(1.0 * originalImage.Height * width / originalImage.Width);
                x = 0;
                y = (resizeHeight - height) / 2;
            }
            else
            {
                resizeHeight = height;
                resizeWidth = (int)(1.0 * originalImage.Width * height / originalImage.Height);
                y = 0;
                x = (resizeWidth - width) / 2;
            }

            using (Image resizeImage = Resize(originalImage, resizeWidth, resizeHeight))
            {
                return Crop(resizeImage, x, y, width, height);
            }
        }

        public static Image Crop(Image originalImage, int x, int y, int width, int height)
        {
            Image newImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(newImage))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                gfx.Clear(Color.Transparent);

                gfx.DrawImage(originalImage, new Rectangle(0, -0, width, height),
                              new Rectangle(x, y, width, height),
                              GraphicsUnit.Pixel);
            }
            return newImage;
        }

        private static void AddSlantWatermark(Graphics gfx, int width, int height, SlantWatermark watermark)
        {
            int watermarkWidth = Convert.ToInt32(height / 2.0 + Math.Sqrt(3) * width / 2);

            int watermarkHeight = Convert.ToInt32(width / 2.0 + Math.Sqrt(3) * height / 2);

            int centerX = Convert.ToInt32(width / 2);

            int centerY = Convert.ToInt32(height / 2);

            Rectangle rectangle = new Rectangle(centerX - watermarkWidth / 2, centerY - watermarkHeight / 2, watermarkWidth, watermarkHeight);

            gfx.TranslateTransform(centerX, centerY);

            gfx.RotateTransform(-30);

            gfx.TranslateTransform(-centerX, -centerY);

            WriteStringToReangle(gfx, watermark, rectangle);

            gfx.ResetTransform();
        }

        private static void WriteStringToReangle(Graphics gfx, SlantWatermark watermark, Rectangle rectangle)
        {
            string text = GetRepeatString(gfx, watermark.Text, watermark.Spacing, watermark.Font, rectangle.Width);
            string alternateText = text.Substring(text.Length - watermark.Text.Length / 2) + watermark.Spacing + text;

            int centerY = rectangle.Height / 2 + rectangle.Y;

            gfx.DrawString(text, watermark.Font, watermark.Brush, rectangle.X, centerY);

            bool alternate = true;

            for (int pos = centerY - watermark.Indent; pos > rectangle.Y; pos -= watermark.Indent, alternate = !alternate)
            {
                gfx.DrawString(alternate ? alternateText : text, watermark.Font, watermark.Brush, rectangle.X, pos);
                gfx.DrawString(alternate ? alternateText : text, watermark.Font, watermark.Brush, rectangle.X, centerY * 2 - pos);
            }
        }

        private static string GetRepeatString(Graphics gfx, string text, string spacing, Font font, int width)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(text);

            while (gfx.MeasureString(buffer.ToString(), font).Width < width)
            {
                buffer.AppendFormat("{0}{1}", spacing, text);
            }
            return buffer.ToString();
        }
        /// <summary>
        /// 从图片地址下载图片到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">图片本地磁盘地址</param>
        /// <param name="Url">图片网址</param>
        /// <returns></returns>
        public static bool SavePhotoFromUrl(string FileName, string Url)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    Value = SaveBinaryFile(response, FileName);
                }
            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return Value;
        }
        /// <summary>
        /// Save a binary file to disk.
        /// </summary>
        /// <param name="response">The response used to save the file</param>
        // 将二进制文件保存到磁盘
        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];
            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }
    }

    public enum ImageSize
    {
        /// <summary>
        /// 四分之一大小（宽高均折半）
        /// </summary>
        QuarterSize,
        /// <summary>
        /// 十六分之一大小（宽高均为四分一）
        /// </summary>
        SixteenthSize,
        /// <summary>
        /// 固定50x37大小
        /// </summary>
        Fixed50X37,
        /// <summary>
        /// 固定宽度150
        /// </summary>
        FixedWidth150,
        /// <summary>
        /// A后台群组背景、个人主页背景
        /// </summary>
        FixedWidth320
    }

    public class SlantWatermark
    {
        public string Text { get; set; }
        public string Spacing { get; set; }
        public int Indent { get; set; }
        public Font Font { get; set; }
        public Brush Brush { get; set; }

        public SlantWatermark(string text, string spacing, int indent, Font font, Brush brush)
        {
            Text = text;
            Spacing = spacing;
            Indent = indent;
            Font = font;
            Brush = brush;
        }

        public SlantWatermark (string text)
        {
            Text = text;
            Spacing = "        ";
            Indent = 300;
            Font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            Brush =   new SolidBrush(Color.FromArgb(80, 211, 211, 211));
        }

        public static SlantWatermark ZJBWatermark
        {
            get
            {
                return new SlantWatermark("住家帮        zhujia001.com");
            }
        }

        public static SlantWatermark OtherWatermark
        {
            get
            {
                return new SlantWatermark("房联网        66HOUSE.COM");
            }
        }
    }

    public class TextUnderLogo
    {
        public string Text { get; set; }
        public Font Font { get; set; }
        public Brush Brush { get; set; }

        public TextUnderLogo(string text, Font font, Brush brush)
        {
            Text = text;
            Font = font;
            Brush = brush;
        }

        public TextUnderLogo(string text)
        {
            Font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            Brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            Text = text;
        }
    }
}
