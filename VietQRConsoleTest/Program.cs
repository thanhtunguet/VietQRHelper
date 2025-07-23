using System;
using System.IO;
using VietQRHelper;

class Program
{
    static void Main(string[] args)
    {
        // Example: Generate a VietQR code for a test account
        var qrPay = QRPay.InitVietQR(
            bankBin: BankApp.BanksObject[BankKey.VIETCOMBANK].bin,
            bankNumber: "0491000147829",
            amount: "25000",
            purpose: "Test cross-platform console app"
        );
        var content = qrPay.Build();
        var pngBytes = QRCodeHelper.TaoVietQRCodePngWithLogo(content, "vietqr.png");
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "vietqr_with_logo.png");
        File.WriteAllBytes(outputPath, pngBytes);
        Console.WriteLine($"QR code PNG with logo saved to: {outputPath}");
    }
}
