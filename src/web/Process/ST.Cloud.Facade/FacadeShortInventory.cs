using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Data.Model;
using System.Data;

namespace BK.Cloud.Facade
{
    public class FacadeShortInventory : BaseWeb, IFacadeShortInventory
    {
        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data">短坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool BlankExist(TRK_SW_SUMMARYMetadata data)
        {
            return LogicProvide.LogicShortInventory.BlankExist(data);
        }

        /// <summary>
        /// 短坯库库存加1
        /// </summary>
        /// <param name="data">短坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool AddShortBlankStock(TRK_SW_SUMMARYMetadata data)
        {
            return LogicProvide.LogicShortInventory.AddShortBlankStock(data);
        }

        /// <summary>
        /// 短坯库库存减1
        /// </summary>
        /// <param name="data">短坯库出库记录表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool SubtractShortBlankStock(TRK_SW_EXWAREHOUSEMetadata data)
        {
            return LogicProvide.LogicShortInventory.SubtractShortBlankStock(data);
        }

        /// <summary>
        /// 查询短坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间</param>
        /// <returns></returns>
        public DataTable SelShortBlankDetailRecord(string PLATE_ID, string TIME)
        {
            return LogicProvide.LogicShortInventory.SelShortBlankDetailRecord(PLATE_ID, TIME);
        }

        /// <summary>
        /// 查询短坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间</param>
        /// <returns></returns>
        public DataTable SelShortBlankExRecord(string PLATE_ID, string TIME)
        {
            return LogicProvide.LogicShortInventory.SelShortBlankExRecord(PLATE_ID, TIME);
        }
    }
}
