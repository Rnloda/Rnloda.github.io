using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Win.Data
{
    /// <summary>
    /// 后台写日志
    /// </summary>
    public class LogWriteClass
    {
        /// <summary>
        /// 目标
        /// </summary>
        public static object m_obj = new object();

        /// <summary>
        /// 文件路径
        /// </summary>
        private string m_PathFile = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";

        /// <summary>
        /// 日志
        /// </summary>
        public LogWriteClass()
        {
            if (!Directory.Exists(m_PathFile))
            {
                Directory.CreateDirectory(m_PathFile);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="errInfo">错误内容</param>
        public void WriteToFile(string errInfo)
        {
            string strInfo = "[" + GetTimeNow() + "]" + " " + errInfo;
            string strFileName = GetFileName();
            lock (LogWriteClass.m_obj)
            {
                StreamWriter sw = new StreamWriter(strFileName, true);

                sw.WriteLine(strInfo);

                sw.Close();
            }
        }

        private string GetFileName()
        {
            string strNow_Date = DateTime.Now.ToString("yyyyMMdd");
            string strFile = "Log_Web" + "-" + strNow_Date;
            string currFileName = m_PathFile + strFile + ".txt";
            return currFileName;
        }

        private string GetTimeNow()
        {
            string strNowMS = DateTime.Now.ToString("HH:mm:ss FFF");
            string[] listMs = strNowMS.Split(' ');
            if (listMs[1].Length == 1)
                strNowMS = strNowMS + "00";
            else
            {
                if (listMs[1].Length == 2)
                    strNowMS = strNowMS + "0";
            }
            return strNowMS;
        }

    }
}
