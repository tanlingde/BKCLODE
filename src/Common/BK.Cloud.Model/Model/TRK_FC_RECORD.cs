using Fireasy.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 火切记录表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_FC_RECORD", Description = "火切记录表")]
    public partial class TRK_FC_RECORD : LighEntityObject<TRK_FC_RECORD>
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
        /// 获取或设置火切时间
        /// </summary>
        [PropertyMapping(ColumnName = "FC_TIME", Description = "火切时间")]
        public virtual DateTime FC_TIME { get; set; }

        /// <summary>
        /// 获取或设置火切结果
        /// </summary>
        [PropertyMapping(ColumnName = "FC_RESULT", Description = "火切结果")]
        public virtual string FC_RESULT { get; set; }

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
