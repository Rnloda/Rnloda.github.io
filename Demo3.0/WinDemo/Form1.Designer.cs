namespace WinDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.基础信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUser = new System.Windows.Forms.ToolStripMenuItem();
            this.报表管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.人员报表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(832, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // 基础信息ToolStripMenuItem
            // 
            this.基础信息ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuUser});
            this.基础信息ToolStripMenuItem.Name = "基础信息ToolStripMenuItem";
            this.基础信息ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.基础信息ToolStripMenuItem.Text = "基础信息";
            this.基础信息ToolStripMenuItem.Click += new System.EventHandler(this.基础信息ToolStripMenuItem_Click);
            // 
            // MenuUser
            // 
            this.MenuUser.Name = "MenuUser";
            this.MenuUser.Size = new System.Drawing.Size(152, 22);
            this.MenuUser.Text = "人员管理";
            this.MenuUser.Click += new System.EventHandler(this.MenuUser_Click);
            // 
            // 报表管理ToolStripMenuItem
            // 
            this.报表管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.人员报表ToolStripMenuItem});
            this.报表管理ToolStripMenuItem.Name = "报表管理ToolStripMenuItem";
            this.报表管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.报表管理ToolStripMenuItem.Text = "报表管理";
            // 
            // 人员报表ToolStripMenuItem
            // 
            this.人员报表ToolStripMenuItem.Name = "人员报表ToolStripMenuItem";
            this.人员报表ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.人员报表ToolStripMenuItem.Text = "人员报表";
            this.人员报表ToolStripMenuItem.Click += new System.EventHandler(this.人员报表ToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.基础信息ToolStripMenuItem,
            this.报表管理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(832, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 412);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "演示界面";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem 基础信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuUser;
        private System.Windows.Forms.ToolStripMenuItem 报表管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 人员报表ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}

