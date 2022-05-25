using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Data.Model;
using System.Collections.Generic;
using System.Data;

namespace BK.Cloud.Facade
{
    public class FacadeShortInventory : BaseWeb, IFacadeShortInventory
    {
        /// <summary>
        /// 新增短坯库待入库记录
        /// </summary>
        /// <param name="data">来料生产计划表实体类</param>
        /// <returns></returns>
        public List<TRK_SW_DETAIL> AddShortBlankStockPending(TRK_PRO_PLANMetadata data)
        {
            return LogicProvide.LogicShortInventory.AddShortBlankStockPending(data);
        }

        /// <summary>
        /// 修改短坯库状态为已入库
        /// </summary>
        /// <param name="data">短坯库库存详情表</param>
        /// <returns></returns>
        public TRK_SW_DETAIL UpdShortBlankState(TRK_SW_DETAILMetadata data)
        {
            return LogicProvide.LogicShortInventory.UpdShortBlankState(data);
        }

        /// <summary>
        /// 修改短坯库状态为已出库
        /// </summary>
        /// <param name="data">短坯库库存详情表</param>
        /// <returns></returns>
        public TRK_SW_DETAIL UpdShortBlankStateEX(TRK_SW_DETAILMetadata data)
        {
            return LogicProvide.LogicShortInventory.UpdShortBlankStateEX(data);
        }

        /// <summary>
        /// 修改短坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="data">短坯库库存详情表</param>
        /// <returns></returns>
        public TRK_SW_DETAIL UpdShortBlankStateDel(TRK_SW_DETAILMetadata data)
        {
            return LogicProvide.LogicShortInventory.UpdShortBlankStateDel(data);
        }

        /// <summary>
        /// 根据卡号修改计划的完成支数
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TYPE">类型</param>
        /// <returns></returns>
        public TRK_PRO_PLAN UpdProPlan(string BLANK_CARD_ID, int TYPE)
        {
            return LogicProvide.LogicShortInventory.UpdProPlan(BLANK_CARD_ID, TYPE);
        }

        /// <summary>
        /// 根据卡号、时间、状态查询短坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public List<TRK_SW_DETAIL> SelShortBankRecord(string BLANK_CARD_ID, string TIME, int state)
        {
            return LogicProvide.LogicShortInventory.SelShortBankRecord(BLANK_CARD_ID, TIME, state);
        }

    }
}
