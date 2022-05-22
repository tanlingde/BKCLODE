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
        /// 短坯库入库接口，新增短坯库库存主表记录或短坯库库存数量加1
        /// </summary>
        /// <param name="trk_sw_summary">短坯库库存实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddUpdShortBlankSummary(TRK_SW_SUMMARYMetadata trk_sw_summary)
        {
            if (trk_sw_summary == null)
            {
                trk_sw_summary = new TRK_SW_SUMMARYMetadata();
            }
            trk_sw_summary = SetObjectVal(trk_sw_summary);
            //查询坯料是否已存在,存在库存加1,否则新增记录
            if (FacadeProvide.FacadeShortInventory.BlankExist(trk_sw_summary))
            {
                if (FacadeProvide.FacadeShortInventory.AddShortBlankStock(trk_sw_summary))
                {
                    return Json(new UpMsg() { success = true, code = "200", message = "入库成功" });
                }
                else
                {
                    return Json(new UpMsg() { success = false, message = "入库失败" });
                }
            }
            else
            {
                return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_sw_summary, "TRK_SW_SUMMARY")));
            }
        }

        /// <summary>
        /// 短坯库入库详情接口，新增短坯库库存详情记录
        /// </summary>
        /// <param name="trk_sw_detail">短坯库库存详情实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> AddShortBlankDetail(TRK_SW_DETAILMetadata trk_sw_detail)
        {
            if (trk_sw_detail == null)
            {
                trk_sw_detail = new TRK_SW_DETAILMetadata();
            }
            trk_sw_detail = SetObjectVal(trk_sw_detail);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_sw_detail, "TRK_SW_DETAIL")));
        }

        /// <summary>
        /// 短坯库出库接口，新增短坯库出库记录并短坯库库存减1
        /// </summary>
        /// <param name="trk_sw_exwarehouse">短坯库出库记录实体类</param>
        /// <returns></returns>
        public JsonResult<UpMsg> AddShortBlankEXWarehouse(TRK_SW_EXWAREHOUSEMetadata trk_sw_exwarehouse)
        {
            if (trk_sw_exwarehouse == null)
            {
                trk_sw_exwarehouse = new TRK_SW_EXWAREHOUSEMetadata();
            }
            trk_sw_exwarehouse = SetObjectVal(trk_sw_exwarehouse);
            UpMsg up = WebQuery.Instance.InsertObj(ObjToCondition(trk_sw_exwarehouse, "TRK_SW_EXWAREHOUSE"));
            //出库成功后再减库存
            if (up.success)
            {
                if (FacadeProvide.FacadeShortInventory.SubtractShortBlankStock(trk_sw_exwarehouse))
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
        /// 查询短坯库入库信息
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        public JsonResult<DataTable> SelShortBlankDetailRecord(string PLATE_ID,string TIME)
        {
            return Json(FacadeProvide.FacadeShortInventory.SelShortBlankDetailRecord(PLATE_ID, TIME));
        }

        /// <summary>
        /// 查询短坯库出库信息
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        public JsonResult<DataTable> SelShortBlankExRecord(string PLATE_ID,string TIME)
        {
            return Json(FacadeProvide.FacadeShortInventory.SelShortBlankExRecord(PLATE_ID, TIME));
        }
    }
}
