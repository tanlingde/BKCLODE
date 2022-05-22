using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Facade.Interface
{
    public interface IFacadeShortInventory
    {
        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data">短坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        bool BlankExist(TRK_SW_SUMMARYMetadata data);

        /// <summary>
        /// 短坯库库存加1
        /// </summary>
        /// <param name="data">短坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        bool AddShortBlankStock(TRK_SW_SUMMARYMetadata data);

        /// <summary>
        /// 短坯库库存减1
        /// </summary>
        /// <param name="data">短坯库出库记录表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        bool SubtractShortBlankStock(TRK_SW_EXWAREHOUSEMetadata data);

        /// <summary>
        /// 查询短坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间</param>
        /// <returns></returns>
        DataTable SelShortBlankDetailRecord(string PLATE_ID, string TIME);

        /// <summary>
        /// 查询短坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间</param>
        /// <returns></returns>
        DataTable SelShortBlankExRecord(string PLATE_ID, string TIME);
    }
}
