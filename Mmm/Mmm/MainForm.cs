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
    public partial class MainForm : Form
    {  //登陆后改变status的状态为1；
        public int formCount = 0;
        public int status = 0;
        public MainForm(int m)
        {

            status = m;
            InitializeComponent();
            lbDate.Text = DateTime.Now.ToLongDateString() + "," + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            BindData();


        }
        //鼠标按下时的位置
        int x1;
        int y1;
        bool flag = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //判断是不是左键按下
            if (e.Button == MouseButtons.Left)
            {
                x1 = e.X;
                y1 = e.Y;
                flag = true;//按下时 标识值为true
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
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

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                flag = false;//标记左键弹起
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("您选择了退出系统操作,现在退出？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Application.Exit();
                status = 0;
            }

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            var strYear = dtpTime.Value.Year.ToString();
            var strMonth = dtpTime.Value.Month.ToString();
            var strDay = dtpTime.Value.Day.ToString();
            var strHour = dtpTime.Value.Hour.ToString();
            var strMinute = dtpTime.Value.Minute.ToString();
            var strSecond = dtpTime.Value.Second.ToString();
            var strTime = strYear + "-" + strMonth + "-" + strDay ;
            //lbID控件里存放要修改数据的ID
            if (lbID.Text != "")
            {
                string sql4 = "";
                //修改
                try
                {
                    sql4 = "update M_MmmInfo set cost_Time='" + strTime + "' ,fee=" + Convert.ToDouble(txtCost.Text.Trim()) + " ,cost_Content=" + "'" + txtContent.Text.Trim() + "' ,remark='" + txtRemark.Text.Trim() + "' where MID=" + lbID.Text;
                }
                catch (Exception)
                {

                    MessageBox.Show("信息有误，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int ct = SqlHelper.ExecuteNonQuery(sql4);
                if (ct > 0)
                {
                    string sql3 = "select * from M_MmmInfo where MID= " + lbID.Text;
                    string ms = dtpTime.Value.ToLongDateString() + "消费金额为：";
                    listView1.Items.Clear();
                    Bind(sql3, ms);
                    dtpTime.Value = DateTime.Now;
                    txtCost.Text = "";
                    txtContent.Text = "";
                    txtRemark.Text = "";
                    lbID.Text = "";
                }

            }

            else
            {

                string sql = "";
                try
                {
                    sql = string.Format("insert into M_MmmInfo values('{0}',{1},'{2}','{3}',getdate())", strTime, Convert.ToDouble(txtCost.Text.Trim()), txtContent.Text.Trim(), txtRemark.Text.Trim());

                }
                catch (Exception)
                {

                    MessageBox.Show("信息有误，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                int count = SqlHelper.ExecuteNonQuery(sql);
                if (count > 0)
                {
                    MessageBox.Show("已经添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string sql2 = "select * from M_MmmInfo where cost_Time= '" + strTime + "'";
                    string msgs = dtpTime.Value.ToLongDateString() + "消费金额为：";
                    listView1.Items.Clear();
                    Bind(sql2, msgs);
                    txtContent.Text = "";
                    txtCost.Text = "";
                    txtRemark.Text = "";
                }


            }

        }
        //查询
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {
                DialogResult dr = MessageBox.Show("为了隐私考虑，您登陆后才能进行这个操作，现在登陆？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Login lg = new Login();
                    lg.Show();
                    this.Hide();
                }
                else
                {
                    return;

                }
            }
            BindData();
        }


        private void BindData()
        {
            listView1.Items.Clear();
            string sql = "";
            string msg = "";
            var startYear = dtpStart.Value.Year.ToString();
            var startMonth = dtpStart.Value.Month.ToString();
            var startDay = dtpStart.Value.Day.ToString();
            var startHour = dtpStart.Value.Hour.ToString();
            var startMinute = dtpStart.Value.Minute.ToString();
            var startSecond = dtpStart.Value.Second.ToString();
            var startTime = startYear + "-" + startMonth + "-" + startDay ;
            var endYear = dtpEnd.Value.Year.ToString();
            var endMonth = dtpEnd.Value.Month.ToString();
            var endDay = dtpEnd.Value.Day.ToString();
            var endHour = dtpEnd.Value.Hour.ToString();
            var endMinute = dtpEnd.Value.Minute.ToString();
            var endSecond = dtpEnd.Value.Second.ToString();
            var endTime = endYear + "-" + endMonth + "-" + endDay ;
            
            if (cbTo.Checked)
            {
             
                sql = string.Format("select * from M_MmmInfo where cost_Time >= '" + startTime + "' and cost_Time <= '" +endTime + "'" + "order by cost_Time desc");
                msg = dtpStart.Value.ToLongDateString() + "至" + dtpEnd.Value.ToLongDateString() + "累计消费金额为：";
            }
            else
            {
                msg = dtpStart.Value.ToLongDateString() + "消费金额为：";
                sql = "select * from M_MmmInfo where cost_Time= '" + startTime + "'";
            }
            Bind(sql, msg);

        }

        private void Bind(string sql, string msg)
        {
            SqlDataReader sdr = SqlHelper.GetReader(sql);
            double num = 0;
            while (sdr.Read())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = sdr["MID"].ToString();
                lvi.SubItems.Add(Convert.ToDateTime(sdr["cost_Time"]).ToLongDateString());
                if (Convert.ToDouble(sdr["fee"]) >= 100)//单次消费100元及以上的红色标出
                {   
                    lvi.SubItems.Add(sdr["fee"].ToString());
                    lvi.UseItemStyleForSubItems = false;//允许设置某一个单元格的背景颜色
                    lvi.SubItems[2].BackColor = Color.Red;
                }
                else if (Convert.ToDouble(sdr["fee"]) >= 30 && Convert.ToDouble(sdr["fee"])<100)//单次消费30至100元的黄色标出
                {
                    lvi.SubItems.Add(sdr["fee"].ToString());
                    lvi.UseItemStyleForSubItems = false;//允许设置某一个单元格的背景颜色
                    lvi.SubItems[2].BackColor = Color.Yellow;
                }
                else
                {
                    lvi.SubItems.Add(sdr["fee"].ToString());
                }
                lvi.SubItems.Add(sdr["cost_Content"].ToString());
                lvi.SubItems.Add(sdr["remark"].ToString());
                listView1.Items.Add(lvi);
                num += Convert.ToDouble(sdr["fee"]);

            }
            sdr.Close();

            label11.Text = msg;
            label12.Text = num.ToString() + "元";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {
                DialogResult dr = MessageBox.Show("为了隐私考虑，您登陆后才能进行这个操作，现在登陆？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Login lg = new Login();
                    lg.Show();
                    this.Hide();
                }
                else
                {
                    return;

                }
            }
            else
            {
                LineForm lf = new LineForm(this);
                lf.TopLevel = false;
                panel2.Controls.Clear();
                this.panel2.Controls.Add(lf);
                lf.Show();
            }

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(groupBox1);
            this.panel2.Controls.Add(pictureBox9);
            this.panel2.Controls.Add(label1);
            dtpTime.Value = DateTime.Now;
            txtContent.Text = "";
            txtCost.Text = "";
            txtRemark.Text = "";
            groupBox1.Show();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        string sql = "delete from M_MmmInfo where Mid=" + id;
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
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(groupBox1);
            this.panel2.Controls.Add(pictureBox9);
            this.panel2.Controls.Add(label1);
            dtpTime.Value = DateTime.Now;
            groupBox1.Show();
            if (listView1.SelectedItems.Count == 1)
            {
                string id = listView1.SelectedItems[0].Text;
                lbID.Text = id;
                string sql = "select * from M_MmmInfo where Mid=" + id;
                SqlDataReader sdr = SqlHelper.GetReader(sql);
                while (sdr.Read())
                {
                    dtpTime.Value = Convert.ToDateTime(sdr["cost_Time"]);
                    txtCost.Text = sdr["fee"].ToString();
                    txtContent.Text = sdr["cost_Content"].ToString();
                    txtRemark.Text = sdr["remark"].ToString();
                }

            }
            else if (listView1.SelectedItems.Count > 1)
            {
                MessageBox.Show("每次只能修改一项，请您重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("请选择要修改的项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (status == 0)
            {

                DialogResult dr = MessageBox.Show("为了隐私考虑，您登陆后才能进行这个操作，现在登陆？", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    Login lg = new Login();
                    lg.Show();
                    this.Hide();
                }
                else
                {
                    return;
                }

            }
            else
            {
                UserForm lf = new UserForm();
                lf.Show();
                lf.TopMost = true;


            }

        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.Height = 25;
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Height = 23;
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Height = 25;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Height = 23;
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            pictureBox5.Height = 25;
        }

        private void pictureBox5_MouseLeave(object sender, EventArgs e)
        {
            pictureBox5.Height = 23;
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            pictureBox7.Height = 25;
        }

        private void pictureBox7_MouseLeave(object sender, EventArgs e)
        {
            pictureBox7.Height = 23;
        }

        private void pictureBox8_MouseEnter(object sender, EventArgs e)
        {
            pictureBox8.Height = 23;
        }

        private void pictureBox8_MouseLeave(object sender, EventArgs e)
        {
            pictureBox8.Height = 21;
        }

        private void pictureBox9_MouseEnter(object sender, EventArgs e)
        {
            pictureBox9.Height =90;

        }

        private void pictureBox9_MouseLeave(object sender, EventArgs e)
        {
            pictureBox9.Height =87;
        }


    }
}
