using System;
using log4net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using Win.Data;
using System.Windows.Forms;

namespace WinDemo
{
    public partial class SubAlter : Form
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string []arr = new string[7];
        private int status;
        public SubAlter()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }   
        public String[] getArry(string[] arr)
        {
            return this.arr = arr;
        }
        public int getStatus(int status)
        {
            return this.status = status;
        }
        private void SubAlter_Load(object sender, EventArgs e)
        { 
            if(status == 0)
            {
                this.Text = "添加用户"; 
            }
            else
            {
                dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);
                lbID.Text = "编号:" + arr[0];
                txtName.Text = arr[1];
                txtDep.Text = arr[2];
                txtCountry.Text = arr[3];
                txtPhone.Text = arr[4];
                if (arr[6] == "男")
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = 1;
                }
            }
            
        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string strUserName = txtName.Text.Trim();
            string strUserDep = txtDep.Text.Trim();
            string strUserPhone = txtPhone.Text.Trim();
            string strUserCountry = txtCountry.Text.Trim();
            string strGED = "";
            if (status == 0)
            {              
                if (strUserName.Length == 0)
                {
                    MessageBox.Show("请输入姓名！");
                    txtName.Focus();
                    return;
                }
                if(strUserName.Length>30)
                {
                    MessageBox.Show("姓名过长！");
                    txtName.Focus();
                    return;
                }
                if (strUserDep.Length == 0)
                {
                    MessageBox.Show("请输入部门！");
                    txtDep.Focus();
                    return;
                }
                if (strUserDep.Length > 30)
                {
                    MessageBox.Show("部门名字过长！");
                    txtDep.Focus();
                    return;
                }

                if (strUserCountry.Length == 0)
                {
                    MessageBox.Show("请输入国籍！");
                    txtCountry.Focus();
                    return;
                }
                if (strUserCountry.Length > 30)
                {
                    MessageBox.Show("国家名字过长！");
                    txtCountry.Focus();
                    return;
                }

                if (strUserPhone.Length == 0)
                {
                    MessageBox.Show("请输入手机号！");
                    txtPhone.Focus();
                    return;
                }
                if (strUserPhone.Length >11)
                {
                    MessageBox.Show("手机号非法");
                    txtPhone.Focus();
                    return;
                }

                if (comboBox1.SelectedIndex == 0)
                {
                    strGED = "男";
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    strGED = "女";
                }
                if (strGED == "")
                {
                    MessageBox.Show("请输入性别！");
                    return;
                }
                string StrRegTime = DateTime.Now.ToString("yyyy-MM-dd");
                dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);
                string strSql1 = "alter session set nls_date_format = 'yyyy/mm/dd'"; //初始化日期格式
                string strSql2 = "select ID_ from z_tmp_user";
                DataTable dt = new DataTable();
                dt = dao.GetDataTable(strSql2);
                string strSql3 = "insert into z_tmp_user(id_, uname_, dep_,country_,phone_,regdate_,gender_)values(seq_z_tmp_user.nextval,'" + strUserName + "', '" + strUserDep + "','" + strUserCountry  + "','" + strUserPhone + "','" + StrRegTime + "','" + strGED + "')";

                int nCou1 = 0;
                log.InfoFormat("添加新员工{0}", strUserName);
                try
                {
                    dao.ExecuteNonQuery(strSql1);

                    nCou1 = dao.ExecuteNonQuery(strSql3);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("错误：{0}", ex.Message);

                }

                if (nCou1 > 0)
                {
                    MessageBox.Show("保存成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("保存失败！");
                }
            }
            if (status == 1)
            {
                
                if (comboBox1.SelectedIndex == 0)
                {
                    strGED = "男";
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    strGED = "女";
                }

                dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);
                string strSql= "update z_tmp_user set uname_='" + strUserName + "',dep_='" + strUserDep + "', phone_='" + strUserPhone + "', country_='" + strUserCountry + "' ,gender_='" + strGED + "'    where id_ = '" + arr[0] + "'";
                int nCou = 0;
                log.Debug("修改操作");
                try
                {
                    nCou = dao.ExecuteNonQuery(strSql);
                }
                catch (Exception ex)
                {

                    log.Error(ex);
                }
                if (nCou > 0)
                {
                    MessageBox.Show("修改成功！");
                }
                else
                {
                    MessageBox.Show("修改失败！");
                }
            }
            
        }

    }
}
