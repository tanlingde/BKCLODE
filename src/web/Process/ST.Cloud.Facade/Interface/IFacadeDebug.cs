using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Model.Customer;

namespace BK.Cloud.Facade.Interface
{
    public interface IFacadeDebug
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
        PageResult GetDebugLog(Condition condition);

        /// <summary>
        /// 获取错误日志数据
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="isdesc">是否日期排序。true降序,false升序</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        PageResult GetErrorLog(Condition condition);

     
    }
}
