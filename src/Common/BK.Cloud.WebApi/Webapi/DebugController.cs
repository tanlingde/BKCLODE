using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Web.App_Start;

namespace BK.Cloud.WebApi.Webapi
{

    public class DebugController : BaseApi
    {
        [HttpGet]
        [HttpPost]
        [LoginCheckFiter(false)]
        public dynamic AddOrg([FromBody] TB_DebugLogMetadata Obj)
        {
            if (Obj == null)
            {
                Obj = new TB_DebugLogMetadata();
            }
            Obj = SetObjectVal(Obj);

            return Json(WebQuery.Instance.InsertObj(ObjToCondition(Obj, "TB_Debug")));
        }
    }
}
