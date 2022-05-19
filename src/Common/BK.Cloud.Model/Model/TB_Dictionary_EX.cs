// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 系统资源字典表 实体类(数据验证元数据)
// 代码编写: 李罗成
// 文件路径: G:\svnnew\201508_cms_v2\trunk\code\src\Common\BK.Cloud.Model\Model\TB_Dictionary_EX.cs
// 创建时间: 2017/3/1 10:33:37
// ------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using Fireasy.Data.Entity;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TB_DictionaryMetadata 中定义验证特性。
    [MetadataType(typeof(TB_DictionaryMetadata))] 
    public partial class TB_Dictionary
    {
    }

    public class TB_DictionaryMetadata
    {
        
        /// <summary>
        /// 属性 DictionaryId 的验证特性。
        /// </summary>
        
        [Required]
        public object DictionaryId { get; set; }

        /// <summary>
        /// 属性 ClassId 的验证特性。
        /// </summary>
        
        [Required]
        [StringLength(128)]
        public object ClassId { get; set; }

        /// <summary>
        /// 属性 ClassParentId 的验证特性。
        /// </summary>
        
        [Required]
        [StringLength(128)]
        public object ClassParentId { get; set; }

        /// <summary>
        /// 属性 DictionaryName 的验证特性。
        /// </summary>
        
        [Required]
        [StringLength(255)]
        public object DictionaryName { get; set; }

        /// <summary>
        /// 属性 DictionaryText 的验证特性。
        /// </summary>
        
        [Required]
        [StringLength(255)]
        public object DictionaryText { get; set; }

        /// <summary>
        /// 属性 DisplayIndex 的验证特性。
        /// </summary>
        
        public object DisplayIndex { get; set; }

        /// <summary>
        /// 属性 Value 的验证特性。
        /// </summary>
        
        [StringLength(255)]
        public object Value { get; set; }

        /// <summary>
        /// 属性 ExtendField1 的验证特性。
        /// </summary>
        
        [StringLength(255)]
        public object ExtendField1 { get; set; }

        /// <summary>
        /// 属性 ExtendField2 的验证特性。
        /// </summary>
        
        [StringLength(255)]
        public object ExtendField2 { get; set; }

    }
}