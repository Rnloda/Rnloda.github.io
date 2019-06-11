using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace WinDemo
{
    public class ComClass
    {
        public static string m_Win32Or64 = "32";

        /// <summary>
        ///  数据库-数据源
        /// </summary>
        public static string m_DB_Source;
        /// <summary>
        /// 数据库- 用户
        /// </summary>
        public static string m_DB_User;
        /// <summary>
        /// 数据库- 密码
        /// </summary>
        public static string m_DB_Pwd;


        /// <summary>
        /// 客户端的IP地址
        /// </summary>
        public static string m_Client_Ip = GetClientIp();
        public static string m_AppPath_Base = AppDomain.CurrentDomain.BaseDirectory + "\\BaseConfig";
        public static string m_CompPath_Base = AppDomain.CurrentDomain.BaseDirectory + "\\CompFile";
        public DataTable GetDataSource(string name)
        {
            DataTable dtXml = new DataTable(name);
            dtXml.Columns.Add("IDX_", typeof(Int32));
            dtXml.Columns.Add("DANAME_", typeof(string));
            dtXml.Columns.Add("DASOURCE_", typeof(string));
            dtXml.Columns.Add("USER_", typeof(string));
            dtXml.Columns.Add("PAS_", typeof(string));

            List<DataColumn> keyColumnList = new List<DataColumn>();
            keyColumnList.Add(dtXml.Columns["IDX_"]);
            dtXml.PrimaryKey = keyColumnList.ToArray();

            return dtXml;
        }


        public DataTable GetDataSourceTable()
        {
            DataTable dt = GetDataSource("DGR");
            string strFile = m_AppPath_Base + "//DataFile.xml";
            if (!File.Exists(strFile))
            {
                dt.WriteXml(strFile);
                return dt;
            }

            dt.ReadXml(strFile);

            return dt;
        }

        public static object m_obj = new object();

        /// <summary>
        /// 保存数据源
        /// </summary>
        /// <param name="dt"></param>
        public void WriteDataSourceTable(DataTable dt)
        {
            DataTable dtNew = dt.Copy();
            string strFile = m_AppPath_Base + "\\DataFile.xml";

            dtNew.WriteXml(strFile);
            dtNew.Clear();
            dtNew.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// 从yyyymmdd转换到 yyyy-mm-dd
        /// </summary>
        /// <param name="sDay"></param>
        /// <returns></returns>
        public static string FormStringToDateString(string sDay)
        {
            string strNew;
            strNew = sDay.Substring(0, 4) + "-" + sDay.Substring(4, 2) + "-" + sDay.Substring(6, 2);
            return strNew;
        }

        /// <summary>
        /// MAc地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string strMac = "";
            try
            {
                ManagementObjectSearcher searcher = MacSql("Win32_NetworkAdapterConfiguration");
                foreach (ManagementBaseObject disk in searcher.Get())
                {

                    if ((bool)disk["IPEnabled"] == true)
                        strMac = disk["MacAddress"].ToString();

                    disk.Dispose();
                    if (strMac != "") break;
                }
                searcher.Dispose();

                if (strMac.Trim().Length == 0)
                {
                    NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in fNetworkInterfaces)
                    {
                        strMac = adapter.GetPhysicalAddress().ToString();
                        if (strMac.Length >= 10 && strMac.Length < 15)
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }

            return strMac;
        }

        private static ManagementObjectSearcher MacSql(string key)
        {
            SelectQuery query = new SelectQuery("Select * From " + key);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            return searcher;
        }

        /// <summary>
        /// 判断是 32位操作系统，还是64位操作系统
        /// </summary>
        /// <returns></returns>
        public static string Distinguish64or32System()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return addressWidth;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "32";
            }
        }

        /// <summary>
        /// 取客户端机器的IP地址 
        ///  Window 7的取IP方法，与Windows Xp, windows2003不一样。
        /// </summary>
        /// <returns></returns>
        private static string GetClientIp()
        {
            try
            {
                string strHostName = Dns.GetHostName();

                IPAddress[] listAdd = Dns.GetHostEntry(strHostName).AddressList;
                string strIp = listAdd[0].ToString();

                string[] listPara = strIp.Split('.');

                if (listPara.Length == 4)
                    return strIp; // Windows Xp.98, windows2003的取法

                if (listAdd.Length >= 6)
                {
                    string strIp3 = listAdd[3].ToString();// Windows 7 的IP地址
                    string[] listPara3 = strIp3.Split('.');
                    if (listPara3.Length == 4)
                        return strIp3; // Win 7

                    string strIp6 = listAdd[5].ToString(); // 若配置IPV6的地址
                    if (strIp6.Split('.').Length == 4)
                    {
                        return strIp6;
                    }

                    for (int j = 0; j < listAdd.Length; j++)
                    {
                        string strIpC = listAdd[j].ToString();
                        if (strIpC.Split('.').Length == 4)
                            return strIpC; // Win 7
                    }

                    return "127.0.0.1";
                }
                else
                {
                    for (int i = 0; i < listAdd.Length; i++)
                    {
                        string strIpC = listAdd[i].ToString();
                        string[] listParaC = strIpC.Split('.');
                        if ((listParaC.Length == 4) && (listParaC[0].Length <= 3) && (listParaC[1].Length <= 3) && (listParaC[2].Length <= 3) && (listParaC[3].Length <= 3))
                            return strIpC; // Win 7
                    }
                }

                return "127.0.0.1";
            }
            catch
            {
                return "0.0.0.0";
            }
        }
 

        /// <summary>
        /// Grid 进行双缓存
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="setting"></param>
        public void DoubleBufferedGrid(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            System.Reflection.PropertyInfo pInfo = dgvType.GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pInfo.SetValue(dgv, setting, null);
        }

 

    }
}
