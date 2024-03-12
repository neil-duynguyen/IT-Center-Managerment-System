using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace KidProEdu.Application.Utils
{
    public class QRCodeUtility
    {
        public string GenerateQRCode(string content)
        {
            QRCodeWriter qrCodeWriter = new QRCodeWriter();
            BitMatrix bitMatrix = qrCodeWriter.encode(content, BarcodeFormat.QR_CODE, 300, 300);

            // Tạo hình ảnh Bitmap từ BitMatrix
            Bitmap bitmap = new Bitmap(bitMatrix.Width, bitMatrix.Height, PixelFormat.Format32bppArgb);
            for (int x = 0; x < bitMatrix.Width; x++)
            {
                for (int y = 0; y < bitMatrix.Height; y++)
                {
                    bitmap.SetPixel(x, y, bitMatrix[x, y] ? Color.Black : Color.White);
                }
            }

            // Chuyển đổi hình ảnh thành chuỗi base64
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                return "data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
        }
    }
}
