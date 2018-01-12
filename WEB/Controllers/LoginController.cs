using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// 验证码图片，文本存在Session中
        /// </summary>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult GetCaptcha()
        {
            CaptchaOptions co = new CaptchaOptions()
            {
                TextLength = 5,
                Width = 160,
                FontWarp = Level.High,
                BackgroundNoise = Level.Low,
                LineNoise = Level.High
            };
            Captcha captcha = new Captcha(co);
            Session["captcha"] = captcha.Text;

            byte[] res = null;
            captcha.RenderImage(out res); // 渲染图片返回字节数组

            return File(res, "image/jpeg");
        }
    }
}