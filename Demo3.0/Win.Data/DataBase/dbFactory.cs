using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using log4net;
using System.Reflection;

namespace Win.Data
{
    public class dbFactory
    {
        ///  注释 20100122
        /// ===================================================================================================================
        //private static dbIntface connIntance;
        /// ===================================================================================================================
        
        public dbFactory()
        {
        }
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string m_DatabaseTypeXML = "ORACLE"; //System.Configuration.ConfigurationManager.AppSettings["DBType"].ToString();

        public static dbIntface GetDataBase(string dataSource,string userId,string pwd )
        {
            try
            {
                dbIntface dbAccess;

                ///  data source=IDMP_99;user id=CTFA;password=ctfa
                /// ===================================================================================================================
                if (m_DatabaseTypeXML == "ORACLE")
                    dbAccess = new oracleDao(dataSource, userId, pwd);
                else
                    dbAccess = new sqlDao("","","","");
                /// ===================================================================================================================

                ///  注释 20100122
                /// ===================================================================================================================
                //connIntance = dbAccess;
                /// ===================================================================================================================

                return dbAccess;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("数据错误:{0}", ex.Message);
                return null; 
            }
        }


        /// <summary>
        /// 专门SQL Server
        /// </summary>
        /// <param name="ServerSource"></param>
        /// <param name="serverDB"></param>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static dbIntface GetDataBase(string ServerSource, string serverDB, string userId, string pwd)
        {
            try
            {
                dbIntface dbAccess = new sqlDao(ServerSource, serverDB, userId, pwd);
                
                return dbAccess;
            }
            catch(Exception ex) 
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
        }

        ///  增加 20100122
        /// ===================================================================================================================
        public static dbIntface GetDataBase(out string strErrMsg)
        {
            try
            {
                dbIntface dbAccess;
                if (m_DatabaseTypeXML == "ORACLE")
                    dbAccess = new oracleDao(out strErrMsg);
                else
                    dbAccess = new sqlDao(out strErrMsg);
                return dbAccess;
            }
            catch(Exception ex)
            {
                strErrMsg = ex.Message;
                return null;
            }
        }

 
        //=====================================================================================
    }
}