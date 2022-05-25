using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Web.App_Start;
using BK.Cloud.WebApi.Webapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace BK.Cloud.Web.Webapi
{
    public class SawCuttingController:BaseApi
    {
        /// <summary>
        /// 锯切记录接口，新增锯切记录
        /// </summary>
        /// <param name="trk_fc_record">火切记录实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddFireCutRecord(TRK_SC_RECORDMetadata trk_sc_record)
        {
            if (trk_sc_record == null)
            {
                trk_sc_record = new TRK_SC_RECORDMetadata();
            }
            trk_sc_record = SetObjectVal(trk_sc_record);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_sc_record, "TRK_FC_RECORD")));
        }

        /// <summary>
        /// 查询锯切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">锯切时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<List<TRK_SC_RECORD>> SelSawCuttingRecord(string BLANK_CARD_ID, string TIME)
        {
            return Json(FacadeProvide.FacadeSawCutting.SelSawCuttingRecord(BLANK_CARD_ID, TIME));
        }
    }
}
