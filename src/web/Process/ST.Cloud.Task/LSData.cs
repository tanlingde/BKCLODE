using Fireasy.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BK.Cloud.Task
{
    public partial class LSData : Form
    {
        public LSData()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread thd = new Thread(() =>
            {
                if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("请输入天数！");
                    return;
                }
                int i = 0, j = 0;
                try
                {
                    i = int.Parse(textBox1.Text);
                    j = int.Parse(textBox2.Text);
                }
                catch
                {
                    MessageBox.Show("请输入正确的整数！");
                    return;
                }
                this.Text = "正在处理历史数据..";
                try
                {
                    string OrgCode = System.Configuration.ConfigurationSettings.AppSettings["OrgCode"];
                    string CarType = System.Configuration.ConfigurationSettings.AppSettings["CarType"];

                 //   Logic.LogicProvide.LogicHmTask.DailyChargeTimes(i, j, OrgCode, CarType);
                    //Logic.LogicProvide.LogicScheduleTask.DailyChargeTimes(1, 200);
                    // Logic.LogicProvide.LogicScheduleTask.InitYDZGDaysData(i);
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理完毕";
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = "处理异常" + ex.Message;
                    }));
                }
            });
            thd.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thd = new Thread(() =>
            {
                string OrgCode = System.Configuration.ConfigurationSettings.AppSettings["OrgCode"];
                string CarType = System.Configuration.ConfigurationSettings.AppSettings["CarType"];

                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    MessageBox.Show("请输入天数！");
                    return;
                }
                int i = 0;
                try
                {
                    i = int.Parse(textBox3.Text);
                }
                catch
                {
                    MessageBox.Show("请输入正确的整数！");
                    return;
                }

                this.Text = "正在处理历史数据..";
                try
                {
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始处理历史的设备运行时间基础数据");
                   // Logic.LogicProvide.LogicHmTask.SetDayRunDateil(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理历史的设备运行时间基础数据结束");
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始同步历史的设备运行总时间与设备运行里程");
                 //   Logic.LogicProvide.LogicHmTask.SetMileageAndTimeToYesterday(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "同步历史的设备运行总时间与设备运行里程结束");
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理完毕";
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = "处理异常" + ex.Message;
                    }));
                }

            });
            thd.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread thd = new Thread(() =>
            {
                if (string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("请输入天数！");
                    return;
                }
                int i = 0;
                try
                {
                    i = int.Parse(textBox4.Text);
                }
                catch
                {
                    MessageBox.Show("请输入正确的整数！");
                    return;
                }

                this.Text = "正在处理历史数据..";
                try
                {
                    string OrgCode = System.Configuration.ConfigurationSettings.AppSettings["OrgCode"];
                    string CarType = System.Configuration.ConfigurationSettings.AppSettings["CarType"];

                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行科达日运行时间");
                    //Logic.YDKT.YDKTLogicProvide.LogicTaskData.RunDayTime(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行科达日运行时间结束");
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行工作情况统计开始");
                   // Logic.YDKT.YDKTLogicProvide.LogicTaskData.KDPerformanceStatistics(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行工作情况统计结束");
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理完毕";
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = "处理异常" + ex.Message;
                    }));
                }

            });
            thd.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thd = new Thread(() =>
            {
                if (string.IsNullOrEmpty(textBox5.Text))
                {
                    MessageBox.Show("请输入天数！");
                    return;
                }
                int i = 0;
                try
                {
                    i = int.Parse(textBox5.Text);
                }
                catch
                {
                    MessageBox.Show("请输入正确的整数！");
                    return;
                }

                this.Text = "正在处理历史数据..";
                try
                {
                    string OrgCode = System.Configuration.ConfigurationSettings.AppSettings["OrgCode"];
                    string CarType = System.Configuration.ConfigurationSettings.AppSettings["CarType"];

                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行国标日运行时间");
                 //   Logic.LogicProvide.LogicGBTaskData.RunDayTime(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行国标日运行时间结束");
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行国标里程开始");
                 //   Logic.LogicProvide.LogicGBTaskData.SetGBDistance(i, OrgCode, CarType);
                    DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行国标里程结束");
                    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始执行国标日运行上线车辆数");
                    //Logic.LogicProvide.LogicGBTaskData.RunDayOnline(i, OrgCode, CarType);
                    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行国标日运行上线车辆结束");
                    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始国标新建车辆及总数");
                    //Logic.LogicProvide.LogicGBTaskData.SetOnlineCar(i, OrgCode, CarType);
                    //DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "执行国标新建车辆及总数结束");
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "处理完毕";
                    }));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        this.Text = "处理异常" + ex.Message;
                    }));
                }

            });
            thd.Start();
        }
    }
}
