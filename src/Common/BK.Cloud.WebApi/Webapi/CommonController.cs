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
using System.Web.Http.Results;
using Fireasy.Data.Extensions;
using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.WebApi.Webapi;

namespace BK.Cloud.Web.Webapi
{

    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : BaseApi
    {

        /// <summary>
        /// 查询数据库视图
        /// </summary>
        /// <param name="condition">通用查询对象,件定义</param>
        /// <returns>查询结果</returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<PageResult> QueryData([FromBody]Condition condition)
        {
            condition = SetCondition(condition);
            return Json(FacadeProvide.WebQuery.QueryPageData(condition));
        }


        /// <summary>
        /// 更新表数据
        /// </summary>
        /// <param name="condition">json对象，{datatype:'表名',[字段在数据库名称]:'字段值',[主键名]:'[not]主键值'}</param>
        /// <returns>包含:false,不包含：true</returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> UpData([FromBody]Condition condition)
        {
            condition = SetCondition(condition);
            return Json(FacadeProvide.WebQuery.UpDateObj(condition));
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="condition">json对象，{datatype:'表名',[字段在数据库名称]:'字段值',[主键名]:'[not]主键值'}</param>
        /// <returns>包含:false,不包含：true</returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> AddData([FromBody]Condition condition)
        {
            condition = SetCondition(condition);
            return Json(FacadeProvide.WebQuery.InsertObj(condition));
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="condition">json对象，{datatype:'表名',[字段在数据库名称]:'字段值',[主键名]:'[not]主键值'}</param>
        /// <returns>包含:false,不包含：true</returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public JsonResult<UpMsg> DeleteData([FromBody]Condition condition)
        {
            condition = SetCondition(condition);
            return Json(FacadeProvide.WebQuery.DelObj(condition));
        }

        /// <summary>
        /// 查询数据，返回Json(DataTable)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [HttpPost]
        [HttpGet]
        [LoginCheckFiter(false)]
        public dynamic QueryAllData(Condition condition)
        {
            condition = SetCondition(condition);
            return Json(FacadeProvide.WebQuery.QueryAllData(condition));
        }

    }


}