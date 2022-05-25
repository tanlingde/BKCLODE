using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Data.Model;
using System.Collections.Generic;
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
        public TRK_PRO_PLAN PlanExist(TRK_PRO_PLANMetadata data)
        {
            return LogicProvide.LogicLongInventory.PlanExist(data);
        }

        /// <summary>
        /// 新增长坯库待入库记录
        /// </summary>
        /// <param name="data">来料生产计划表实体类</param>
        /// <returns></returns>
        public List<TRK_LW_DETAIL> AddLongBlankStockPending(TRK_PRO_PLANMetadata data)
        {
            return LogicProvide.LogicLongInventory.AddLongBlankStockPending(data);
        }

        /// <summary>
        /// 修改长坯库状态为已入库
        /// </summary>
        /// <param name="data">长坯库库存详情表</param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankState(TRK_LW_DETAILMetadata data)
        {
            return LogicProvide.LogicLongInventory.UpdLongBlankState(data);
        }

        /// <summary>
        /// 修改长坯库状态为已出库
        /// </summary>
        /// <param name="data">长坯库库存详情表</param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankStateEX(TRK_LW_DETAILMetadata data)
        {
            return LogicProvide.LogicLongInventory.UpdLongBlankStateEX(data);
        }

        /// <summary>
        /// 修改长坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="data">长坯库库存详情表</param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankStateDel(TRK_LW_DETAILMetadata data)
        {
            return LogicProvide.LogicLongInventory.UpdLongBlankStateDel(data);
        }

        /// <summary>
        /// 根据卡号修改计划的完成支数/未完成支数
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TYPE">类型</param>
        /// <returns></returns>
        public TRK_PRO_PLAN UpdProPlan(string BLANK_CARD_ID,int TYPE)
        {
            return LogicProvide.LogicLongInventory.UpdProPlan(BLANK_CARD_ID,TYPE);
        }

        /// <summary>
        /// 根据卡号、时间、状态查询长坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public List<TRK_LW_DETAIL> SelLongBankRecord(string BLANK_CARD_ID, string TIME, int state)
        {
            return LogicProvide.LogicLongInventory.SelLongBankRecord(BLANK_CARD_ID, TIME, state);
        }


    }
}
