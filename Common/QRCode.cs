using System.Drawing;
using ZXing;
using ZXing.QrCode;

namespace Common
{
    public class QRCode
    {
        public Bitmap GenerateQRCode(int width, int height)
        {
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.CharacterSet = "UTF-8";
            // Extended Channel Interpretation (ECI) 主要用于特殊的字符集。并不是所有的扫描器都支持这种编码。
            options.DisableECI = true;
            // 纠错级别
            // L - 约 7% 纠错能力
            // M - 约 15% 纠错能力
            // Q - 约 25% 纠错能力
            // H - 约 30% 纠错能力
            options.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
            options.Width = 300;
            options.Height = 300;
            options.Margin = 1;
            // options.Hints，更多属性，也可以在这里添加

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;

            Bitmap bmp = writer.Write("嘿嘿");

            return bmp;
        }
    }
}
