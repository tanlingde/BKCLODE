using Fireasy.Data.Entity;
using System;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 长坯库库存主表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_LW_SUMMARY", Description = "长坯库库存主表")]
    public partial class TRK_LW_SUMMARY : LighEntityObject<TRK_LW_SUMMARY>
    {
        /// <summary>
        /// 获取或设置卡号/坯料号
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_CARD_ID", Description = "卡号/坯料号", Length = 30)]
        public virtual string BLANK_CARD_ID { get; set; }
        /// <summary>
        /// 获取或设置炉号
        /// </summary>
        [PropertyMapping(ColumnName = "FUR_NO", Description = "炉号", Length = 30)]
        public virtual string FUR_NO { get; set; }
        /// <summary>
        /// 获取或设置牌号/钢号
        /// </summary>
        [PropertyMapping(ColumnName = "PLATE_ID", Description = "牌号/钢号", Length = 30)]
        public virtual string PLATE_ID { get; set; }
        /// <summary>
        /// 获取或设置坯料规格
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_SPECS", Description = "坯料规格")]
        public virtual int BLANK_SPECS { get; set; }
        /// <summary>
        /// 获取或设置坯料长度
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_LENGTH", Description = "坯料长度")]
        public virtual int BLANK_LENGTH { get; set; }
        /// <summary>
        /// 获取或设置坯料总重量
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_WEIGHT", Description = "坯料总重量")]
        public virtual int BLANK_WEIGHT { get; set; }
        /// <summary>
        /// 获取或设置长坯库库存数量
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_LONG_STOCK", Description = "长坯库库存数量")]
        public virtual int BLANK_LONG_STOCK { get; set; }
        /// <summary>
        /// 获取或设置备注
        /// </summary>
        [PropertyMapping(ColumnName = "REMARKS", Description = "备注", Length = 255)]
        public virtual string REMARKS { get; set; }
        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        [PropertyMapping(ColumnName = "CREATE_T", Description = "创建时间")]
        public virtual DateTime CREATE_T { get; set; }
        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        [PropertyMapping(ColumnName = "MODIFY_T", Description = "修改时间")]
        public virtual DateTime MODIFY_T { get; set; }
    }
}
