using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Data.Model;
using System.Data;

namespace BK.Cloud.Facade
{
    public class FacadeLongInventory : BaseWeb, IFacadeLongInventory
    {
        /// <summary>
        /// 查询计划是否存在
        /// </summary>
        /// <param name="data">来料生产计划表实体类(根据卡号、炉号、牌号、规格、重量、长度、支数)</param>
        /// <returns></returns>
        public bool PlanExist(TRK_PRO_PLANMetadata data)
        {
            return LogicProvide.LogicLongInventory.PlanExist(data);
        }

        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data">长坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool BlankExist(TRK_LW_SUMMARYMetadata data)
        {
            return LogicProvide.LogicLongInventory.BlankExist(data);
        }

        /// <summary>
        /// 长坯库库存加1
        /// </summary>
        /// <param name="data">长坯库库存主表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool AddLongBlankStock(TRK_LW_SUMMARYMetadata data)
        {
            return LogicProvide.LogicLongInventory.AddLongBlankStock(data);
        }

        /// <summary>
        /// 长坯库库存减1
        /// </summary>
        /// <param name="data">长坯库出库记录表实体类(根据卡号、炉号、牌号)</param>
        /// <returns></returns>
        public bool SubtractLongBlankStock(TRK_LW_EXWAREHOUSEMetadata data)
        {
            return LogicProvide.LogicLongInventory.SubtractLongBlankStock(data);
        }

        /// <summary>
        /// 查询长坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间</param>
        /// <returns></returns>
        public DataTable SelLongBlankDetailRecord(string PLATE_ID,string TIME)
        {
            return LogicProvide.LogicLongInventory.SelLongBlankDetailRecord(PLATE_ID, TIME);
        }

        /// <summary>
        /// 查询长坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间</param>
        /// <returns></returns>
        public DataTable SelLongBlankExRecord(string PLATE_ID,string TIME)
        {
            return LogicProvide.LogicLongInventory.SelLongBlankExRecord(PLATE_ID, TIME);
        }
    }
}
