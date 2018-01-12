using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    public void swap(ref string a, ref string b)
    {
        string tmp = "";
        tmp = a;
        a = b;
        b = tmp;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //xxx.InnerText = "HelloWorld";

        DateTime dt = new DateTime(2018, 1, 20);
        DateTime? dt2 = dt;
        //xxx.InnerText = dt.GetPreWeekMonday().ToString(); 
        //xxx.InnerText = dt.GetPreWeekSunday().ToString() + " " + dt2.GetCurMonthLast().ToString();
        string s = "abc";
        string m = "efg";
        swap(ref s, ref m);

        ;
        xxx.InnerText = dt.SetTime(22, 33, 23).ToString();
        //xxx.InnerText = s.ConcatString("3443");
        //xxx.InnerText = dt.ToShortDateString(); // 2018/1/8
        //xxx.InnerText = dt.ToLongTimeString(); // 13:34:10
        //xxx.InnerText = dt.ToString(); // 13:34:10


        //string扩展测试
        //string str = "1234567890";
        //this.ceshi.InnerText = str.Right(3);
        //string str2 = "";
        //xxx.InnerText = str2.ToDateTime().ToString("yyyy-MM-dd");

        // 随机数测试
        //RandomNum rn = new RandomNum(0);
        //int[] arr = rn.GetDifferentInt(5, 10);
        //xxx.InnerText = string.Join("-", arr);

        // 二维码测试
        //QRCode qrCode = new QRCode();
        //Bitmap bmp = qrCode.GenerateQRCode(1, 1);
        //Response.Clear();
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    bmp.Save(ms, ImageFormat.Png);

        //    Response.ContentType = "image/png";
        //    Response.BinaryWrite(ms.ToArray());
        //}
        //Response.End();

        // 验证码测试
        //CaptchaOptions co = new CaptchaOptions()
        //{
        //    TextLength = 5,
        //    Width = 160,
        //    FontWarp = Level.High,
        //    BackgroundNoise = Level.Low,
        //    LineNoise = Level.High
        //};
        //Captcha a = new Captcha();
        //Captcha captcha = new Captcha(co);
        //this.Context.Session["captcha"] = captcha.Text;
        //Response.Clear();
        //using (var bmp = captcha.RenderImage())
        //{
        //    bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
        //}
        //Response.Cache.SetNoStore();
        //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
        //Response.ContentType = "image/jpeg";
        //Response.StatusCode = 200;
        //Response.StatusDescription = "OK";
        //Response.End();
    }
}