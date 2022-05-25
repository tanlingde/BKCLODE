// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 系统资源字典表 实体类
// 代码编写: 谭凌德
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
    [EntityMapping("TB_CarDictionary", Description = "车辆对照字典表")]
    public partial class TB_CarDictionary : LighEntityObject<TB_CarDictionary>
    {
        
        /// <summary>
        /// 获取或设置数据字典编号。
        /// </summary>
        [PropertyMapping(ColumnName = "BrandId", Description = "BrandId", IsNullable = false, Length = 255)]
        public virtual string BrandId { get; set; }

        /// <summary>
        /// 获取或设置字典分类编码。
        /// </summary>
        [PropertyMapping(ColumnName = "BrandName", Description = "BrandName", IsNullable = false, Length = 255)]
        public virtual string BrandName { get; set; }

        /// <summary>
        /// 获取或设置字典分类父编码。
        /// </summary>
        [PropertyMapping(ColumnName = "SeriesId", Description = "SeriesId", IsNullable = false, Length = 255)]
        public virtual string SeriesId { get; set; }

        /// <summary>
        /// 获取或设置字典名称。
        /// </summary>
        [PropertyMapping(ColumnName = "SeriesName", Description = "SeriesName", IsNullable = false, Length = 255)]
        public virtual string SeriesName { get; set; }

        /// <summary>
        /// 获取或设置字典值。
        /// </summary>
        [PropertyMapping(ColumnName = "Abbr", Description = "Abbr", IsNullable = false, Length = 255)]
        public virtual string Abbr { get; set; }


        /// <summary>
        /// 车辆类型。
        /// </summary>
        [PropertyMapping(ColumnName = "Initials", Description = "Initials", Length = 255)]
        public virtual string Initials { get; set; }

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "length", Description = "length", Length = 255)]
        public virtual string length { get; set; }

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "width", Description = "width", Length = 255)]
        public virtual string width { get; set; }

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "height", Description = "height", Length = 255)]
        public virtual string height { get; set; }

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "CarType", Description = "CarType", Length = 255)]
        public virtual string CarType { get; set; }
        

        /// <summary>
        /// 获取或设置扩展字段1。
        /// </summary>
        [PropertyMapping(ColumnName = "Value", Description = "Value", Length = 255)]
        public virtual string Value { get; set; }


    }
}