using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public interface ILogicShortInventory
    {
        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool BlankExist(TRK_SW_SUMMARYMetadata data);

        /// <summary>
        /// 短坯库库存加1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool AddShortBlankStock(TRK_SW_SUMMARYMetadata data);

        /// <summary>
        /// 短坯库库存加1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SubtractShortBlankStock(TRK_SW_EXWAREHOUSEMetadata data);

        /// <summary>
        /// 查询短坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        DataTable SelShortBlankDetailRecord(string PLATE_ID, string TIME);

        /// <summary>
        /// 查询短坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        DataTable SelShortBlankExRecord(string PLATE_ID, string TIME);
    }
}
