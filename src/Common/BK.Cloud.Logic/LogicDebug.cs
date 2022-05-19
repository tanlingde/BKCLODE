using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fireasy.Data.Extensions;
using Nest;
using ServiceStack;
using BK.Cloud.Logic.Interface;
using BK.Cloud.Model.Customer;
using BK.Cloud.Tools;
using Fireasy.Common.Extensions;
using Fireasy.Common.Logging;
using BK.Cloud.Model.Data.Model;
using Fireasy.Data;

namespace BK.Cloud.Logic
{


    public class LogicDebug : BusQuery, ILogicDebug
    {

        
        /// <summary>
        /// 获取调试日志数据
        /// </summary>
        /// <param name="devcode">设备代码</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="message">消息内容模糊查询</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PageResult GetDebugLog(string devcode, DateTime? beginDate, DateTime? endDate, string LogMessage, Condition objCondition)
        {
            string sql = @"SELECT *,(length(ReceiveBytes)/2) ReceiveBytesLength FROM tb_debuglog td where 1=1 and td.LogInfoType='数据采集设备调试日志' ";
            var res = RunDb(db =>
            {
                if (!string.IsNullOrEmpty(devcode))
                {
                    //devcode = devcode.ToLower();
                    sql = string.Format(" {0} and  td.LogEquipment='{1}' ", sql, devcode);
                }
                if (!beginDate.IsNullOrEmpty())
                {
                    sql = string.Format(" {0} and  td.LogDate >= '{1}' ", sql, beginDate);
                }
                if (!endDate.IsNullOrEmpty())
                {
                    sql = string.Format(" {0} and td.LogDate <= '{1}' ", sql, endDate);
                }
                if (!string.IsNullOrEmpty(LogMessage))
                {
                    sql = string.Format(" {0} and  td.LogMessage like '%{1}%' ", sql, LogMessage);
                }
                objCondition.datatype = sql;
                objCondition.sort = "LogDate";
                //objCondition.order = objCondition.order;

                return QueryPageDataBySql(objCondition, db);
            });
            return res;
        }


        /// <summary>
        /// 获取错误日志数据
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="message">消息</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PageResult GetErrorLog(DateTime? beginDate, DateTime? endDate, string LogMessage, Condition objCondition)
        {
            string sql = @"SELECT * FROM tb_debuglog td where 1=1 and td.LogInfoType='数据采集错误日志' ";
            var res = RunDb(db =>
            {
                if (!beginDate.IsNullOrEmpty())
                {
                    sql = string.Format(" {0} and  td.LogDate >= '{1}' ", sql, beginDate);
                }
                if (!endDate.IsNullOrEmpty())
                {
                    sql = string.Format(" {0} and td.LogDate <= '{1}' ", sql, endDate);
                }
                if (!string.IsNullOrEmpty(LogMessage))
                {
                    sql = string.Format(" {0} and  td.LogMessage like '%{1}%' ", sql, LogMessage);
                }
                objCondition.datatype = sql;
                objCondition.sort = "LogDate";

                return QueryPageDataBySql(objCondition, db);
            });
            return res;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="modelName">模块</param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public async void LogErrorMessage(string modelName, string message, Exception ex = null)
        {
            try
            {

                using (DbContext db = new DbContext())
                {
                    TB_DebugLog debuginfo = new TB_DebugLog();
                    debuginfo.LogDate = DateTime.Now;
                    debuginfo.LastUpdateDate = DateTime.Now;
                    debuginfo.LogId = DateTime.Now.Ticks;
                    debuginfo.LogInfoType = "数据采集错误日志";
                    debuginfo.LogMessage = message + "|" + (ex == null ? "" : (ex.Message + ":" + ex.StackTrace));
                    //debuginfo.LogEquipment=devcode;
                    debuginfo.LogModel = modelName;
                    debuginfo.LogUser = null;
                    await db.TB_DebugLogs.InsertAsync(debuginfo);
                }
            }
            catch (Exception ex1)
            {
                DefaultLogger.Instance.Fatal("插入日志异常", ex1);
            }
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="modelName">模块</param>
        /// <param name="devcode">设备代码</param>
        /// <param name="message">消息</param>
        public async void LogDebugMessage(string modelName, string devcode, string message)
        {
            using (DbContext db = new DbContext())
            {
                TB_DebugLog debuginfo = new TB_DebugLog();
                debuginfo.LogDate = DateTime.Now;
                debuginfo.LastUpdateDate = DateTime.Now;
                debuginfo.LogId = DateTime.Now.Ticks;
                debuginfo.LogInfoType = "数据采集设备调试日志";
                debuginfo.LogMessage = message;
                debuginfo.LogEquipment = devcode;
                debuginfo.LogModel = modelName;
                debuginfo.LogUser = null;
                await db.TB_DebugLogs.InsertAsync(debuginfo);
            }
        }


        /// <summary>
        /// 数据采集设备命令日志
        /// </summary>
        /// <param name="devcode">设备代码</param>
        /// <param name="message">消息</param>
        public async void LogCommondMessage(string devcode, string model, string message)
        {
            try
            {
                using (DbContext db = new DbContext())
                {
                    TB_DebugLog debuginfo = new TB_DebugLog();
                    debuginfo.LogDate = DateTime.Now;
                    debuginfo.LastUpdateDate = DateTime.Now;
                    debuginfo.LogId = DateTime.Now.Ticks;
                    debuginfo.LogInfoType = "数据采集设备命令日志";
                    debuginfo.LogMessage = message;
                    debuginfo.LogEquipment = devcode;
                    debuginfo.LogModel = model;
                    debuginfo.LogUser = null;
                    await db.TB_DebugLogs.InsertAsync(debuginfo);
                }
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Fatal("插入日志异常", ex);
            }
        }


    }
}
