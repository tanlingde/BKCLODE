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
        /// 到达清单接口，新增到达清单信息
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
        /// 来料生产计划接口，新增来料计划或热轧计划信息
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
            //如果是热轧计划先查询该计划是否已存在，存在提示，不存在再新增
            if (trk_pro_plan.PRO_TYPE.ToString() == "热轧计划")
            {
                if (!FacadeProvide.FacadeLongInventory.PlanExist(trk_pro_plan))
                {
                    return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_pro_plan, "TRK_PRO_PLAN")));
                }
                else
                {
                    return Json(new UpMsg() { success = false, message = "计划已存在" });
                }
            }
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_pro_plan, "TRK_PRO_PLAN")));
        }

        /// <summary>
        /// 长坯库入库接口，新增长坯库库存主表记录或长坯库库存数量加1
        /// </summary>
        /// <param name="trk_lw_summary">长坯库库存实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddUpdLongBlankSummary([FromBody]TRK_LW_SUMMARYMetadata trk_lw_summary)
        {
            if (trk_lw_summary == null)
            {
                trk_lw_summary = new TRK_LW_SUMMARYMetadata();
            }
            trk_lw_summary = SetObjectVal(trk_lw_summary);
            //查询坯料是否已存在,存在库存加1,否则新增记录
            if (FacadeProvide.FacadeLongInventory.BlankExist(trk_lw_summary))
            {
                if (FacadeProvide.FacadeLongInventory.AddLongBlankStock(trk_lw_summary))
                {
                    return Json(new UpMsg() { success = true, code = "200",message="入库成功" });
                }
                else
                {
                    return Json(new UpMsg() { success = false, message = "入库失败" });
                }
            }
            else 
            {
                return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_lw_summary, "TRK_LW_SUMMARY")));
            }
        }

        /// <summary>
        /// 长坯库入库详情接口，新增长坯库库存详情记录
        /// </summary>
        /// <param name="trk_lw_detail">长坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> AddLongBlankDetail([FromBody]TRK_LW_DETAILMetadata trk_lw_detail)
        {
            if (trk_lw_detail == null)
            {
                trk_lw_detail = new TRK_LW_DETAILMetadata();
            }
            trk_lw_detail = SetObjectVal(trk_lw_detail);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_lw_detail, "TRK_LW_DETAIL")));
        }

        /// <summary>
        /// 长坯库出库接口，新增长坯库出库记录并长坯库库存减1
        /// </summary>
        /// <param name="trk_lw_exwarehouse">长坯库出库记录实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> AddLongBlankEXWarehouse([FromBody]TRK_LW_EXWAREHOUSEMetadata trk_lw_exwarehouse)
        {
            if (trk_lw_exwarehouse == null)
            {
                trk_lw_exwarehouse = new TRK_LW_EXWAREHOUSEMetadata();
            }
            trk_lw_exwarehouse = SetObjectVal(trk_lw_exwarehouse);
            UpMsg up = WebQuery.Instance.InsertObj(ObjToCondition(trk_lw_exwarehouse, "TRK_LW_EXWAREHOUSE"));
            //出库成功后再减库存
            if (up.success)
            {
                if (FacadeProvide.FacadeLongInventory.SubtractLongBlankStock(trk_lw_exwarehouse))
                {
                    return Json(new UpMsg() { success = true, code = "200", message = "出库成功" });
                }
                else
                {
                    return Json(new UpMsg() { success = false, message = "出库失败" });
                }
            }
            return Json(new UpMsg() { success = false, message = "出库失败" });
        }

        /// <summary>
        /// 查询长坯库入库信息
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        public JsonResult<DataTable> SelLongBlankDetailRecord(string PLATE_ID,string TIME)
        {
            return Json(FacadeProvide.FacadeLongInventory.SelLongBlankDetailRecord(PLATE_ID, TIME));
        }

        /// <summary>
        /// 查询长坯库出库信息
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        public JsonResult<DataTable> SelLongBlankExRecord(string PLATE_ID,string TIME)
        {
            return Json(FacadeProvide.FacadeLongInventory.SelLongBlankExRecord(PLATE_ID, TIME));
        }
    }
}
