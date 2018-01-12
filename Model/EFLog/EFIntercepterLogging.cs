using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Text;

namespace Model
{
    public class EFIntercepterLogging : DbCommandInterceptor
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        public override void ScalarExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ScalarExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                Trace.TraceError("Exception:{1}\r\n--> Error executing command:\r\n{0}", command.CommandText, interceptionContext.Exception.ToString());
            }
            else
            {
                Trace.TraceInformation("\r\nEF的SQL语句\r\n执行时间:{0}毫秒\r\n-->ScalarExecuted.Command:{1}\r\n参数:{2}\r\n", _stopwatch.ElapsedMilliseconds, command.CommandText, GetParameters(command));
            }
            base.ScalarExecuted(command, interceptionContext);
        }

        public override void NonQueryExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void NonQueryExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                Trace.TraceError("Exception:{1}\r\n--> Error executing command:\r\n{0}", command.CommandText, interceptionContext.Exception.ToString());
            }
            else
            {
                Trace.TraceInformation("\r\nEF的SQL语句\r\n执行时间:{0}毫秒\r\n-->NonQueryExecuted.Command:\r\n{1}\r\n参数:{2}\r\n", _stopwatch.ElapsedMilliseconds, command.CommandText, GetParameters(command));
            }
            base.NonQueryExecuted(command, interceptionContext);
        }

        public override void ReaderExecuting(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            _stopwatch.Restart();
        }

        public override void ReaderExecuted(System.Data.Common.DbCommand command, DbCommandInterceptionContext<System.Data.Common.DbDataReader> interceptionContext)
        {
            _stopwatch.Stop();
            if (interceptionContext.Exception != null)
            {
                Trace.TraceError("Exception:{1}\r\n--> Error executing command:\r\n{0}", command.CommandText, interceptionContext.Exception.ToString());
            }
            else
            {
                Trace.TraceInformation("\r\nEF的SQL语句\r\n执行时间:{0}毫秒\r\n-->ReaderExecuted.Command:\r\n{1}\r\n参数:{2}\r\n", _stopwatch.ElapsedMilliseconds, command.CommandText, GetParameters(command));
            }
            base.ReaderExecuted(command, interceptionContext);
        }

        string GetParameters(System.Data.Common.DbCommand command)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("[");
            foreach (System.Data.Common.DbParameter item in command.Parameters)
            {
                parameters.Append(" " + item.ParameterName + " = " + item.Value + " ");
            }
            parameters.Append("]");

            return parameters.ToString();
        }
    }
}