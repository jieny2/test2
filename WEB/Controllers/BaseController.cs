using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Base控制器下的Action之Index
        /// </summary>
        public ActionResult Index()
        {
            //TODO: 要做的事情，视图→任务列表
            return View();
        }
    }
}