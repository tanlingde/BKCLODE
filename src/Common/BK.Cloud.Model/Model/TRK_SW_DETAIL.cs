using Fireasy.Data.Entity;
using System;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 短坯库库存详情表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_SW_DETAIL", Description = "短坯库库存详情表")]
    public partial class TRK_SW_DETAIL : LighEntityObject<TRK_SW_DETAIL>
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
        /// 获取或设置状态(0:待入库，1:已入库，2:已出库，3:已删除)
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_STATE", Description = "状态")]
        public virtual int BLANK_STATE { get; set; }

        /// <summary>
        /// 获取或设置在库时间
        /// </summary>
        [PropertyMapping(ColumnName = "TIME", Description = "在库时间")]
        public virtual int TIME { get; set; }

        /// <summary>
        /// 获取或设置备注
        /// </summary>
        [PropertyMapping(ColumnName = "REMARKS", Description = "备注", Length = 255)]
        public virtual string REMARKS { get; set; }

        /// <summary>
        /// 获取或设置创建时间
        /// </summary>
        [PropertyMapping(ColumnName = "CREATE_TIME", Description = "创建时间")]
        public virtual DateTime CREATE_TIME { get; set; }

        /// <summary>
        /// 获取或设置修改时间
        /// </summary>
        [PropertyMapping(ColumnName = "UPDATE_TIME", Description = "修改时间")]
        public virtual DateTime UPDATE_TIME { get; set; }
    }
}
