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
    public class FireCutController:BaseApi
    {
        /// <summary>
        /// 火切记录接口，新增火切记录
        /// </summary>
        /// <param name="trk_fc_record">火切记录实体类</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddFireCutRecord(TRK_FC_RECORDMetadata trk_fc_record)
        {
            if (trk_fc_record == null)
            {
                trk_fc_record = new TRK_FC_RECORDMetadata();
            }
            trk_fc_record = SetObjectVal(trk_fc_record);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_fc_record, "TRK_FC_RECORD")));
        }

        /// <summary>
        /// 查询火切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">火切时间(格式 yyyy-MM-dd)</param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<List<TRK_FC_RECORD>> SelFireCutRecord(string BLANK_CARD_ID, string TIME)
        { 
            return Json(FacadeProvide.FacadeFireCut.SelFireCutRecord(BLANK_CARD_ID, TIME));
        }
    }
}
