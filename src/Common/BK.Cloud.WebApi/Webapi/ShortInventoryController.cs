using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Web.App_Start;
using BK.Cloud.WebApi.Webapi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace BK.Cloud.Web.Webapi
{
    public class ShortInventoryController:BaseApi
    {
        /// <summary>
        /// 修改短坯库状态为已入库
        /// </summary>
        /// <param name="trk_sw_detail">短坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdShortBlankState([FromBody] TRK_SW_DETAILMetadata trk_sw_detail)
        {
            if (trk_sw_detail == null)
            {
                trk_sw_detail = new TRK_SW_DETAILMetadata();
            }
            trk_sw_detail = SetObjectVal(trk_sw_detail);
            TRK_SW_DETAIL data = FacadeProvide.FacadeShortInventory.UpdShortBlankState(trk_sw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_SW_DETAIL"));
            //修改短坯库状态为已入库的同时修改来料计划的已完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeShortInventory.UpdProPlan(trk_sw_detail.BLANK_CARD_ID.ToString(),1);
                pro = SetObjectVal(pro);
                WebQuery.Instance.UpDateObj(ObjToCondition(pro, "TRK_PRO_PLAN"));
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改短坯库状态为已出库
        /// </summary>
        /// <param name="trk_sw_detail">短坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdShortBlankStateEX([FromBody] TRK_SW_DETAILMetadata trk_sw_detail)
        {
            if (trk_sw_detail == null)
            {
                trk_sw_detail = new TRK_SW_DETAILMetadata();
            }
            trk_sw_detail = SetObjectVal(trk_sw_detail);
            TRK_SW_DETAIL data = FacadeProvide.FacadeShortInventory.UpdShortBlankStateEX(trk_sw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_SW_DETAIL"));
            //修改短坯库状态为已出库的同时修改热轧生产计划的已完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeShortInventory.UpdProPlan(trk_sw_detail.BLANK_CARD_ID.ToString(),1);
                pro = SetObjectVal(pro);
                WebQuery.Instance.UpDateObj(ObjToCondition(pro, "TRK_PRO_PLAN"));
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改短坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="trk_sw_detail">短坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdShortBlankStateDel([FromBody] TRK_SW_DETAILMetadata trk_sw_detail)
        {
            if (trk_sw_detail == null)
            {
                trk_sw_detail = new TRK_SW_DETAILMetadata();
            }
            trk_sw_detail = SetObjectVal(trk_sw_detail);
            TRK_SW_DETAIL data = FacadeProvide.FacadeShortInventory.UpdShortBlankStateDel(trk_sw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_SW_DETAIL"));
            //修改短坯库状态为已删除的同时修改热轧生产计划的未完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeShortInventory.UpdProPlan(trk_sw_detail.BLANK_CARD_ID.ToString(),2);
                pro = SetObjectVal(pro);
                WebQuery.Instance.UpDateObj(ObjToCondition(pro, "TRK_PRO_PLAN"));
            }
            return Json(msg);
        }

        /// <summary>
        /// 根据卡号、时间、状态查询长坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">时间</param>
        /// <param name="state">状态(待入库、入库、出库)</param>
        /// <returns></returns>
        public List<TRK_SW_DETAIL> SelShortBankRecord(string BLANK_CARD_ID, string TIME, int state)
        {
            return FacadeProvide.FacadeShortInventory.SelShortBankRecord(BLANK_CARD_ID, TIME, state);
        }
    }
}
