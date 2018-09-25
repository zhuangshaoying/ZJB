using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Collections;

namespace ZJB.WX.Common
{
    public class QrCodes : System.Web.Services.WebService
    {

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="data">文本内容</param>
        /// <param name="encodeing">文本类型</param>
        /// <param name="correction">水平位置</param>
        /// <param name="scale">比例</param>
        /// <param name="version">version</param>
        /// <returns></returns>
        public static Image CreateQRCode(string data, QRCodeEncoder.ENCODE_MODE encodeing = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION correction = QRCodeEncoder.ERROR_CORRECTION.M, int version = 7, int scale = 4)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder
            {
                QRCodeEncodeMode = encodeing,
                QRCodeErrorCorrect = correction,
                QRCodeScale = scale,
                QRCodeVersion = version
            };
            Image image = qrCodeEncoder.Encode(data);
            return image;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="data">内容</param>
        /// <param name="version">version</param>
        /// <param name="scale">scale</param>
        /// <returns></returns>
        public Image CreateQRCode(string data, int version = 7, int scale = 4)
        {
            return CreateQRCode(data, QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION.M, version, scale);
        }
        public Image CreateQRCode(string data)
        {
            return CreateQRCode(data, QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION.M, 4, 7);
        }


        public Image CreateQRCodeLogo(string data, string logoPath, QRCodeEncoder.ENCODE_MODE encodeing = QRCodeEncoder.ENCODE_MODE.BYTE, QRCodeEncoder.ERROR_CORRECTION correction = QRCodeEncoder.ERROR_CORRECTION.M, int version = 7, int scale = 4)
        {
            Image img = CreateQRCode(data, encodeing, correction, scale, version);
            if (!File.Exists(logoPath)) return img;
            Image waterimg = Image.FromFile(logoPath);
            //添加水印
            Graphics g = Graphics.FromImage(img);
            //获取水印位置设置
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;
            x = img.Width / 2 - waterimg.Width / 2;
            y = img.Height / 2 - waterimg.Height / 2;
            loca.Add(x);
            loca.Add(y);

            g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
            //释放资源
            waterimg.Dispose();
            g.Dispose();

            //string newpath = "D:\\ss.png";
            //img.Save(newpath);
            //img.Dispose();

            return img;
        }
        public Image CreateQRCodeLogo(string data, string logoPath, int version = 7, int scale = 4)
        {
            return CreateQRCodeLogo(data, logoPath, QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC, QRCodeEncoder.ERROR_CORRECTION.M, version, scale);
        }
        public Image CreateQRCodeLogo(string data, string logoPath)
        {
            return CreateQRCodeLogo(data, logoPath, QRCodeEncoder.ENCODE_MODE.ALPHA_NUMERIC, QRCodeEncoder.ERROR_CORRECTION.M, 4, 7);
        }
    }
}