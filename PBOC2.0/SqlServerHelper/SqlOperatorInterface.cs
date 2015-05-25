using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace SqlServerHelper
{
    interface InterfaceSqlOperator
    {
        bool OpenSqlServerConnection(string strServerName, string strDbName, string strUser, string strPwd);

        int ExecuteCommand(string strSql);
        int ExecuteCommand(string strSql, out SqlDataReader dataReader);
        int ExecuteProc(string strProcName);
        int ExecuteProc(string strProcName, out SqlDataReader dataReader);
        SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value);
        int ExecuteProc(string strProcName, SqlParameter[] procParam);
        int ExecuteProc(string strProcName, SqlParameter[] procParam, out SqlDataReader dataReader);

        bool CloseConnection();
    }
}
