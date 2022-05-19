using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Customer;
using Newtonsoft.Json;

namespace BK.Cloud.Facade
{


    public class FacadeDebug : BaseWeb, IFacadeDebug
    {
        

        /// <summary>
        /// 获取调试日志数据
        /// </summary>
        /// <param name="devcode">设备代码</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PageResult GetDebugLog(Condition condition)
        {
            string devcode = condition["devcode"];
            DateTime? beginDate = Convert.ToDateTime(condition["beginDate"]);
            DateTime? endDate = Convert.ToDateTime(condition["endDate"]);
            int page = condition.page;
            int size = condition.rows;
            bool isdesc = condition.order == "desc";
            PageResult pageRes= LogicProvide.LogicDebug.GetDebugLog(devcode, beginDate, endDate, condition["LogMessage"], condition);
            //Logic.LogicProvide.LogicLog.WriteLog("FacadeDebug.GetDebugLog:获取调试日志数据", "传入参数:" + JsonConvert.SerializeObject(condition.where) + ",执行结果:" + JsonConvert.SerializeObject(pageRes), ((LoginInfo)System.Web.HttpContext.Current.Session["LoginInfo"]).UserID ?? 0);
            return pageRes;
        }

        /// <summary>
        /// 获取错误日志数据
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PageResult GetErrorLog(Condition condition)
        {
            DateTime? beginDate = Convert.ToDateTime(condition["beginDate"]);
            DateTime? endDate = Convert.ToDateTime(condition["endDate"]);
            int page = condition.page;
            int size = condition.rows;
            bool isdesc = condition.order == "desc";
            PageResult pageRes= LogicProvide.LogicDebug.GetErrorLog(beginDate, endDate, condition["LogMessage"], condition);
            //Logic.LogicProvide.LogicLog.WriteLog("FacadeDebug.GetErrorLog:获取错误日志数据", "传入参数:" + JsonConvert.SerializeObject(condition.where) + ",执行结果:" + JsonConvert.SerializeObject(pageRes), ((LoginInfo)System.Web.HttpContext.Current.Session["LoginInfo"]).UserID ?? 0);
            return pageRes;
        }

    }
}
