using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Model.Customer;

namespace BK.Cloud.Logic.Interface
{

    public interface ILogicDebug
    {
       
        /// <summary>
        /// 获取调试日志数据
        /// </summary>
        /// <param name="devcode">设备代码</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="message"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        PageResult GetDebugLog(string devcode, DateTime? beginDate, DateTime? endDate, string LogMessage, Condition objCondition);

        /// <summary>
        /// 获取错误日志数据
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="message"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        PageResult GetErrorLog(DateTime? beginDate, DateTime? endDate, string LogMessage, Condition objCondition);

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="modelName">模块</param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void LogErrorMessage(string modelName, string message, Exception ex = null);
        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="modelName">模块</param>
        /// <param name="devcode">设备代码</param>
        /// <param name="message">消息</param>
        void LogDebugMessage(string modelName, string devcode, string message);

        /// <summary>
        /// 数据采集设备命令日志
        /// </summary>
        /// <param name="devcode">设备代码</param>
        /// <param name="message">消息</param>
        void LogCommondMessage(string devcode, string model, string message);

    }
}
