// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 调试日志表 实体类(数据验证元数据)
// 代码编写: 李罗成
// 文件路径: G:\svnnew\201508_cms_v2\trunk\code\src\Common\BK.Cloud.Model\Model\TB_DebugLog_EX.cs
// 创建时间: 2017/3/1 10:33:38
// ------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Fireasy.Data.Entity;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TB_DebugLogMetadata 中定义验证特性。
    [MetadataType(typeof(TB_DebugLogMetadata))] 
    public partial class TB_DebugLog
    {
    }

    public class TB_DebugLogMetadata
    {
        
        /// <summary>
        /// 属性 LogId 的验证特性。
        /// </summary>
        
        [Required]
        public object LogId { get; set; }

        /// <summary>
        /// 属性 LogModel 的验证特性。
        /// </summary>
        
        [Required]
        [StringLength(200)]
        public object LogModel { get; set; }

        /// <summary>
        /// 属性 LogInfoType 的验证特性。
        /// </summary>
        
        [StringLength(20)]
        public object LogInfoType { get; set; }

        /// <summary>
        /// 属性 LogDate 的验证特性。
        /// </summary>
        
        public object LogDate { get; set; }

        /// <summary>
        /// 属性 LogEquipment 的验证特性。
        /// </summary>
        
        [StringLength(50)]
        public object LogEquipment { get; set; }

        /// <summary>
        /// 属性 LogMessage 的验证特性。
        /// </summary>
        
        [StringLength(5000)]
        public object LogMessage { get; set; }

        /// <summary>
        /// 属性 LogUser 的验证特性。
        /// </summary>
        
        public object LogUser { get; set; }

        /// <summary>
        /// 属性 LastUpdateDate 的验证特性。
        /// </summary>
        
        public object LastUpdateDate { get; set; }

        /// <summary>
        /// 属性 ReceiveBytes 的验证特性。
        /// </summary>
        
        [StringLength(2000)]
        public object ReceiveBytes { get; set; }

        /// <summary>
        /// 属性 ReplayBytes 的验证特性。
        /// </summary>
        
        [StringLength(2000)]
        public object ReplayBytes { get; set; }

        /// <summary>
        /// 属性 Status 的验证特性。
        /// </summary>
        
        [StringLength(50)]
        public object Status { get; set; }

    }
}