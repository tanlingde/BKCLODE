
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
        /// 属性 LW_SUMMARY_ID 的验证特性。
        /// </summary>
        [Required]
        public object LW_SUMMARY_ID { get; set; }
        /// <summary>
        /// 属性 BLANK_CARD_ID 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object BLANK_CARD_ID { get; set; }
        /// <summary>
        /// 属性 FUR_NO 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object FUR_NO { get; set; }
        /// <summary>
        /// 属性 PLATE_ID 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object PLATE_ID { get; set; }
        /// <summary>
        /// 属性 BLANK_SPECS 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_SPECS { get; set; }
        /// <summary>
        /// 属性 BLANK_LENGTH 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_LENGTH { get; set; }
        /// <summary>
        /// 属性 BLANK_WEIGHT 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_WEIGHT { get; set; }
        /// <summary>
        /// 属性 BLANK_TYPE 的验证特性。
        /// </summary>
        [Required]
        public object BLANK_TYPE { get; set; }
        /// <summary>
        /// 属性 BLANK_IS_USE 的验证特性。
        /// </summary>
        public object BLANK_IS_USE { get; set; }
        /// <summary>
        /// 属性 BLANK_IS_FD 的验证特性。
        /// </summary>
        public object BLANK_IS_FD { get; set; }
        /// <summary>
        /// 属性 IN_TIME 的验证特性。
        /// </summary>
        [Required]
        public object IN_TIME { get; set; }
        /// <summary>
        /// 属性 IN_USER 的验证特性。
        /// </summary>
        public object IN_USER { get; set; }
        /// <summary>
        /// 属性 REMARKS 的验证特性。
        /// </summary>
        [StringLength(255)]
        public object REMARKS { get; set; }
        /// <summary>
        /// 属性 CREATE_T 的验证特性。
        /// </summary>
        [Required]
        public object CREATE_T { get; set; }
        /// <summary>
        /// 属性 MODIFY_T 的验证特性。
        /// </summary>
        public object MODIFY_T { get; set; }
    }
}
