// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 系统资源字典表 实体类
// 代码编写: 李罗成
// 文件路径: G:\svnnew\201508_cms_v2\trunk\code\src\Common\BK.Cloud.Model\Model\TB_Dictionary.cs
// 创建时间: 2017/3/1 10:33:37
// ------------------------------

using System;
using Fireasy.Data.Entity;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 系统资源字典表 实体类。
    /// </summary>
    [Serializable]
    [EntityMapping("TB_Dictionary", Description = "系统资源字典表")]
    public partial class TB_Dictionary : LighEntityObject<TB_Dictionary>
    {
        
        /// <summary>
        /// 获取或设置数据字典编号。
        /// </summary>
        [PropertyMapping(ColumnName = "DictionaryId", Description = "数据字典编号", IsPrimaryKey = true, IsNullable = false)]
        public virtual long DictionaryId { get; set; }

        /// <summary>
        /// 获取或设置字典分类编码。
        /// </summary>
        [PropertyMapping(ColumnName = "ClassId", Description = "字典分类编码", IsNullable = false, Length = 128)]
        public virtual string ClassId { get; set; }

        /// <summary>
        /// 获取或设置字典分类父编码。
        /// </summary>
        [PropertyMapping(ColumnName = "ClassParentId", Description = "字典分类父编码", IsNullable = false, Length = 128)]
        public virtual string ClassParentId { get; set; }

        /// <summary>
        /// 获取或设置字典名称。
        /// </summary>
        [PropertyMapping(ColumnName = "DictionaryName", Description = "字典名称", IsNullable = false, Length = 255)]
        public virtual string DictionaryName { get; set; }

        /// <summary>
        /// 获取或设置字典值。
        /// </summary>
        [PropertyMapping(ColumnName = "DictionaryText", Description = "字典值", IsNullable = false, Length = 255)]
        public virtual string DictionaryText { get; set; }

        /// <summary>
        /// 获取或设置显示顺序。
        /// </summary>
        [PropertyMapping(ColumnName = "DisplayIndex", Description = "显示顺序")]
        public virtual int? DisplayIndex { get; set; }

        /// <summary>
        /// 获取或设置Value。
        /// </summary>
        [PropertyMapping(ColumnName = "Value", Description = "Value", Length = 255)]
        public virtual string Value { get; set; }

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "ExtendField1", Description = "扩展字段1", Length = 255)]
        public virtual string ExtendField1 { get; set; }

        /// <summary>
        /// 获取或设置扩展字段2。
        /// </summary>
        [PropertyMapping(ColumnName = "ExtendField2", Description = "扩展字段2", Length = 255)]
        public virtual string ExtendField2 { get; set; }

        
        

        
    }
}