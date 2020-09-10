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
    public partial class UserForm : Form
    {
      
        public UserForm()
        {
            
            InitializeComponent();
        }
        /// <summary>
        /// 添加、修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox6_Click(object sender, EventArgs e)
        {

            if (lbid.Text=="")
            {
            string name = textBox1.Text.Trim();
            string pwd = textBox2.Text.Trim();
            if (name==""||pwd=="")
            {
                MessageBox.Show("用户名或密码不能为空！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            string  sql =string.Format( "insert into M_user values('{0}','{1}')",name,pwd);        
            int count = SqlHelper.ExecuteNonQuery(sql);
            if (count>0)
            {
                BindData();
                textBox1.Text= "";
                textBox2.Text = "";
            }
           
            }

            else
            {


                if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
                {
                    MessageBox.Show("用户名或密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

             string  sql = "update M_user set username='"+textBox1.Text.Trim()+"', userPwd='"+textBox2.Text.Trim()+"' where userid="+lbid.Text; 
                int num = SqlHelper.ExecuteNonQuery(sql);
                if (num>0)
                {
                    BindData();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    lbid.Text = "";
                }

            }

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            BindData();

        }

        private void BindData()
        {
            string sql = "select * from M_user";
            SqlDataReader sdr = SqlHelper.GetReader(sql);
            listView1.Items.Clear();
            while (sdr.Read())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = sdr["UserId"].ToString();
                lvi.SubItems.Add(sdr["userName"].ToString());
                lvi.SubItems.Add(sdr["userPwd"].ToString());
                listView1.Items.Add(lvi);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count==0)
            {
                MessageBox.Show("您还没有选择项！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else if (listView1.SelectedItems.Count>1)
            {
                MessageBox.Show("每次只能修改一项，请重现选择项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {

                string id = listView1.SelectedItems[0].Text;
                lbid.Text = id;
                textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;

            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {

                DialogResult dr = MessageBox.Show("信息删除将无法恢复，确定要删除信息？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {

                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        string id = listView1.SelectedItems[i].Text;
                        string sql = "delete from M_user where userid=" + id;
                        int it = SqlHelper.ExecuteNonQuery(sql);
                        if (it > 0)
                        {

                        }

                    }
                    BindData();
                }

            }

            else
            {
                MessageBox.Show("请选择要删除的信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //鼠标按下时的位置
        int x1;
        int y1;
        bool flag = false;
        private void UserForm_MouseDown(object sender, MouseEventArgs e)
        {
            //判断是不是左键按下
            if (e.Button == MouseButtons.Left)
            {
                x1 = e.X;
                y1 = e.Y;
                flag = true;//按下时 标识值为true
            }
        }

        private void UserForm_MouseMove(object sender, MouseEventArgs e)
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

        private void UserForm_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                flag = false;//标记左键弹起
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
           
           
        }
    }
}
