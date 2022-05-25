using Fireasy.Data.Entity;
using System;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 到达清单缓存表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_TEMP_LIST", Description = "到达清单缓存表")]
    public partial class TRK_TEMP_LIST : LighEntityObject<TRK_TEMP_LIST>
    {
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
        /// 获取或设置来料方式
        /// </summary>
        [PropertyMapping(ColumnName = "INCOMING_TYPE", Description = "来料方式",Length = 255)]
        public virtual string INCOMING_TYPE { get; set; }

        /// <summary>
        /// 获取或设置到达时间
        /// </summary>
        [PropertyMapping(ColumnName = "INCOMING_TIME", Description = "到达时间")]
        public virtual DateTime INCOMING_TIME { get; set; }

        /// <summary>
        /// 获取或设置发货方
        /// </summary>
        [PropertyMapping(ColumnName = "SENDER", Description = "发货方",Length = 30)]
        public virtual string SENDER { get; set; }

        /// <summary>
        /// 获取或设置车号
        /// </summary>
        [PropertyMapping(ColumnName = "CAR_NO", Description = "车号")]
        public virtual int CAR_NO { get; set; }

        /// <summary>
        /// 获取或设置坯料状态(0:到达未校验，1:到达已校验入库)
        /// </summary>
        [PropertyMapping(ColumnName = "BLANK_STATUS", Description = "坯料状态")]
        public virtual int BLANK_STATUS { get; set; }

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
