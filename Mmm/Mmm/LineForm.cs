using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.Data.SqlClient;

namespace Mmm
{
    public partial class LineForm : Form
    {
        public MainForm mf;
        public LineForm(MainForm f)
        {
            mf = f;
            InitializeComponent();
        }
        PointPairList list = new PointPairList();
        LineItem myCurve;
        private void Form1_Load(object sender, EventArgs e)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

           //更改标题的字体

            FontSpec myFont = new FontSpec("楷体", 23,Color.DarkBlue, false, false, false);

            myPane.Title.FontSpec = myFont;

            myPane.XAxis.Title.FontSpec = myFont;

            myPane.YAxis.Title.FontSpec = myFont;

            myPane.Title.Text = "金额支出折线图(日消费大于100的按100来显示,如果您想知道该日的详细消费信息，请通过上方列表进行查询)";
            myPane.XAxis.Title.Text = "时间";
            myPane.YAxis.Title.Text = "金额";
            myPane.XAxis.Type = ZedGraph.AxisType.DateAsOrdinal;
            myPane.XAxis.Scale.Format = "yyyy/MM/dd";
            var startYear = mf.dtpStart.Value.Year.ToString();
            var startMonth = mf.dtpStart.Value.Month.ToString();
            var startDay = mf.dtpStart.Value.Day.ToString();
            var startHour = mf.dtpStart.Value.Hour.ToString();
            var startMinute = mf.dtpStart.Value.Minute.ToString();
            var startSecond = mf.dtpStart.Value.Second.ToString();
            var startTime = startYear + "-" + startMonth + "-" + startDay ;
            var endYear = mf.dtpEnd.Value.Year.ToString();
            var endMonth = mf.dtpEnd.Value.Month.ToString();
            var endDay = mf.dtpEnd.Value.Day.ToString();
            var endHour = mf.dtpEnd.Value.Hour.ToString();
            var endMinute = mf.dtpEnd.Value.Minute.ToString();
            var endSecond = mf.dtpEnd.Value.Second.ToString();
            var endTime = endYear + "-" + endMonth + "-" + endDay ;
            string sql = "";
            if (mf.cbTo.Checked)
            {
                sql = string.Format("select cost_Time,SUM(fee) as fee from M_MmmInfo where cost_Time >= '" + startTime + "' and cost_Time <= '" + endTime + "' group by cost_Time order by cost_Time asc");

            }
            else
            {
                //sql = "select cost_Time,SUM(fee) as fee from M_MmmInfo where cost_Time= '" + Convert.ToDateTime(mf.dtpStart.Value.ToLongDateString()) + "' group by cost_Time order by cost_Time asc";
                MessageBox.Show("仅一天的数据，无法显示成图！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SqlDataReader sdr = SqlHelper.GetReader(sql);

            while (sdr.Read())
            {
                double x;
                double y;
                if (Convert.ToDouble(sdr["fee"]) >=100)
                {
                    x = (double)new XDate(Convert.ToDateTime(sdr["cost_Time"]));
                    y =100;//日消费大于100的按100显示

                }
                else
                {
                    x = (double)new XDate(Convert.ToDateTime(sdr["cost_Time"]));
                    y = Convert.ToDouble(sdr["fee"]);

                }
                list.Add(x, y);

            }
            sdr.Close();
            myCurve = myPane.AddCurve("我的支出", list, Color.Blue, SymbolType.Star);
            //myPane.Fill = new Fill(Color.Transparent, Color.FromArgb(200, 200, 255), 45.0f);
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();

        }


    }
}
