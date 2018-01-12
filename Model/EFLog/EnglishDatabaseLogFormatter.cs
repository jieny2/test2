using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Threading.Tasks;

namespace Model
{
    public class EnglishDatabaseLogFormatter : DatabaseLogFormatter
    {
        public EnglishDatabaseLogFormatter(DbContext context, Action<string> writeAction) : base(context, writeAction)
        {

        }

        /// <summary>
        /// 打开了连接
        /// </summary>
        public override void Opened(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        {
            Write(Environment.NewLine);
            base.Opened(connection, interceptionContext);
        }

        ///// <summary>
        ///// 关闭了连接
        ///// </summary>
        //public override void Closed(DbConnection connection, DbConnectionInterceptionContext interceptionContext)
        //{
        //    base.Closed(connection, interceptionContext);
        //    Write(Environment.NewLine);
        //}

        ///// <summary>
        ///// 启动了事务
        ///// </summary>
        //public override void BeganTransaction(DbConnection connection, BeginTransactionInterceptionContext interceptionContext)
        //{
        //    base.BeganTransaction(connection, interceptionContext);
        //    Write(Environment.NewLine);
        //}

        ///// <summary>
        ///// 提交了事务
        ///// </summary>
        //public override void Committed(DbTransaction transaction, DbTransactionInterceptionContext interceptionContext)
        //{
        //    base.Committed(transaction, interceptionContext);
        //    Write(Environment.NewLine);
        //}

        ///// <summary>
        ///// 回滚了事务
        ///// </summary>
        //public override void RolledBack(DbTransaction transaction, DbTransactionInterceptionContext interceptionContext)
        //{
        //    base.RolledBack(transaction, interceptionContext);
        //    Write(Environment.NewLine);
        //}

        ///// <summary>
        ///// 执行
        ///// </summary>
        //public override void Executing<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        //{
        //    base.Executing<TResult>(command, interceptionContext);
        //    Write(Environment.NewLine);
        //}

        //public override void LogCommand<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        //{
        //    Write("【SQL语句】" + Environment.NewLine);
        //    base.LogCommand<TResult>(command, interceptionContext);
        //}

        public override void LogResult<TResult>(DbCommand command, DbCommandInterceptionContext<TResult> interceptionContext)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            if (interceptionContext == null)
            {
                throw new ArgumentNullException("interceptionContext");
            }

            if (interceptionContext.Exception != null)
            {
                this.Write("CommandLogFailed in " + Stopwatch.ElapsedMilliseconds + " ms with error [ " + interceptionContext.Exception.Message + " ]" + Environment.NewLine);
                // EF6.2.0才可以用下面这种方式
                //this.Write("CommandLogFailed in " + GetStopwatch(interceptionContext).ElapsedMilliseconds + " ms with error [ " + interceptionContext.Exception.Message + " ]" + Environment.NewLine);
            }
            else if (interceptionContext.TaskStatus.HasFlag(TaskStatus.Canceled))
            {
                this.Write("CommandLogCanceled in " + Stopwatch.ElapsedMilliseconds + " ms" + Environment.NewLine);
                // EF6.2.0才可以用下面这种方式
                //this.Write("CommandLogCanceled in " + GetStopwatch(interceptionContext).ElapsedMilliseconds + " ms" + Environment.NewLine);
            }
            else
            {
                TResult result = interceptionContext.Result;
                string str = (result == null) ? "null" : ((result is DbDataReader) ? result.GetType().Name : result.ToString());
                this.Write("CommandLogComplete in " + Stopwatch.ElapsedMilliseconds + " ms with result [ " + result + " ]" + Environment.NewLine);
                // EF6.2.0才可以用下面这种方式
                //this.Write("CommandLogComplete in " + GetStopwatch(interceptionContext).ElapsedMilliseconds + " ms with result [ " + result + " ]" + Environment.NewLine);
            }
        }
    }
}
