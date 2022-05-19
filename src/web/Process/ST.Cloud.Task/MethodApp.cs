using Fireasy.Common.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BK.Cloud.Task
{
    public class MethodApp : IDisposable
    {
        public MethodApp()
        {
            _isStop = true;
            LastValue = null;
        }

        public Action<string> ShowMsg;

        public void RunMethod()
        {

            if (RealRunMethod == null)
                return;

            RealRunMethod(this);

        }

        /// <summary>
        /// 是否停止
        /// </summary>
        private bool _isStop = true;

        /// <summary>
        /// 方法名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 方法代码
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// 最新值
        /// </summary>
        public double? LastValue { get; set; }

        /// <summary>
        /// 最近运行时间
        /// </summary>
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 运行时间间隔(小时)
        /// </summary>
        public double TimeInterval { get; set; }

        public string RunTypeStr
        {
            get
            {
                if (RunType == 0)
                {
                    return "每隔 " + TimeInterval + " 小时执行";
                }
                else
                {
                    return "每天第 " + TimeInterval + " 小时执行";
                }
            }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 是否显示（对应的config文件中存在）
        /// </summary>
        public bool IsShow { get; set; }

        public Action<MethodApp> RealRunMethod { get; set; }

        private Thread _thd;


        public void Start()
        {
            //是否执行
            if (!IsShow)
                return;
            if (!_isStop)
                return;
            _isStop = false;
            _thd = new Thread(InnerRun);

            // _thd.IsBackground = true;
            _thd.Start();
        }

        public void Stop()
        {
            if (!IsShow)
                return;

            try
            {
                if (_thd != null)
                {
                    _thd.Interrupt();
                    _thd.Abort();
                }
                _isStop = true;
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsStart
        {
            get { return !_isStop; }
        }

        public void InnerRun()
        {
            try
            {
                RunMyMethod();
                while (!_isStop)
                {
                    if (RunType == 0)
                    {
                        Thread.Sleep(TimeSpan.FromHours(TimeInterval));
                        RunMyMethod();
                    }
                    else
                    {
                        Thread.Sleep(60 * 60 * 1000);
                        if (DateTime.Now.Hour == ((int)TimeInterval))
                        {
                            RunMyMethod();
                        }
                    }
                }
            }
            catch (ThreadInterruptedException ex1)
            {
                //DefaultLogger.Instance.Error("服务[" + AppName + "]中断报错", ex1.InnerException ?? ex1);
            }
            catch (ThreadAbortException ex1)
            {
                //DefaultLogger.Instance.Error("服务[" + AppName + "]中断报错", ex1.InnerException ?? ex1);
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Error("服务[" + AppName + "]报错", ex.InnerException ?? ex);
            }
        }


        public void RunMyMethod()
        {
            DateTime starTime = DateTime.Now;

            try
            {

                RunMethod();
                DefaultLogger.Instance.Info("[成功]调度任务 [" + AppName + "(" + AppCode + ")" + "]执行成功, 开始时间:" +
                                            starTime.ToString("yyyy-MM-dd HH:mm:ss") + ",结束时间:" +
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",耗时 " +
                                            (DateTime.Now - starTime).TotalSeconds + " 秒");
                if (ShowMsg != null)
                {
                    ShowMsg("[成功]调度任务 [" + AppName + "(" + AppCode + ")" + "]执行成功, 开始时间:" +
                            starTime.ToString("yyyy-MM-dd HH:mm:ss") + ",结束时间:" +
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",耗时 " +
                            (DateTime.Now - starTime).TotalSeconds + " 秒");
                }
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Error(
                    "[异常]调度任务 [" + AppName + "(" + AppCode + ")" + "]执行异常, 开始时间:" +
                    starTime.ToString("yyyy-MM-dd HH:mm:ss") + ",出现异常耗时 " + (DateTime.Now - starTime).TotalSeconds +
                    " 秒", ex.InnerException ?? ex);

                if (ShowMsg != null)
                {
                    ShowMsg("[异常]调度任务 [" + AppName + "(" + AppCode + ")" + "]执行异常, 开始时间:" +
                    starTime.ToString("yyyy-MM-dd HH:mm:ss") + ",出现异常耗时 " + (DateTime.Now - starTime).TotalSeconds +
                    " 秒,异常明细:" + (ex.InnerException ?? ex).StackTrace);
                }
                //   DefaultLogger.Instance.Info("调度任务 [" + AppName + "(" + AppCode + ")" + "] 出现异常,错误如下:", ex);
            }
            finally
            {
                LastRunTime = DateTime.Now;
            }
        }


        /// <summary>
        /// 0，间隔执行（多少小时执行）.1.定时执行（固定每天的几点执行）
        /// </summary>
        public int RunType { get; set; }

        /// <summary>
        ///  附加参数
        /// </summary>
        public Dictionary<string, string> Params = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);


        public string RunStatus
        {
            get
            {
                if (!_isStop)
                {
                    return "已启动";
                }
                else
                {
                    return "已停止";
                }
            }
        }

        public string RunButton
        {
            get
            {
                if (!_isStop)
                {
                    return "停止";
                }
                else
                {
                    return "启动";
                }
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class StatisVars
    {

        public StatisVars()
        {
            Unit = string.Empty;
            Value = 0;
            Vname = string.Empty;
            DevId = string.Empty;
            Vid = -1;
            RecordDate = DateTime.Now;
        }

        public string DevId { get; set; }

        public int Vid { get; set; }

        public string Vname { get; set; }

        public decimal Value { get; set; }

        public DateTime RecordDate { get; set; }

        public string Unit { get; set; }

        public decimal FirstVal { get; set; }

        /// <summary>
        /// 是否昨天
        /// </summary>
        public bool IsYesToday { get; set; }

        /// <summary>
        /// 昨天值
        /// </summary>
        public decimal YesTodayVal { get; set; }
    }


    public class DevBom
    {

        public DevBom()
        {
            IsChange = 1;
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Devid { get; set; }

        /// <summary>
        /// 部件ID
        /// </summary>
        public int PartId { set; get; }

        /// <summary>
        /// 1.型号。2.部件。3.部件子级部件
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 当前工作时间
        /// </summary>
        public double WorkTime { get; set; }

        /// <summary>
        /// 维护周期
        /// </summary>
        public double Overtime { get; set; }

        /// <summary>
        /// 使用寿命
        /// </summary>
        public double Lifetime { get; set; }

        /// <summary>
        /// 维护内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 提醒时间.离报警还剩下多少时间提醒
        /// </summary>
        public double Warntime { get; set; }

        #region 自定义
        /// <summary>
        /// 告警剩下时间.正式报警到已告警剩余的时间。
        /// </summary>
        public double Warntime1 { get; set; }

        /// <summary>
        /// 最后更换或维护时工作时间
        /// </summary>
        public double LastChangeTime { get; set; }

        /// <summary>
        /// 1.维修。2.更换
        /// </summary>
        public int IsChange { get; set; }

        /// <summary>
        /// 最近一次的状态。1.维修。2.更换
        /// </summary>
        public int LastIsChange { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime LastUpDate { get; set; }

        #endregion


    }

}
