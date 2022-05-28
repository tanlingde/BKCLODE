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
    public class RedisManageController: BaseApi
    {
        /// <summary>
        /// 获取redis集合根据Key
        /// </summary>
        /// <param name="devkey"></param>
        /// <returns></returns>
        [LoginCheckFiter(false)]
        [HttpGet]
        [HttpPost]
        public dynamic GetRealCommond([FromBody] Condition account)
        {
            account = SetCondition(account);
            string devkey = account["devkey"];
            var data = FacadeProvide.FacadeRedisManage.GetRedisLW(devkey);
            return Json(data);
        }
    }
}
