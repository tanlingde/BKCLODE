
using System.ComponentModel.DataAnnotations;

namespace BK.Cloud.Model.Data.Model
{
    //如果要启用实体验证，请使用以下特性，并在 TRK_RE_RECORDMetadata 中定义验证特性。
    [MetadataType(typeof(TRK_RE_RECORDMetadata))]
    public partial class TRK_RE_RECORD
    {
    }
    public class TRK_RE_RECORDMetadata
    {
        /// <summary>
        /// 属性 TUBE_ID 的验证特性。
        /// </summary>
        [Required]
        public object TUBE_ID { get; set; }
        /// <summary>
        /// 属性 RE_REGION 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object RE_REGION { get; set; }
        /// <summary>
        /// 属性 RE_TIME 的验证特性。
        /// </summary>
        [Required]
        public object RE_TIME { get; set; }
        /// <summary>
        /// 属性 RE_INFO 的验证特性。
        /// </summary>
        [Required]
        public object RE_INFO { get; set; }
        /// <summary>
        /// 属性 RE_REVISOR 的验证特性。
        /// </summary>
        [Required]
        [StringLength(30)]
        public object RE_REVISOR { get; set; }
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
