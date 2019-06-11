using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Win.Data;

namespace WinDemo
{
    public partial class UserForm : Form
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public UserForm()
        { 
            InitializeComponent();
            ComClass com = new ComClass();
            com.DoubleBufferedGrid(grdView, true);
        }
      
        private void btnQuery_Click(object sender, EventArgs e)
        {
            dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);

            string strSql = "select ID_, UNAME_, DEP_ ,COUNTRY_ ,PHONE_ ,REGDATE_ ,GENDER_ from z_tmp_user ORDER BY id_";
                
            DataTable dtUser = dao.GetDataTable(strSql);         
            grdView.DataSource = dtUser;           
        }

        private void grdView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }         

        private void btnSearch_Click(object sender, EventArgs e)
        {                        
            string strUserId = txtID.Text.Trim();
            string strUserName = txtName.Text.Trim();
            string strUserDep = txtDep.Text.Trim();
            string strQue = "";
            if(strUserId .Length > 0)
            {
                strQue = " where id_ like'%" + strUserId + "%'";
            }
            if(strUserName .Length > 0)
            {
                if(strQue .Length == 0)
                {
                    strQue = " where uname_ like '%" + strUserName + "%'";
                }
                else
                {
                    strQue += "and uname_ like '%" + strUserName + "%'";
                }
            }
            if(strUserDep.Length  > 0)
            {
                if(strQue .Length == 0)
                {
                    strQue = " where DEP_='" + strUserDep + "'";
                }
                else
                {
                    strQue += "and  DEP_='" + strUserDep + "'";
                }
            }          
            dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);
            string strSql = "select *from z_tmp_user" + strQue+" order by ID_";
            DataTable dtUser = dao.GetDataTable(strSql);
            grdView.DataSource = dtUser;           
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SubAlter addForm = new SubAlter();
            addForm.getStatus(0);
            addForm.ShowDialog();            
            btnQuery_Click(null, null);
        }

        private void btnAlt_Click(object sender, EventArgs e)
        {
            if (grdView.DataSource == null)
            {
                MessageBox.Show("请选中要修改的行！");
                btnQuery_Click(null, null);
            }
            else
            {
                string[] arr = new string[7];
                for (int i = 0; i< 7; i++)
                {
                    arr[i] = grdView[i, grdView.SelectedCells[i].RowIndex].Value.ToString();
                }                                                                         
                    SubAlter sub = new SubAlter();
                sub.getStatus(1);
                sub.getArry(arr);
                sub.ShowDialog();
                    

            }

            btnQuery_Click(null, null);
        }

        private void btnDelet_Click(object sender, EventArgs e)
        {
            if (grdView.DataSource == null)
            {
                MessageBox.Show("请先选中数据");
            }
            else
            {                 
                string Confirm = string.Format("确认删除编号{0}，{1}?", grdView[0, grdView.SelectedCells[0].RowIndex].Value.ToString(), grdView[1, grdView.SelectedCells[1].RowIndex].Value.ToString());
                
                DialogResult dr = MessageBox.Show (Confirm , "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    string id_;
                    string name;
                    int index = 0;
                    string tmp = "delete from z_tmp_user where id_='" + grdView[index, grdView.SelectedCells[0].RowIndex].Value.ToString() + "'";
                    dbIntface dao = dbFactory.GetDataBase(ComClass.m_DB_Source, ComClass.m_DB_User, ComClass.m_DB_Pwd);
                    dao.ExecuteNonQuery(tmp);
                    id_ = grdView[0, grdView.SelectedCells[0].RowIndex].Value.ToString();
                    name = grdView[1, grdView.SelectedCells[1].RowIndex].Value.ToString();
                    log.InfoFormat("删除：编号{0},姓名{1}", id_, name);
                }
            }
            btnQuery_Click(null, null);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            btnQuery_Click(null, null);
        }

        private void grdView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
                string[] arr = new string[7];
                for (int i = 0; i< 7; i++)
                {
                    arr[i] = grdView[i, grdView.SelectedCells[i].RowIndex].Value.ToString();
                }
                SubAlter sub = new SubAlter();
                sub.getStatus(1);
                sub.getArry(arr);
                sub.ShowDialog();                     
                btnQuery_Click(null, null);
        }

      
        private void grdView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            string strCL = grdView.Rows[e.RowIndex].Cells[6].Value.ToString();
            string strDep = grdView.Rows[e.RowIndex].Cells[2].Value.ToString();
            string strCT = grdView.Rows[e.RowIndex].Cells[3].Value.ToString();
            if (strCL == "男")
            {
                grdView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Blue;
            }
            else
            {
                grdView.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
            }
            if (strDep == "123")
            {
                grdView.Rows[e.RowIndex].Cells[2].Style .ForeColor  = Color.Green ;
            }
            if (strCT == "US")
            {
                grdView.Rows[e.RowIndex].Cells[3].Style.Font = new Font("宋体", 12, FontStyle.Bold);
            }
        }

        private void grdView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > 0)
            {
                grdView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow ;
            }
        }

        private void grdView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                grdView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }
    }
    
}
