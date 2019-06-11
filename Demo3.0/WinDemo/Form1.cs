using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinDemo
{
    public partial class Form1 : Form
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ComClass com = new ComClass();
            DataTable dtConfig = com.GetDataSourceTable();

            DataRow[] drSmzq = dtConfig.Select("DANAME_='TestDB'");
            if (drSmzq.Length != 1)
            {
                MessageBox.Show("生命周期数据源配置错误！");
                return;
            }
            ComClass.m_DB_Source  = drSmzq[0]["DASOURCE_"].ToString().Trim();
            ComClass.m_DB_User = drSmzq[0]["USER_"].ToString().Trim();
            ComClass.m_DB_Pwd = PWDEncrypt.Decrypt(drSmzq[0]["PAS_"].ToString().Trim());


            string strProcess = Process.GetCurrentProcess().Id.ToString();
            this.Text = "测试系统  进程ID:" + strProcess;
            string strAdd = ComClass.GetMacAddress();
            ComClass.m_Win32Or64 = ComClass.Distinguish64or32System();

            log.InfoFormat("操作系统：{0}位,客户端IP地址:{1},Mac地址:{2}。", ComClass.m_Win32Or64, ComClass.m_Client_Ip, strAdd);


        }

        private void MenuUser_Click(object sender, EventArgs e)
        {
            UserForm user = new UserForm();
            user.ShowDialog();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void 基础信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 人员报表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
