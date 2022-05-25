// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 调试日志表 实体类
// 代码编写: 谭凌德
// 文件路径: G:\svnnew\201508_cms_v2\trunk\code\src\Common\BK.Cloud.Model\Model\TB_DebugLog.cs
// 创建时间: 2017/3/1 10:33:38
// ------------------------------

using System;
using Fireasy.Data.Entity;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 调试日志表 实体类。
    /// </summary>
    [Serializable]
    [EntityMapping("TB_DebugLog", Description = "调试日志表")]
    public partial class TB_DebugLog : LighEntityObject<TB_DebugLog>
    {
        
        /// <summary>
        /// 获取或设置日志ID。
        /// </summary>
        [PropertyMapping(ColumnName = "LogId", Description = "日志ID", IsPrimaryKey = true, IsNullable = false)]
        public virtual long LogId { get; set; }

        /// <summary>
        /// 获取或设置日志类型。
        /// </summary>
        [PropertyMapping(ColumnName = "LogModel", Description = "日志类型", IsNullable = false, Length = 200)]
        public virtual string LogModel { get; set; }

        /// <summary>
        /// 获取或设置日志类型.比如：正常包,错误包.异常包等等。
        /// </summary>
        [PropertyMapping(ColumnName = "LogInfoType", Description = "日志类型.比如：正常包,错误包.异常包等等", Length = 20)]
        public virtual string LogInfoType { get; set; }

        /// <summary>
        /// 获取或设置LogDate。
        /// </summary>
        [PropertyMapping(ColumnName = "LogDate", Description = "LogDate")]
        public virtual DateTime? LogDate { get; set; }

        /// <summary>
        /// 获取或设置LogEquipment。
        /// </summary>
        [PropertyMapping(ColumnName = "LogEquipment", Description = "LogEquipment", Length = 50)]
        public virtual string LogEquipment { get; set; }

        /// <summary>
        /// 获取或设置LogMessage。
        /// </summary>
        [PropertyMapping(ColumnName = "LogMessage", Description = "LogMessage", Length = 5000)]
        public virtual string LogMessage { get; set; }

        /// <summary>
        /// 获取或设置LogUser。
        /// </summary>
        [PropertyMapping(ColumnName = "LogUser", Description = "LogUser")]
        public virtual long? LogUser { get; set; }

        /// <summary>
        /// 获取或设置LastUpdateDate。
        /// </summary>
        [PropertyMapping(ColumnName = "LastUpdateDate", Description = "LastUpdateDate")]
        public virtual DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// 获取或设置ReceiveBytes。
        /// </summary>
        [PropertyMapping(ColumnName = "ReceiveBytes", Description = "ReceiveBytes", Length = 2000)]
        public virtual string ReceiveBytes { get; set; }

        /// <summary>
        /// 获取或设置ReplayBytes。
        /// </summary>
        [PropertyMapping(ColumnName = "ReplayBytes", Description = "ReplayBytes", Length = 2000)]
        public virtual string ReplayBytes { get; set; }

        ///// <summary>
        ///// 获取或设置Status。
        ///// </summary>
        //[PropertyMapping(ColumnName = "Status", Description = "Status", Length = 50)]
        //public virtual string Status { get; set; }

        
        

        
    }
}