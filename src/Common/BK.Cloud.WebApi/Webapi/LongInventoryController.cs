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
    public class LongInventoryController:BaseApi
    {
        /// <summary>
        /// 新增到达清单信息
        /// </summary>
        /// <param name="trk_temp_list">到达清单实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> ReachTheListing([FromBody]TRK_TEMP_LISTMetadata trk_temp_list)
        {
            if (trk_temp_list == null)
            {
                trk_temp_list = new TRK_TEMP_LISTMetadata();
            }
            trk_temp_list = SetObjectVal(trk_temp_list);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_temp_list, "TRK_TEMP_LIST")));
        }

        /// <summary>
        /// 新增来料计划或热轧计划信息;(注:如果是来料计划需添加长坯库待入库记录,如果是锯切火切计划添加短坯库待入库记录)
        /// </summary>
        /// <param name="trk_pro_plan">来料生产计划实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddIncomingplan([FromBody]TRK_PRO_PLANMetadata trk_pro_plan)
        {
            if (trk_pro_plan == null)
            {
                trk_pro_plan = new TRK_PRO_PLANMetadata();
            }
            trk_pro_plan = SetObjectVal(trk_pro_plan);
            //不是来料先查询该计划是否已存在，存在删除，再新增(热轧、火切、锯切)
            if (trk_pro_plan.PRO_TYPE.ToString() != "来料计划")
            {
                //查询计划，把原有的计划删除再新增新的计划
                TRK_PRO_PLAN data = FacadeProvide.FacadeLongInventory.PlanExist(trk_pro_plan);
                if (data != null)
                {
                    data = SetObjectVal(data);
                    WebQuery.Instance.DelObj(ObjToCondition(data, "TRK_PRO_PLAN"));//删除计划
                }
                UpMsg msg = WebQuery.Instance.InsertObj(ObjToCondition(trk_pro_plan, "TRK_PRO_PLAN"));//添加计划
                //火切锯切计划添加短坯库待入库记录
                if (trk_pro_plan.PRO_TYPE.ToString() == "火切计划" || trk_pro_plan.PRO_TYPE.ToString() == "锯切计划")
                {
                    //添加短坯库待入库记录
                    List<TRK_SW_DETAIL> list = FacadeProvide.FacadeShortInventory.AddShortBlankStockPending(trk_pro_plan);
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i] = SetObjectVal(list[i]);
                        WebQuery.Instance.InsertObj(ObjToCondition(list[i], "TRK_SW_DETAIL"));
                    }
                }
                return Json(msg);
            }
            else
            {
                UpMsg msg = WebQuery.Instance.InsertObj(ObjToCondition(trk_pro_plan, "TRK_PRO_PLAN"));//添加来料计划
                //添加长坯库待入库记录
                List<TRK_LW_DETAIL> list = FacadeProvide.FacadeLongInventory.AddLongBlankStockPending(trk_pro_plan);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = SetObjectVal(list[i]);
                    WebQuery.Instance.InsertObj(ObjToCondition(list[i], "TRK_LW_DETAIL"));
                }
                return Json(msg);
            }
        }

        /// <summary>
        /// 修改长坯库状态为已入库
        /// </summary>
        /// <param name="trk_lw_detail">长坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdLongBlankState([FromBody]TRK_LW_DETAILMetadata trk_lw_detail)
        {
            if (trk_lw_detail == null)
            {
                trk_lw_detail = new TRK_LW_DETAILMetadata();
            }
            trk_lw_detail = SetObjectVal(trk_lw_detail);
            TRK_LW_DETAIL data = FacadeProvide.FacadeLongInventory.UpdLongBlankState(trk_lw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_LW_DETAIL"));
            //修改长坯库状态为已入库的同时修改来料计划的已完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeLongInventory.UpdProPlan(trk_lw_detail.BLANK_CARD_ID.ToString(),1);
                pro = SetObjectVal(pro);
                WebQuery.Instance.UpDateObj(ObjToCondition(pro, "TRK_PRO_PLAN"));
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改长坯库状态为已出库
        /// </summary>
        /// <param name="trk_lw_detail">长坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdLongBlankStateEX([FromBody] TRK_LW_DETAILMetadata trk_lw_detail)
        {
            if (trk_lw_detail == null)
            {
                trk_lw_detail = new TRK_LW_DETAILMetadata();
            }
            trk_lw_detail = SetObjectVal(trk_lw_detail);
            TRK_LW_DETAIL data = FacadeProvide.FacadeLongInventory.UpdLongBlankStateEX(trk_lw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_LW_DETAIL"));
            //修改长坯库状态为已出库的同时修改热轧或火切锯切计划的已完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeLongInventory.UpdProPlan(trk_lw_detail.BLANK_CARD_ID.ToString(),1);
                pro = SetObjectVal(pro);
                WebQuery.Instance.UpDateObj(ObjToCondition(pro, "TRK_PRO_PLAN"));
            }
            return Json(msg);
        }

        /// <summary>
        /// 修改长坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="trk_lw_detail">长坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> UpdLongBlankStateDel([FromBody] TRK_LW_DETAILMetadata trk_lw_detail)
        {
            if (trk_lw_detail == null)
            {
                trk_lw_detail = new TRK_LW_DETAILMetadata();
            }
            trk_lw_detail = SetObjectVal(trk_lw_detail);
            TRK_LW_DETAIL data = FacadeProvide.FacadeLongInventory.UpdLongBlankStateDel(trk_lw_detail);
            data = SetObjectVal(data);
            UpMsg msg = WebQuery.Instance.UpDateObj(ObjToCondition(data, "TRK_LW_DETAIL"));
            //修改长坯库状态为已删除的同时修改热轧或火切锯切计划的未完成支数
            if (msg.success)
            {
                TRK_PRO_PLAN pro = FacadeProvide.FacadeLongInventory.UpdProPlan(trk_lw_detail.BLANK_CARD_ID.ToString(),2);
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
        public List<TRK_LW_DETAIL> SelLongBankRecord(string BLANK_CARD_ID, string TIME, int state)
        {
            return FacadeProvide.FacadeLongInventory.SelLongBankRecord(BLANK_CARD_ID, TIME, state);
        }

    }
}
