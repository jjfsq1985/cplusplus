using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SqlServerHelper
{
    public class SqlHelper : InterfaceSqlOperator
    {
        private SqlConnection m_Conn = null;

        public bool OpenSqlServerConnection(string strServerName, string strDbName, string strUser, string strPwd)
        {
            string strConnection= "Persist Security Info=False;Integrated Security=sspi;server=" + strServerName +
                                                ";Initial Catalog=" + strDbName + ";User ID=" + strUser + ";Password=" + strPwd;
            m_Conn = new SqlConnection(strConnection);
            m_Conn.Open();
            return m_Conn == null ? false: true;
        }

        /// <summary>
        /// 使用SQL语句执行
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>执行结果</returns>
        public int ExecuteCommand(string strSql)
        {
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strSql, m_Conn);
            cmd.CommandType = CommandType.Text;
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true, 
                                                                                        0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            cmd.ExecuteNonQuery();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        public int ExecuteCommand(string strSql, out SqlDataReader dataReader)
        {
            dataReader = null;
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strSql, m_Conn);
            cmd.CommandType = CommandType.Text;
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                                        0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            
            dataReader = cmd.ExecuteReader();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        /// <summary>
        /// 参数化SQL查询
        /// </summary>
        /// <param name="strProcName">SQL语句</param>
        /// <param name="procParam">SQL语句的参数</param>
        /// <returns>执行结果</returns>
        public int ExecuteCommand(string strSql, SqlParameter[] procParam)
        {
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strSql, m_Conn);
            cmd.CommandType = CommandType.Text;

            // 依次把参数传入SQL语句
            if (procParam != null)
            {
                foreach (SqlParameter param in procParam)
                    cmd.Parameters.Add(param);
            }
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                               0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            cmd.ExecuteNonQuery();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        /// <summary>
        /// 参数化SQL查询
        /// </summary>
        /// <param name="strProcName">SQL语句</param>
        /// <param name="procParam">SQL语句的参数</param>        
        /// <param name="dataReader">输出</param>
        /// <returns>执行结果</returns>
        public int ExecuteCommand(string strSql, SqlParameter[] procParam, out SqlDataReader dataReader)
        {
            dataReader = null;
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strSql, m_Conn);
            cmd.CommandType = CommandType.Text;

            // 依次把参数传入SQL语句 
            if (procParam != null)
            {
                foreach (SqlParameter param in procParam)
                    cmd.Parameters.Add(param);
            }
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                               0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            dataReader = cmd.ExecuteReader();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }


        /// <summary>
        /// 使用无参数无输出的存储过程执行
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <returns>执行结果</returns>
        public int ExecuteProc(string strProcName)
        {
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strProcName, m_Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, 
                                                                                        true, 0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);            
            cmd.ExecuteNonQuery();
            if(cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

       /// <summary>
        /// 使用存储过程执行
       /// </summary>
        /// <param name="strProcName">存储过程名称</param>
       /// <param name="dataReader">输出</param>
        /// <returns>执行结果</returns>
        public int ExecuteProc(string strProcName, out SqlDataReader dataReader)
        {
            dataReader = null;
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strProcName, m_Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                                        0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            dataReader = cmd.ExecuteReader();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        /// <summary>
        /// 存储过程参数组装
        /// </summary>
        /// <param name="ParamName">参数名称</param>
        /// <param name="DbType">类型</param>
        /// <param name="Size">字节数</param>
        /// <param name="Direction">参数方向(输入/输出)</param>
        /// <param name="Value">参数值</param>
        /// <returns>执行结果</returns>
        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;
            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;
            return param; 
        }

       /// <summary>
        /// 使用存储过程执行
       /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="procParam">存储过程的参数</param>
        /// <returns>执行结果</returns>
        public int ExecuteProc(string strProcName, SqlParameter[] procParam)
        {
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strProcName, m_Conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // 依次把参数传入存储过程 
            if (procParam != null)
            {
                foreach (SqlParameter param in procParam)
                    cmd.Parameters.Add(param);
            }
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                               0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            cmd.ExecuteNonQuery();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        /// <summary>
        /// 使用存储过程执行
        /// </summary>
        /// <param name="strProcName">存储过程名称</param>
        /// <param name="procParam">存储过程的参数</param>
        /// <param name="dataReader">输出</param>
        /// <returns>执行结果</returns>
        public int ExecuteProc(string strProcName, SqlParameter[] procParam, out SqlDataReader dataReader)
        {
            dataReader = null;
            if (m_Conn == null)
                return 0;
            SqlCommand cmd = new SqlCommand(strProcName, m_Conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // 依次把参数传入存储过程 
            if (procParam != null)
            {
                foreach (SqlParameter param in procParam)
                    cmd.Parameters.Add(param);
            }
            SqlParameter retValParam = new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, true,
                                                                               0, 0, string.Empty, DataRowVersion.Default, null);
            // 加入返回参数 
            cmd.Parameters.Add(retValParam);
            dataReader = cmd.ExecuteReader();
            if (cmd.Parameters["ReturnValue"].Value != null)
                return (int)cmd.Parameters["ReturnValue"].Value;
            else
                return 1;
        }

        public bool CloseConnection()
        {
            if (m_Conn != null)
            {
                m_Conn.Close();
                m_Conn = null;
            }
            return m_Conn == null ? true : false;
        }
    }
}
