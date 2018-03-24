using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danfoss.Core.Utilities
{
    /// <summary>
    ///  二维码 帮助类
    /// </summary>
    public class QrCodeHelper
    {
        /// <summary>
        /// 生成二维码  
        /// </summary>
        /// <param name="content"></param>
        /// <param name="level"></param>
        /// <param name="pixelsPerModule"></param>
        /// <returns></returns>
        public static MemoryStream RenderQrCode(string content, string level,int pixelsPerModule)
        {
            var stream = new MemoryStream();
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, eccLevel))
                {
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    {

                        var qRCode = qrCode.GetGraphic(pixelsPerModule);

                        //pictureBoxQRCode.Size = new System.Drawing.Size(pictureBoxQRCode.Width, pictureBoxQRCode.Height);
                        //Set the SizeMode to center the image.
                        qRCode.Save(stream, ImageFormat.Jpeg);
                    }
                }
            }
            return stream;
        }
    }
}
