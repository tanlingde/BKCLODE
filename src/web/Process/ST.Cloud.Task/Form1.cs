using Fireasy.Common.Logging;
using BK.Cloud.Logic;
using System;
using System.Threading;
using System.Windows.Forms;

namespace BK.Cloud.Task
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        const string KEY_64 = "ShenTu10";//注意了，是8个字符，64位

        const string IV_64 = "JiTuan99";
        private void Form1_Load(object sender, EventArgs e)
        {
            //LogicCar.GeDevLineStatus();
            Control.CheckForIllegalCrossThreadCalls = false;
            dataGridView1.AutoGenerateColumns = false;
            //修改人：谭凌德
            //修改时间：2019-09-30
            //循环添加需要显示的服务，在config中有配置的服务
            System.Collections.Generic.List<MethodApp> list = RunMyAppList.Instance.Apps;
            System.Collections.Generic.List<MethodApp> newList = new System.Collections.Generic.List<MethodApp>();
            foreach (MethodApp ma in list)
            {
                if (ma.IsShow)
                {
                    newList.Add(ma);
                }
            }

            bindingSource1.DataSource = newList;
            dataGridView1.DataSource = bindingSource1;
            var instance = ShowLog.Instance;
            RunMyAppList.Instance.AttachAction((msg) =>
            {
                instance.WriteLog(msg);
                if (dataGridView1.InvokeRequired)
                {
                    dataGridView1.BeginInvoke(new Action(dataGridView1.Refresh));
                }
                else
                {
                    dataGridView1.Refresh();
                }
            });
            RunMyAppList.Instance.StartAll();
            SetBtn(true);
            SetLSBtn();
            //this.Text += "[同步时间间隔：" + RunAppList.Target.RepeatInterval.TotalSeconds + "秒]";
        }

        /// <summary>
        /// 设置历史按钮是否可以显示
        /// </summary>
        private void SetLSBtn()
        {
            string isShowLSBtn = Tools.CommonHelper.GetAppConfig("IsShowLSBtn");
            try
            {
                button5.Visible = bool.Parse(isShowLSBtn);
            }
            catch
            {
                button5.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //RunAppList.IsRun = true;
            //new Action(() =>
            //    {
            //        RunAppList run = new RunAppList();
            //        run.SyncZdjxToFlowServer();
            //    }).BeginInvoke(null, null);
            RunMyAppList.Instance.StartAll();
            SetBtn(true);
        }

        void SetBtn(bool isStart)
        {
            button1.Enabled = !isStart;
            button2.Enabled = isStart;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RunMyAppList.Instance.StopAll();
            SetBtn(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ShowLog.Instance.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            //Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //RunAppList.Dispose();
            RunMyAppList.Instance.StopAll();
            Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LSData lsd = new LSData();
            lsd.Show();



            #region 注释

            //Thread thd = new Thread(() =>
            //{
            //    if (string.IsNullOrEmpty(textBox1.Text)&&string.IsNullOrEmpty(textBox2.Text))
            //    {
            //        MessageBox.Show("请输入天数！");
            //        return;
            //    }
            //    int i = 0, j = 0;
            //    try
            //    {
            //        i = int.Parse(textBox1.Text);
            //        j = int.Parse(textBox2.Text);
            //    }
            //    catch
            //    {
            //        MessageBox.Show("请输入正确的整数！");
            //        return;
            //    }
            //    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始处理历史的设备运行时间基础数据");
            //    //Logic.LogicProvide.LogicScheduleTask.SetDayRunDateil(i);
            //    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理历史的设备运行时间基础数据结束");
            //    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始同步历史的设备运行总时间与设备运行里程");
            //    //Logic.LogicProvide.LogicScheduleTask.SetMileageAndTimeToYesterday(i);
            //    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "同步历史的设备运行总时间与设备运行里程结束");
            //    this.Text = "正在处理历史数据..";
            //    try
            //    {
            //        Logic.LogicProvide.LogicScheduleTask.DailyChargeTimes(i, j);
            //        //Logic.LogicProvide.LogicScheduleTask.DailyChargeTimes(1, 200);
            //        // Logic.LogicProvide.LogicScheduleTask.InitYDZGDaysData(i);
            //        this.BeginInvoke(new Action(() =>
            //        {
            //            this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理完毕";
            //        }));
            //    }
            //    catch (Exception ex)
            //    {
            //        this.BeginInvoke(new Action(() =>
            //        {
            //            this.Text = "处理异常" + ex.Message;
            //        }));
            //    }

            //    //label1.Visible = false;
            //    //textBox1.Visible = false;
            //    //button5.Visible = false;
            //});
            //thd.Start();

            #endregion

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string OrgID = System.Configuration.ConfigurationSettings.AppSettings["OrgID"];
           // Logic.YDZG.YDZGLogicProvide.LogicMaintenPlanData.SetNewMaintenPlanTask(string.Empty, OrgID, string.Empty, DateTime.Parse("2016-09-01"));
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4) {
                var app = (dataGridView1.Rows[e.RowIndex].DataBoundItem as MethodApp);
                if (!app.IsStart)
                {
                    app.Start();
                    
                }
                else {
                    app.Stop();
                }
                dataGridView1.Refresh();
            }
        }

    }
}
