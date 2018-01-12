using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            BLL.TestBLL testBLL = new BLL.TestBLL();
            testBLL.Test();

            LogHelper.WriteLog(LogLevel.Warn, "老子警告你");
            LogHelper.WriteLog(LogLevel.Debug, "xxx");
            LogHelper.WriteLog(LogLevel.Error, "出错了");

            //Console.WriteLine();
            //string sql = "SELECT * FROM dbo.T_Sys_User;";
            //DataTable dt = DAL.SqlServerHelper.ExecuteDataTable(sql);
            //var xx = from item in dt.ToList<XXX.Model.T_Sys_User>() where item.ID < 3 select item;
            //var yy = (from item in dt.AsEnumerable()
            //          where item.Field<int>("ID") == 1
            //          select new
            //          {
            //              UserID = item.Field<int>("ID"),
            //              UserName = item.Field<string>("Name")
            //          }).ToList();

            //DataView dv = dt.DefaultView;
            //// 通过RowFilter属性设置DataView过滤信息,只需要ID大于20岁的记录  
            //dv.RowFilter = "ID > 20";
            //// 设置RowFilter为null或空字符串，清除过滤信息，二选一  
            //dv.RowFilter = string.Empty;
            //dv.RowFilter = null;
            //// 通过Sort属性设置DataView排序信息  
            //dv.Sort = "ID ASC, Name DESC";
            //dv.Sort = string.Empty;
            //dv.Sort = null;

            //DataTable partDataTable = dv.ToTable(false, "ID", "Name"); // 默认为不去重

            //var str1 = dt.ToArrayInt("CreateBy");
            //var str2 = dt.ToArrayString("Name");

            //return Content(string.Join(",", str1) + "***" + string.Join("-", str2));


            return Content("HelloWorld");
            //return View();
        }
    }
}