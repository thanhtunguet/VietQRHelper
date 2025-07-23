using QRCoder;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace VietQRHelper
{
    public class QRCodeHelper
    {
        public static byte[] TaoVietQRCodePng(string input)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q))
            {
                var qrCode = new BitmapByteQRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
        }

        // Hàm tạo QR code PNG và chèn logo vào giữa (cross-platform)
        public static byte[] TaoVietQRCodePngWithLogo(string input, string logoPath)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q))
            {
                var qrCode = new BitmapByteQRCode(qrCodeData);
                var qrBytes = qrCode.GetGraphic(20);

                using (var qrImage = Image.Load<Rgba32>(qrBytes))
                using (var logo = Image.Load<Rgba32>(logoPath))
                {
                    // Resize logo (20% chiều rộng QR)
                    int logoSize = qrImage.Width / 5;
                    logo.Mutate(x => x.Resize(logoSize, logoSize));

                    // Vị trí đặt logo vào giữa
                    int x = (qrImage.Width - logo.Width) / 2;
                    int y = (qrImage.Height - logo.Height) / 2;

                    // Vẽ logo lên QR
                    qrImage.Mutate(xc => xc.DrawImage(logo, new Point(x, y), 1f));

                    using (var ms = new MemoryStream())
                    {
                        qrImage.SaveAsPng(ms);
                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
