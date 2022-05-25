
using System.ComponentModel.DataAnnotations;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TB_DictionaryMetadata 中定义验证特性。
    [MetadataType(typeof(TRK_TEMP_LISTMetadata))]
    public partial class TRK_TEMP_LIST
    {
    }
    public class TRK_TEMP_LISTMetadata
    {
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
        [Required]
        public object BLANK_WEIGHT { get; set; }

        /// <summary>
        /// 来料方式
        /// 属性 INCOMING_TYPE 的验证特性。
        /// </summary>
        [StringLength(255)]
        public object INCOMING_TYPE { get; set; }

        /// <summary>
        /// 到达时间
        /// 属性 INCOMING_TIME 的验证特性。
        /// </summary>
        public object INCOMING_TIME { get; set; }

        /// <summary>
        /// 发货方
        /// 属性 SENDER 的验证特性。
        /// </summary>
        [StringLength(30)]
        public object SENDER { get; set; }

        /// <summary>
        /// 车号
        /// 属性 CAR_NO 的验证特性。
        /// </summary>
        public object CAR_NO { get; set; }

        /// <summary>
        /// 坯料状态(0:到达未校验，1:到达已校验入库)
        /// 属性 BLANK_STATUS 的验证特性。
        /// </summary>
        public object BLANK_STATUS { get; set; }

        /// <summary>
        /// 创建时间
        /// 属性 CREATE_TIME 的验证特性。
        /// </summary>
        [Required]
        public object CREATE_TIME { get; set; }

        /// <summary>
        /// 修改时间
        /// 属性 UPDATE_TIME 的验证特性。
        /// </summary>
        public object UPDATE_TIME { get; set; }
    }
}
