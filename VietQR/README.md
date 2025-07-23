# VietQR - Thư viện tạo & giải mã QR thanh toán Việt Nam (C#/.NET 6+)

**Tác giả:** thanhtunguet

**Mô tả:**

Thư viện tạo mã VietQR cho C# từ 6.0. Là bản fork dựa trên các thư viện mã nguồn có sẵn cho Winform và .NET 3.5:

- [xuannghia/vietnam-qr-pay](https://github.com/xuannghia/vietnam-qr-pay)
- [nick-hoang/vietnam-qr-pay-csharp](https://github.com/nick-hoang/vietnam-qr-pay-csharp)

Xin gửi lời cảm ơn tới các tác giả.

---

## Giới thiệu

**VietQR** là thư viện C# hỗ trợ tạo và giải mã mã QR thanh toán theo chuẩn VietQR, VNPayQR, QR đa năng (MoMo, ZaloPay) cho các ứng dụng .NET hiện đại (>= .NET 6, cross-platform: Windows, Linux, macOS).

- Tạo mã QR chuyển khoản ngân hàng, QR động, QR đa năng ví điện tử.
- Giải mã nội dung QR, trích xuất thông tin tài khoản, số tiền, nội dung chuyển khoản, merchant...
- Dễ dàng tích hợp vào web API, desktop, console app.

---

## Cài đặt

Thêm vào project .NET 6+ của bạn:

```sh
dotnet add package QRCoder --version 1.4.3
```

Thêm tham chiếu project nếu dùng source:
```xml
<ProjectReference Include="../VietQR/VietQR.csproj" />
```

---

## Sử dụng nhanh

### 1. Tạo mã QR VietQR (chuyển khoản ngân hàng)

```csharp
using VietQR;

// Khởi tạo QR chuyển khoản Vietcombank
var qrPay = QRPay.InitVietQR(
    bankBin: BankApp.BanksObject[BankKey.VIETCOMBANK].bin,
    bankNumber: "0491000147829",
    amount: "20000",
    purpose: "Ủng hộ VietQR"
);
var content = qrPay.Build(); // Chuỗi nội dung QR

// Tạo ảnh PNG QR (cross-platform, dùng cho web, API, console...)
var pngBytes = QRCodeHelper.TaoVietQRCodePng(content);
File.WriteAllBytes("vietqr.png", pngBytes);
```

### 2. Tạo QR động, QR MoMo, ZaloPay, VNPayQR

#### QR động (số tiền, nội dung động)
```csharp
var qrPay = QRPay.InitVietQR(
    bankBin: BankApp.BanksObject[BankKey.ACB].bin,
    bankNumber: "257678859",
    amount: "10000",
    purpose: "Chuyen tien"
);
var content = qrPay.Build();
var pngBytes = QRCodeHelper.TaoVietQRCodePng(content);
```

#### QR MoMo
```csharp
var accountNumber = "99MM24011M34875080";
var momoQR = QRPay.InitVietQR(
    bankBin: BankApp.BanksObject[BankKey.BANVIET].bin,
    bankNumber: accountNumber
);
momoQR.additionalData.reference = "MOMOW2W" + accountNumber.Substring(10);
momoQR.SetUnreservedField("80", "046"); // 3 số cuối SĐT
var content = momoQR.Build();
```

#### QR ZaloPay
```csharp
var accountNumber = "99ZP24009M07248267";
var zaloPayQR = QRPay.InitVietQR(
    bankBin: BankApp.BanksObject[BankKey.BANVIET].bin,
    bankNumber: accountNumber
);
var content = zaloPayQR.Build();
```

#### QR VNPay
```csharp
var qrPay = QRPay.InitVNPayQR(
    merchantId: "0102154778",
    merchantName: "TUGIACOMPANY",
    store: "TU GIA COMPUTER",
    terminal: "TUGIACO1"
);
var content = qrPay.Build();
```

---

## Giải mã (decode) mã QR

```csharp
using VietQR;

var qrContent = "00020101021238530010A0000007270123000697041601092576788590208QRIBFTTA5303704540410005802VN62150811Chuyen tien6304BBB8";
var qrPay = new QRPay(qrContent);
Console.WriteLine(qrPay.isValid); // true
Console.WriteLine(qrPay.provider.name); // VIETQR
Console.WriteLine(qrPay.consumer.bankBin); // 970416
Console.WriteLine(qrPay.consumer.bankNumber); // 257678859
Console.WriteLine(qrPay.amount); // 1000
Console.WriteLine(qrPay.additionalData.purpose); // Chuyen tien
```

---

## API các class chính

### QRPay
| Thuộc tính     | Kiểu           | Ý nghĩa                                |
|----------------|----------------|----------------------------------------|
| isValid        | bool           | Mã QR hợp lệ?                          |
| initMethod     | string         | 11: QR tĩnh, 12: QR động               |
| provider       | Provider       | Thông tin nhà cung cấp (VietQR, VNPay) |
| merchant       | Merchant       | Thông tin merchant (nếu có)            |
| consumer       | Consumer       | Thông tin người nhận                   |
| amount         | string         | Số tiền                                |
| currency       | string         | Mã tiền tệ (704 = VND)                 |
| nation         | string         | Mã quốc gia                            |
| additionalData | AdditionalData | Thông tin bổ sung                      |
| crc            | string         | CRC kiểm tra                           |
| build()        | method         | Tạo lại nội dung QR                    |

### Provider
| Thuộc tính | Kiểu   | Ý nghĩa          |
|------------|--------|------------------|
| guid       | string | Mã định danh     |
| name       | string | Tên nhà cung cấp |

### Merchant
| Thuộc tính | Kiểu   | Ý nghĩa      |
|------------|--------|--------------|
| id         | string | Mã merchant  |
| name       | string | Tên merchant |

### Consumer
| Thuộc tính | Kiểu   | Ý nghĩa      |
|------------|--------|--------------|
| bankBin    | string | Mã ngân hàng |
| bankNumber | string | Số tài khoản |

### AdditionalData
| Thuộc tính    | Kiểu   | Ý nghĩa                  |
|---------------|--------|--------------------------|
| billNumber    | string | Số hóa đơn               |
| mobileNumber  | string | Số điện thoại            |
| store         | string | Tên cửa hàng             |
| loyaltyNumber | string | Mã khách hàng thân thiết |
| reference     | string | Mã tham chiếu            |
| customerLabel | string | Mã khách hàng            |
| terminal      | string | Tên điểm bán             |
| purpose       | string | Nội dung giao dịch       |

---

## Đóng góp & Giấy phép

- Đóng góp: PR, issue, góp ý đều được hoan nghênh!
- License: MIT
- Dựa trên các dự án nguồn mở:
  - [xuannghia/vietnam-qr-pay](https://github.com/xuannghia/vietnam-qr-pay)
  - [nick-hoang/vietnam-qr-pay-csharp](https://github.com/nick-hoang/vietnam-qr-pay-csharp)

---

**Mọi thắc mắc, góp ý, vui lòng tạo issue hoặc liên hệ qua GitHub của [thanhtunguet](https://github.com/thanhtunguet/csharp-vietqr)!**
