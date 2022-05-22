using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public interface ILogicLongInventory
    {
        /// <summary>
        /// 查询计划是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool PlanExist(TRK_PRO_PLANMetadata data);

        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool BlankExist(TRK_LW_SUMMARYMetadata data);

        /// <summary>
        /// 长坯库库存加1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool AddLongBlankStock(TRK_LW_SUMMARYMetadata data);

        /// <summary>
        /// 长坯库库存加1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SubtractLongBlankStock(TRK_LW_EXWAREHOUSEMetadata data);

        /// <summary>
        /// 查询长坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        DataTable SelLongBlankDetailRecord(string PLATE_ID, string TIME);

        /// <summary>
        /// 查询长坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        DataTable SelLongBlankExRecord(string PLATE_ID, string TIME);
    }
}
