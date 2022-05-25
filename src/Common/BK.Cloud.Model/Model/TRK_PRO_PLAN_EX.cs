using System.ComponentModel.DataAnnotations;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TRK_PRO_PLANMetadata 中定义验证特性。
    [MetadataType(typeof(TRK_PRO_PLANMetadata))]
    public partial class TRK_PRO_PLAN
    {
    }
    public class TRK_PRO_PLANMetadata
    {
        /// <summary>
        /// 卡号/坯料号
        /// 属性 BLANK_CARD_ID 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object BLANK_CARD_ID { get; set; }

        /// <summary>
        /// 坯料规格
        /// 属性 BLANK_SPECS 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_SPECS { get; set; }

        /// <summary>
        /// 坯料重量
        /// 属性 BLANK_WEIGHT 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_WEIGHT { get; set; }

        /// <summary>
        /// 坯料长度
        /// 属性 BLANK_LENGTH 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_LENGTH { get; set; }

        /// <summary>
        /// 支数
        /// 属性 COUNT 的验证特性。
        /// </summary>
        [Required]
        public object COUNT { get; set; }

        /// <summary>
        /// 是否完成(1:已完成、0:未完成)
        /// 属性 PRO_STATE 的验证特性。
        /// </summary>
        [Required]
        public object PRO_STATE { get; set; }

        /// <summary>
        /// 备注
        /// 属性 REMARKS 的验证特性。
        /// </summary>
        [StringLength(255)]
        public object REMARKS { get; set; }

        /// <summary>
        /// 计划类型(1:来料计划 2:热轧计划 3:锯切计划)
        /// 属性 PRO_TYPE 的验证特性。
        /// </summary>
        [Required]
        public object PRO_TYPE { get; set; }

        /// <summary>
        /// 已完成支数
        /// 属性 ACCOMPLISH_COUNT 的验证特性。
        /// </summary>
        [Required]
        public object ACCOMPLISH_COUNT { get; set; }

        /// <summary>
        /// 未完成支数(吊走下线)
        /// 属性 UNFINISHED_COUNT 的验证特性。
        /// </summary>
        [Required]
        public object UNFINISHED_COUNT { get; set; }

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
