using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Business
{
    public class GeneraCodigoQR
    {
        public byte[] ConvertirQR(string mensaje)
        {
            byte[] byteImage;
            string code = mensaje;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
          
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
             imgBarCode.Height = 150;
             imgBarCode.Width = 150;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
                //plBarCode.Controls.Add(imgBarCode);
            }

            return byteImage;
        }
    }
}