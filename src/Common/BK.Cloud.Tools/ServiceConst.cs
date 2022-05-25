using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Tools
{
    public class ServiceConst
    {
        #region 数据跟踪队列

        /// <summary>
        /// 炉前提升链数据集
        /// </summary>
        public const string LIFTING_DATA = "LIFTING_DATA";
        /// <summary>
        /// 炉前区1#辊道数据集
        /// </summary>
        public const string FURNAROLLER_DATA1 = "FURNAROLLER_DATA1";
        /// <summary>
        /// 炉前区2#辊道数据集
        /// </summary>
        public const string FURNAROLLER_DATA2 = "FURNAROLLER_DATA2";
        /// <summary>
        /// 炉前区3#辊道数据集
        /// </summary>
        public const string FURNAROLLER_DATA3 = "FURNAROLLER_DATA3";
        /// <summary>
        /// 炉前区4#辊道数据集
        /// </summary>
        public const string FURNAROLLER_DATA4 = "FURNAROLLER_DATA4";
        /// <summary>
        /// 预热区数据集
        /// </summary>
        public const string PREHEAT_DATA = "PREHEAT_DATA";
        /// <summary>
        /// 炉内1#数据集
        /// </summary>
        public const string FURNACE_DATA1 = "FURNACE_DATA1";
        /// <summary>
        /// 炉内2#数据集
        /// </summary>
        public const string FURNACE_DATA2 = "FURNACE_DATA2";
        /// <summary>
        /// 炉内3#数据集
        /// </summary>
        public const string FURNACE_DATA3 = "FURNACE_DATA3";
        /// <summary>
        /// 炉内4#数据集
        /// </summary>
        public const string FURNACE_DATA4 = "FURNACE_DATA4";
        /// <summary>
        /// 炉内5#数据集
        /// </summary>
        public const string FURNACE_DATA5 = "FURNACE_DATA5";
        /// <summary>
        /// 均热1区数据集
        /// </summary>
        public const string SOAKING_DATA1 = "SOAKING_DATA1";
        /// <summary>
        /// 均热2区数据集
        /// </summary>
        public const string SOAKING_DATA2 = "SOAKING_DATA2";
        /// <summary>
        /// 炉后辊道数据集
        /// </summary>
        public const string BEHINDFURNACE_DATA = "BEHINDFURNACE_DATA";
        /// <summary>
        /// 穿孔运输链数据集
        /// </summary>
        public const string TRANSPORTATIONCHAIN_DATA = "TRANSPORTATIONCHAIN_DATA";
        /// <summary>
        /// 穿孔轧制数据集
        /// </summary>
        public const string PIERCING_DATA = "PIERCING_DATA";
        /// <summary>
        /// 轧管轨道数据集
        /// </summary>
        public const string PIPETRACK_DATA = "PIPETRACK_DATA";
        /// <summary>
        /// 扎管机中数据集
        /// </summary>
        public const string PIPE_DATA = "PIPE_DATA";
        /// <summary>
        /// 减定径前辊道数据集
        /// </summary>
        public const string REDUCINGROLLER_DATA = "REDUCINGROLLER_DATA";
        /// <summary>
        /// 减定径机中数据集
        /// </summary>
        public const string REDUCING_DATA = "REDUCING_DATA";
        /// <summary>
        /// 冷床前辊道数据集
        /// </summary>
        public const string COOLINGROLLER_DATA = "COOLINGROLLER_DATA";
        /// <summary>
        /// 冷床1#数据集
        /// </summary>
        public const string COOLING_DATA1 = "COOLING_DATA1";
        /// <summary>
        /// 冷床2#数据集
        /// </summary>
        public const string COOLING_DATA2 = "COOLING_DATA2";
        /// <summary>
        /// 冷床3#数据集
        /// </summary>
        public const string COOLING_DATA3 = "COOLING_DATA3";
        /// <summary>
        /// 矫直前辊道数据集
        /// </summary>
        public const string STRAIGHTENINGROLLER_DATA = "STRAIGHTENINGROLLER_DATA";
        /// <summary>
        /// 数据集矫直机中
        /// </summary>
        public const string STRAIGHTENING_DATA = "STRAIGHTENING_DATA";
        #endregion

        /// <summary>
        /// 修改记录时,旧主键和修改后的主键分割符号
        /// </summary>
        public const string UpKeySplit = "[~KeyField]";

    }
}
