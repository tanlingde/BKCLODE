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
        TRK_PRO_PLAN PlanExist(TRK_PRO_PLANMetadata data);

        /// <summary>
        /// 新增长坯库待入库记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        List<TRK_LW_DETAIL> AddLongBlankStockPending(TRK_PRO_PLANMetadata data);

        /// <summary>
        /// 修改长坯库状态为已入库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TRK_LW_DETAIL UpdLongBlankState(TRK_LW_DETAILMetadata data);

        /// <summary>
        /// 修改长坯库状态为已出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TRK_LW_DETAIL UpdLongBlankStateEX(TRK_LW_DETAILMetadata data);

        /// <summary>
        /// 修改长坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TRK_LW_DETAIL UpdLongBlankStateDel(TRK_LW_DETAILMetadata data);

        /// <summary>
        /// 根据卡号修改计划的完成支数/未完成支数
        /// </summary>
        /// <param name="BLANK_CARD_ID"></param>
        /// <param name="TYPE"></param>
        /// <returns></returns>
        TRK_PRO_PLAN UpdProPlan(string BLANK_CARD_ID, int TYPE);

        /// <summary>
        /// 根据卡号、时间、状态查询长坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID"></param>
        /// <param name="TIME"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        List<TRK_LW_DETAIL> SelLongBankRecord(string BLANK_CARD_ID, string TIME, int state);

    }
}
