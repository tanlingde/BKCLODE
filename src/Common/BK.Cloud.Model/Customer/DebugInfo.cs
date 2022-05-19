using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Model.Customer
{
    /// <summary>
    /// 调试信息
    /// </summary>
    public class StDebugInfo
    {
        /// <summary>
        /// 设备代码
        /// </summary>
        public string DevCode { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? RecordDate { get; set; }


        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }


    }

    /// <summary>
    /// 错误信息
    /// </summary>
    public class StErrorInfo
    {
        /// <summary>
        /// 设备代码
        /// </summary>
        public string DevCode { get; set; }
        /// <summary>
        /// 模块
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? ErrorDate { get; set; }


        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误堆栈信息
        /// </summary>
        public string ErrStackInfo { get; set; }


    }

    /// <summary>
    /// 命令信息
    /// </summary>
    public class StCommond
    {
        public StCommond()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 发送的命令类型
        /// </summary>
        public string CmdType { get; set; }


        public string Id { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 设备代码
        /// </summary>
        public string DevCode { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 发送
        /// </summary>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// 回复日期
        /// </summary>
        public DateTime ReplayDate { get; set; }

        /// <summary>
        /// 命令编号
        /// </summary>
        public ushort FieldId { get; set; }

        /// <summary>
        /// 是否已发送
        /// </summary>
        public bool IsSend { get; set; }

        /// <summary>
        /// 发送命令
        /// </summary>
        public string SendCmd { get; set; }

        /// <summary>
        /// 接收命令
        /// </summary>
        public string ReplayCmd { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int SendCount { get; set; }

    }

    /// <summary>
    /// 服务信息
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 模块
        /// </summary>
        public string ServName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? RecordDate { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

    }
}
