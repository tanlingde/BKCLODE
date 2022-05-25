using Fireasy.Data.Entity;
using System;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 来料生产计划表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_PRO_PLAN", Description = "来料生产计划表")]
    public partial class TRK_PRO_PLAN : LighEntityObject<TRK_PRO_PLAN>
    {
        /// <summary>
        /// 获取或设置卡号/坯料号
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_CARD_ID", Description = "卡号/坯料号", Length = 30)]
        public virtual string BLANK_CARD_ID { get; set; }

        /// <summary>
        /// 获取或设置坯料规格
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_SPECS", Description = "坯料规格")]
        public virtual int BLANK_SPECS { get; set; }

        /// <summary>
        /// 获取或设置坯料重量
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_WEIGHT", Description = "坯料重量")]
        public virtual int BLANK_WEIGHT { get; set; }

        /// <summary>
        /// 获取或设置坯料长度
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_LENGTH", Description = "坯料长度")]
        public virtual int BLANK_LENGTH { get; set; }

        /// <summary>
        /// 获取或设置支数
        /// </summary>
        [PropertyMapping(ColumnName = "COUNT", Description = "支数")]
        public virtual int COUNT { get; set; }

        /// <summary>
        /// 获取或设置是否完成(1:已完成、0:未完成)
        /// </summary>
        [PropertyMapping(ColumnName = "PRO_STATE", Description = "是否完成")]
        public virtual int PRO_STATE { get; set; }

        /// <summary>
        /// 获取或设置备注
        /// </summary>
        [PropertyMapping(ColumnName = "REMARKS", Description = "备注", Length = 255)]
        public virtual string REMARKS { get; set; }

        /// <summary>
        /// 获取或设置计划类型(1:来料计划 2:热轧计划 3:锯切计划)
        /// </summary>
        [PropertyMapping(ColumnName = "PRO_TYPE", Description = "计划类型")]
        public virtual int PRO_TYPE { get; set; }

        /// <summary>
        /// 获取或设置已完成支数
        /// </summary>
        [PropertyMapping(ColumnName = "ACCOMPLISH_COUNT", Description = "已完成支数")]
        public virtual int ACCOMPLISH_COUNT { get; set; }

        /// <summary>
        /// 获取或设置未完成支数(吊走下线)
        /// </summary>
        [PropertyMapping(ColumnName = "UNFINISHED_COUNT", Description = "未完成支数")]
        public virtual int UNFINISHED_COUNT { get; set; }

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
