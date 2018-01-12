using System;
using System.Text;

namespace Common
{
    public class LogHelper
    {
        public static void InitConfig()
        {
            // log4net.xml配置文件要放在网站根目录下，一般和Web.config一起
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"log4net.xml"));
            #region log4net.config方式
            // log4net.config文件的【生成操作】属性的值要修改为【嵌入的资源】
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var xml = assembly.GetManifestResourceStream("XXX.Common.Log.log4net.config");
            //log4net.Config.XmlConfigurator.Configure(xml); 
            #endregion
        }

        public static void ShutDown()
        {
            log4net.LogManager.Shutdown();
        }

        #region WriteLog（无Exception）
        public static void WriteLog(string msg)
        {
            WriteLog(LogLevel.Debug, msg);
        }

        public static void WriteLog(LogLevel logLevel, string msg)
        {
            WriteLog(logLevel, msg, null, null, null);
        }

        public static void WriteLog(LogLevel logLevel, string msg, string loggerName)
        {
            WriteLog(logLevel, msg, null, loggerName, null);
        }

        public static void WriteLog(LogLevel logLevel, string msg, Type type)
        {
            WriteLog(logLevel, msg, null, null, type);
        }
        #endregion

        #region WriteLog（有Exception）
        public static void WriteLog(LogLevel logLevel, string msg, Exception ex)
        {
            WriteLog(logLevel, msg, ex, null, null);
        }

        public static void WriteLog(LogLevel logLevel, string msg, Exception ex, string loggerName)
        {
            WriteLog(logLevel, msg, ex, loggerName, null);
        }

        public static void WriteLog(LogLevel logLevel, string msg, Exception ex, Type type)
        {
            WriteLog(logLevel, msg, ex, null, type);
        }
        #endregion

        // 如果loggerName非null且非""，则type不起作用
        private static void WriteLog(LogLevel logLevel, string msg, Exception ex, string loggerName, Type type)
        {
            log4net.ILog log = null;
            if (loggerName.IsNotNullOrEmpty())
            {
                log = log4net.LogManager.GetLogger(loggerName); // 如果找不到对应的logger，则用的是root
            }
            else if (type != null)
            {
                log = log4net.LogManager.GetLogger(type); // 用的是root
            }
            else
            {
                log = log4net.LogManager.GetLogger("root");
            }

            if (log != null)
            {
                if (ex == null)
                {
                    switch (logLevel)
                    {
                        case LogLevel.Debug:
                            log.Debug(msg);
                            break;
                        case LogLevel.Info:
                            log.Info(msg);
                            break;
                        case LogLevel.Warn:
                            log.Warn(msg);
                            break;
                        case LogLevel.Error:
                            log.Error(msg);
                            break;
                        case LogLevel.Fatal:
                            log.Fatal(msg);
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    switch (logLevel)
                    {
                        case LogLevel.Debug:
                            log.Debug(msg, ex);
                            break;
                        case LogLevel.Info:
                            log.Info(msg, ex);
                            break;
                        case LogLevel.Warn:
                            log.Warn(msg, ex);
                            break;
                        case LogLevel.Error:
                            log.Error(msg, ex);
                            break;
                        case LogLevel.Fatal:
                            log.Fatal(msg, ex);
                            break;
                        default:
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// 利用Action委托封装log4net对方法的处理方法
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <param name="msg">部分日志信息</param>
        /// <param name="isThrow">异常处理方式，是否抛出</param>
        /// <param name="tryHandle">调试/运行代码</param>
        /// <param name="catchHandle">异常处理方法</param>
        /// <param name="finallyHandle">最终处理方法</param>
        public static void Logger(log4net.ILog log, string msg, bool isThrow, Action tryHandle, Action<Exception> catchHandle = null, Action finallyHandle = null)
        {
            try
            {
                log.Debug(msg);
                tryHandle();
            }
            catch (Exception ex)
            {
                log.Error(msg + "失败", ex);
                if (catchHandle != null)
                {
                    catchHandle(ex);
                }
                if (isThrow == true)
                {
                    throw ex;
                }
            }
            finally
            {
                if (finallyHandle != null)
                {
                    finallyHandle();
                }
            }
        }
    }
}
