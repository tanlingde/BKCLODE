
using System.ComponentModel.DataAnnotations;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TRK_LW_DETAILMetadata 中定义验证特性。
    [MetadataType(typeof(TRK_LW_DETAILMetadata))]
    public partial class TRK_LW_DETAIL
    {
    }
    public class TRK_LW_DETAILMetadata
    {
        /// <summary>
        /// 卡号/坯料号
        /// 属性 BLANK_CARD_ID 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object BLANK_CARD_ID { get; set; }

        /// <summary>
        /// 炉号
        /// 属性 FUR_NO 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object FUR_NO { get; set; }

        /// <summary>
        /// 牌号/钢号
        /// 属性 PLATE_ID 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object PLATE_ID { get; set; }

        /// <summary>
        /// 坯料规格
        /// 属性 BLANK_SPECS 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_SPECS { get; set; }

        /// <summary>
        /// 坯料长度
        /// 属性 BLANK_LENGTH 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_LENGTH { get; set; }

        /// <summary>
        /// 坯料重量
        /// 属性 BLANK_WEIGHT 的验证特性。
        /// </summary>
        public object BLANK_WEIGHT { get; set; }

        /// <summary>
        /// 坯料类型(1:连铸坯，2:轧坯，3:锻坯)
        /// 属性 BLANK_TYPE 的验证特性。
        /// </summary>
        public object BLANK_TYPE { get; set; }

        /// <summary>
        /// 状态(0:待入库，1:已入库，2:已出库，3:已删除)
        /// 属性 BLANK_STATE 的验证特性。
        /// </summary>
        public object BLANK_STATE { get; set; }

        /// <summary>
        /// 坯料是否可用(0:未探伤，1:已探伤)
        /// 属性 BLANK_IS_FD 的验证特性。
        /// </summary>
        public object BLANK_IS_FD { get; set; }

        /// <summary>
        /// 在库时间
        /// 属性 TIME 的验证特性。
        /// </summary>
        public object TIME { get; set; }

        /// <summary>
        /// 备注
        /// 属性 REMARKS 的验证特性。
        /// </summary>
        [StringLength(255)]
        public object REMARKS { get; set; }

        /// <summary>
        /// 创建时间
        /// 属性 CREATE_TIME 的验证特性。
        /// </summary>
        public object CREATE_TIME { get; set; }

        /// <summary>
        /// 修改时间
        /// 属性 UPDATE_TIME 的验证特性。
        /// </summary>
        public object UPDATE_TIME { get; set; }
    }
}
