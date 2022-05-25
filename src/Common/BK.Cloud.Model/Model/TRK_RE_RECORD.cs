using Fireasy.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Model.Data.Model
{
    /// <summary>
    /// 吊销记录表 实体类
    /// </summary>
    [Serializable]
    [EntityMapping("TRK_RE_RECORD", Description = "吊销记录表")]
    public partial class TRK_RE_RECORD : LighEntityObject<TRK_RE_RECORD>
    {
        /// <summary>
        /// 获取或设置钢管物料表ID
        /// </summary>
        [PropertyMapping(ColumnName = "TUBE_ID", Description = "钢管物料表ID")]
        public virtual int TUBE_ID { get; set; }

        /// <summary>
        /// 获取或设置吊销区域(炉前、炉后、穿孔、轧制、减定径)
        /// </summary>
        [PropertyMapping(ColumnName = "RE_REGION", Description = "吊销区域", Length = 30)]
        public virtual string RE_REGION { get; set; }

        /// <summary>
        /// 获取或设置吊销时间
        /// </summary>
        [PropertyMapping(ColumnName = "RE_TIME", Description = "吊销时间")]
        public virtual DateTime RE_TIME { get; set; }

        /// <summary>
        /// 获取或设置吊销原因
        /// </summary>
        [PropertyMapping(ColumnName = "RE_INFO", Description = "吊销原因")]
        public virtual int RE_INFO { get; set; }

        /// <summary>
        /// 获取或设置吊销责任人
        /// </summary>
        [PropertyMapping(ColumnName = "RE_REVISOR", Description = "吊销责任人", Length = 30)]
        public virtual string RE_REVISOR { get; set; }

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
