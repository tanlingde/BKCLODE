using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;

namespace BK.Cloud.Logic
{
    public interface ILogicLog
    {
     
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model"></param>
        /// <param name="msginfo"></param>
        /// <param name="uid"></param>
        bool WriteLog(string model, string msginfo, long uid);
       
        /// <summary>
        ///  查询日志。根据条件
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        PageResult QueryLog(Condition condition,long? orgid);

    }
}
