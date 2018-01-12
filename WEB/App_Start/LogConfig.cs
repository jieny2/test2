using System.Web;
using System.Web.Mvc;

namespace WEB
{
    public class LogConfig
    {
        public static void RegisterLogs()
        {
            Common.LogHelper.InitConfig();
        }
    }
}