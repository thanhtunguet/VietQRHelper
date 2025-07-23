using QRCoder;
using System.IO;

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
    }
}
