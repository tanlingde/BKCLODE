using Fireasy.Data.Entity;
using System;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 长坯库库存详情表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_LW_DETAIL", Description = "长坯库库存详情表")]
    public partial class TRK_LW_DETAIL : LighEntityObject<TRK_LW_DETAIL>
    {
        /// <summary>
        /// 获取或设置长坯库库存主表主键
        /// </summary>
        [PropertyMapping(ColumnName = "LW_SUMMARY_ID", Description = "长坯库库存主表主键")]
        public virtual int LW_SUMMARY_ID { get; set; }
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
        /// 获取或设置坯料重量
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_WEIGHT", Description = "坯料重量")]
        public virtual int BLANK_WEIGHT { get; set; }
        /// <summary>
        /// 获取或设置坯料类型(1:连铸坯，2:轧坯，3:锻坯)
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_TYPE", Description = "坯料类型")]
        public virtual int BLANK_TYPE { get; set; }
        /// <summary>
        /// 获取或设置坯料是否可用(0:不可用，1:可用)
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_IS_USE", Description = "坯料是否可用")]
        public virtual int BLANK_IS_USE { get; set; }
        /// <summary>
        /// 获取或设置坯料是否可用(0:未探伤，1:已探伤)
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_IS_FD", Description = "坯料是否探伤")]
        public virtual int BLANK_IS_FD { get; set; }
        /// <summary>
        /// 获取或设置入库时间
        /// </summary>
        [PropertyMapping(ColumnName = "IN_TIME", Description = "入库时间")]
        public virtual DateTime IN_TIME { get; set; }
        /// <summary>
        /// 获取或设置入库人员
        /// </summary>
        [PropertyMapping(ColumnName = "IN_USER", Description = "入库人员")]
        public virtual int IN_USER { get; set; }
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
