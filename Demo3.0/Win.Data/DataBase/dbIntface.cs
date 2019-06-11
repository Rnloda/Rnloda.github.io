using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Win.Data
{
    public interface dbIntface
    {
        bool OpenConnection();
        void CloseConnection();
        
        //简单sql执行
        DataSet GetData(string strSql);
        DataSet GetData(string strSql, out string strErrMsg);
        DataSet GetData(string strSql, out System.Data.OracleClient.OracleDataAdapter adapter);
        DataSet FillDataSet(DataSet dsData, string strSql, string tabName);

        /// <summary>
        /// 获取表数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        DataTable GetDataTable(string strSql);
        int ExecuteNonQuery(string strSql);
        int ExecuteNonQuery(string strSql, out string strErrMsg);
        int ExecuteNonQuery(string strSql, string strParamName, byte[] buff);
        int ExecuteNonQuery(string strSql, string strParamName, string parValue);
        object ExecuteScale(string strSql);
        object ExecuteScale(string strSql, out string strErrMsg);
        void ExecuteSqlTran(System.Collections.ArrayList SQLStringList);
        int ExecuteSql(string SQLString, string content);

        void AddParamWithValue(string paramName, object value, DbType dbType);

        int BeginTransaction();
        int CommitTransation();
        int RollBackTransation();

       // void RunProcedure(string storedProcName, IDataParameter[] parameters,out string returnparm);
        void RunProcedure(string storedProcName, IDataParameter[] parameters, out string[] returnparm, out string errorMsg);
        DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName);
        int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected);
        void RunProcedure(string storedProcName, IDataParameter[] parameters, out string returnparm);
    }
}
