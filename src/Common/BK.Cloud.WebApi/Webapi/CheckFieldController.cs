using Newtonsoft.Json;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Fireasy.Data.Extensions;
using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.WebApi.Webapi;

namespace BK.Cloud.Web.Webapi
{

    public class CheckFieldController : BaseApi
    {

        /// <summary>
        /// 判断某字段除指定条件外在数据库中是否存在
        /// </summary>
        /// <param name="condition">json对象，{datatype:'表名',[字段在数据库名称]:'字段值',[主键名]:'[not]主键值'}</param>
        /// <returns>包含:false,不包含：true</returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public bool CheckField([FromBody]Condition condition)
        {
            condition = SetQueryCondition(condition);
            condition.showcols = "count(1)";
            var tb = FacadeProvide.WebQuery.QueryAllData(condition);
            int cnt = Convert.ToInt32(tb.Rows[0][0]);
            return cnt == 0;
        }
    }


}




