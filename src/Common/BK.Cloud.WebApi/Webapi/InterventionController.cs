using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Web.App_Start;
using BK.Cloud.WebApi.Webapi;
using System.Web.Http;
using System.Web.Http.Results;

namespace ST.Cloud.WebApi.Webapi
{
    public class InterventionController: BaseApi
    {
        /// <summary>
        /// 人工干预接口，新增人工干预信息
        /// </summary>
        /// <param name="intervention"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> ManualIntervention(TRK_RE_RECORDMetadata trk_re_record)
        {
            if (trk_re_record == null)
            {
                trk_re_record = new TRK_RE_RECORDMetadata();
            }
            trk_re_record = SetObjectVal(trk_re_record);
            return Json(WebQuery.Instance.InsertObj(ObjToCondition(trk_re_record, "TRK_RE_RECORD")));
        }
    }
}
