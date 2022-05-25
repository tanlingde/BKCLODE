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
        /// 新增短坯库待入库记录
        /// </summary>
        /// <param name="data">来料生产计划表实体类</param>
        /// <returns></returns>
        List<TRK_SW_DETAIL> AddShortBlankStockPending(TRK_PRO_PLANMetadata data);

        /// <summary>
        /// 修改短坯库状态为已入库
        /// </summary>
        /// <param name="data">短坯库库存详情表实体类</param>
        /// <returns></returns>
        TRK_SW_DETAIL UpdShortBlankState(TRK_SW_DETAILMetadata data);

        /// <summary>
        /// 修改短坯库状态为已出库
        /// </summary>
        /// <param name="data">短坯库库存详情表实体类</param>
        /// <returns></returns>
        TRK_SW_DETAIL UpdShortBlankStateEX(TRK_SW_DETAILMetadata data);

        /// <summary>
        /// 修改短坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="data">短坯库库存详情表实体类</param>
        /// <returns></returns>
        TRK_SW_DETAIL UpdShortBlankStateDel(TRK_SW_DETAILMetadata data);

        /// <summary>
        /// 根据卡号修改计划的完成支数/未完成支数
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TYPE">类型</param>
        /// <returns></returns>
        TRK_PRO_PLAN UpdProPlan(string BLANK_CARD_ID, int TYPE);

        /// <summary>
        /// 根据卡号、时间、状态查询短坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">时间</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        List<TRK_SW_DETAIL> SelShortBankRecord(string BLANK_CARD_ID, string TIME, int state);

    }
}
