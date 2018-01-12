using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Common;

namespace XXX.DAL
{
    public class SqlHelper
    {
        private string connectionString;

        public SqlHelper()
        {
            // 数据库连接字符串(通过Web.config来配置)
            // 添加引用【System.configuration】和using System.Configuration;
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public SqlHelper(bool isEncrypt)
        {
            connectionString = Decrypt.DESDecrypt(ConfigurationManager.AppSettings["CS"], Encrypt.key);
        }

        // 设置连接字符串
        public void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #region ExecuteNonQuery
        #region 非静态
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
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
        #endregion
        #endregion

        #region ExecuteScalar
        #region 非静态
        public object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
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
        #endregion
        #endregion

        #region ExecuteReader
        #region 非静态
        public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
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
        #endregion
        #endregion

        #region ExecuteDataSet
        #region 非静态
        public DataSet ExecuteDataset(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteDataset(cmdText, null, cmdParms);
        }

        public DataSet ExecuteDataset(string cmdText, int? timeout, params SqlParameter[] cmdParms)
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
        #endregion
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

