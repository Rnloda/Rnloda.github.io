using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace Win.Data.OracleBase
{
    public class OracleDB
    {

        protected OracleConnection dbConnection = null;
        private static string connectString;
        public static string ConnectString
        {
            get { return connectString; }
            set { connectString = value; }
        }

        /// <summary>
        /// 只连ORACLE
        /// </summary>
        /// <param name="source">生命周期 数据源</param>
        /// <param name="user">生命周期 用户</param>
        /// <param name="pwd">生命周期 密码</param>
        public OracleDB(string source, string user, string pwd)
        {
            connectString = "data source=" + source + ";user id=" + user + ";password=" + pwd + "";
            Connect();
        }


        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        private bool Connect()
        {

            if (connectString == string.Empty)
            {
                return false;
            }
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                return true;
            }
            try
            {
                if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }


                dbConnection = new OracleConnection(ConnectString);
                //dbConnection.Timeout = 5;
                dbConnection.Open();
            }
            catch (Exception e)
            {
                if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
                {
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }

            return (dbConnection.State == ConnectionState.Open);

        }

        private void Close()
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
            dbConnection = null;
            GC.Collect();
        }

        /// <summary>
        /// 读取单一的字段
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IDataReader GetOneReader(string strSql)
        {
            OracleCommand cmd = dbConnection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strSql;

            cmd.ExecuteNonQuery();

            OracleDataReader odr = cmd.ExecuteReader();  
            cmd.Dispose();
            Close();
            cmd = null;

            return odr;
        }

        //public byte[] GetReaderOfOne(string strSql)
        //{
        //    OracleCommand cmd = dbConnection.CreateCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = strSql;

        //    cmd.ExecuteNonQuery();

        //    OracleDataReader odr = cmd.ExecuteReader();

            
        //    cmd.Dispose();
        //    Close();
        //    cmd = null;
        //    while (odr.Read())
        //    {
        //        byte[] ooo = (byte[])dr[0];  
        //    }


        //    return odr;
        //}

    }
}
