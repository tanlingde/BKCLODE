using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Fireasy.Common.Logging;

namespace BK.Cloud.Task
{
    partial class STTaskService : ServiceBase
    {
        public STTaskService()
        {
            InitializeComponent();
        }

        private RunMyAppList Instance = null;
        protected override void OnStart(string[] args)
        {
            try
            {
                DefaultLogger.Instance.Info("服务开始启动");
                Instance = RunMyAppList.Instance;
                Instance.StartAll();

                Instance.AttachAction(o =>
                {
                    DefaultLogger.Instance.Info(o);
                });

                DefaultLogger.Instance.Info("服务启动成功");
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Info("服务启动出现异常", ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                DefaultLogger.Instance.Info("服务开始停止");
                if (Instance != null)
                {
                    Instance.StopAll();
                }
                DefaultLogger.Instance.Info("服务停止成功");
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Info("服务停止出现异常", ex);
            }
        }
    }
}
