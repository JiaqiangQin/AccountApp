using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Mmm
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            SqlParameter[] pms = new SqlParameter[4];
            pms[0] = new SqlParameter("@userName", txtUsername.Text.Trim());
            pms[1] = new SqlParameter("@userpwd", txtPwd.Text.Trim());
            pms[2] = new SqlParameter("@msg",SqlDbType.VarChar,50);
            pms[3] = new SqlParameter("@success", SqlDbType.VarChar,10);
            pms[2].Direction = ParameterDirection.Output;
            pms[3].Direction = ParameterDirection.Output;
            int count = SqlHelper.ExecuteNonQuery("up_login", CommandType.StoredProcedure,pms);
            string msg = pms[2].Value.ToString();
            string suc = pms[3].Value.ToString();
            if (suc == "1")
            {
              //成功登陆            
                MainForm mf = new MainForm(1);
                mf.Show();
                this.Hide();
               
            }
            else
            {
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Text = "";
                txtPwd.Text = "";
            }
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();   
        }
        //鼠标按下时的位置
        int x1;
        int y1;
        bool flag = false;
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            //判断是不是左键按下
            if (e.Button == MouseButtons.Left)
            {
                x1 = e.X;
                y1 = e.Y;
                flag = true;//按下时 标识值为true
            }
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag == true)
            {//新位置
                int x2 = e.X;
                int y2 = e.Y;
                //窗体老位置
                int fx = this.Location.X;
                int fy = this.Location.Y;
                //窗体新位置
                fx += x2 - x1;
                fy += y2 - y1;
                //给窗体设置新位置
                this.Location = new Point(fx, fy);
            }
        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                flag = false;//标记左键弹起
            }
        }
    }
}
