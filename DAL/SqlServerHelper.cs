using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Common;

namespace XXX.DAL
{
    /// <summary>
    /// 使用ADO.NET封装对数据库操作的代码（SQL Server数据库专用，均为静态方法）
    /// </summary>
    /// <author>xyb_jieny</author>
    /// <createdate>2017-12-21</createdate>
    /// <remarks>参考自微软PetShop（PetShop.DBUtility.SqlHelper类）</remarks>
    public static class SqlServerHelper
    {
        // 数据库连接字符串(通过Web.config来配置)
        // 添加引用【System.configuration】和using System.Configuration;
        public static readonly string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        static SqlServerHelper()
        {
            if (ConfigurationManager.AppSettings["IsEncryptConnectionString"] == "true")
            {
                connectionString = Decrypt.DESDecrypt(ConfigurationManager.AppSettings["CS"], Encrypt.key);
            }
        }

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL，返回受影响的行数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="cmdType">cmdText的类型：1（CommandType.Text）为SQL文本命令，2（CommandType.StoredProcedure）为存储过程的名称，512（CommandType.TableDirect）为表的名称</param>
        /// <param name="cmdText">文本</param>
        /// <param name="cmdParms">参数列表</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                return val;
            }
        }

        /// <summary>
        /// 执行SQL，返回受影响的行数（使用默认的连接字符串）
        /// </summary>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteNonQuery(connectionString, cmdType, cmdText, cmdParms);
        }

        /// <summary>
        /// 执行SQL，返回受影响的行数（使用默认的连接字符串且CommandType为SQL文本命令）
        /// </summary>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmdText, cmdParms);
        }

        /// <summary>
        /// 执行SQL，返回受影响的行数（调用此方法，要注意SqlConnection对象的关闭）
        /// </summary>
        public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            return val;
        }

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            return val;
        }
        #endregion

        #region ExecuteScalar
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                return val;
            }
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(connectionString, cmdType, cmdText, cmdParms);
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(connectionString, CommandType.Text, cmdText, cmdParms);
        }

        /// <summary>
        /// （调用此方法，要注意SqlConnection对象的关闭）
        /// </summary>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();

            return val;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// （调用此方法，要注意SqlDataReader对象的关闭）
        /// </summary>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                // CommandBehavior.CloseConnection这个枚举参数，表示将来使用完毕SqlDataReader后，在关闭的同时，SqlDataReader内部会将关联的Connection对象也关闭掉
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();

                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// （调用此方法，要注意SqlDataReader对象的关闭）
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(connectionString, CommandType.Text, cmdText, cmdParms);
        }
        #endregion

        #region ExecuteDataSet
        public static DataSet ExecuteDataSet(string connectionString, string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdText, connectionString))
            {
                if (cmdParms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(cmdParms);
                }
                if (timeout != null)
                {
                    sda.SelectCommand.CommandTimeout = timeout.Value;
                }
                sda.Fill(ds);
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            return ExecuteDataSet(connectionString, cmdText, timeout, cmdParms);
        }

        public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataSet(connectionString, cmdText, null, cmdParms);
        }

        /// <summary>
        /// （调用此方法，要注意SqlConnection对象的关闭）
        /// </summary>
        public static DataSet ExecuteDataSet(SqlConnection conn, string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdText, conn))
            {
                if (cmdParms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(cmdParms);
                }
                if (timeout != null)
                {
                    sda.SelectCommand.CommandTimeout = timeout.Value;
                }
                sda.Fill(ds);
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataSet(conn, cmdText, null, cmdParms);
        }
        #endregion

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(string connectionString, string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdText, connectionString))
            {
                if (cmdParms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(cmdParms);
                }
                if (timeout != null)
                {
                    sda.SelectCommand.CommandTimeout = timeout.Value;
                }
                sda.Fill(dt);
            }

            return dt;
        }

        public static DataTable ExecuteDataTable(string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            return ExecuteDataTable(connectionString, cmdText, timeout, cmdParms);
        }

        public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataTable(connectionString, cmdText, null, cmdParms);
        }

        /// <summary>
        /// （调用此方法，要注意SqlConnection对象的关闭）
        /// </summary>
        public static DataTable ExecuteDataTable(SqlConnection conn, string cmdText, int? timeout, params SqlParameter[] cmdParms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdText, conn))
            {
                if (cmdParms != null)
                {
                    sda.SelectCommand.Parameters.AddRange(cmdParms);
                }
                if (timeout != null)
                {
                    sda.SelectCommand.CommandTimeout = timeout.Value;
                }
                sda.Fill(dt);
            }

            return dt;
        }

        public static DataTable ExecuteDataTable(SqlConnection conn, string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataTable(conn, cmdText, null, cmdParms);
        }
        #endregion

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
