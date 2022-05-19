using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BK.Cloud.Facade;
using System.IO;
using System.Data;
using Newtonsoft.Json.Linq;
using Aspose.Cells;
using BK.Cloud.Logic;
using BK.Cloud.Tools;
using BK.Cloud.Model.Customer;
using BK.Cloud.Facade.Interface;
using System.Reflection;
using Fireasy.Common.Extensions;
using Fireasy.Data.Extensions;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Threading;
using Fireasy.Common.Logging;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using BK.Cloud.Logic.Interface;

namespace BK.Cloud.Facade
{
    public class WebQuery : BaseWeb, IWebQuery
    {

        public static readonly WebQuery Instance = new WebQuery();

        private ILogicGeneric logicQuery
        {
            get
            {
                return LogicProvide.LogicGeneric;
            }
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {   // 总是接受  
            return true;
        }


        public DataTable QueryAllData(Condition condition)
        {

            return logicQuery.ConditionQueryAll(condition);
        }

        public PageResult QueryPageData(Condition condition)
        {
            return logicQuery.ConditionQueryData(condition);
        }

        public UpMsg InsertObj(Condition condition)
        {
            UpMsg msg = logicQuery.ConditionInsertObj(condition);
            //LogicProvide.LogicLog.WriteLog("[" + condition.datatype + "][增加记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(condition.where) + ",执行结果:" + (msg.success ? "增加记录成功" : msg.message), LoginInfo.UserID ?? 0);
            return msg;

        }

        public UpMsg UpDateObj(Condition condition)
        {
            UpMsg msg = logicQuery.ConditionUpDateObj(condition);
            //LogicProvide.LogicLog.WriteLog("[" + condition.datatype + "][修改记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(condition.where) + ",执行结果:" + (msg.success ? "修改记录成功" : msg.message), LoginInfo.UserID ?? 0);
            return msg;
        }

        public UpMsg DelObj(Condition condition)
        {
            UpMsg msg = logicQuery.ConditionDeleteObj(condition);
            //LogicProvide.LogicLog.WriteLog("[" + condition.datatype + "][删除记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(condition.where) + ",执行结果:" + (msg.success ? "删除记录成功" : msg.message), LoginInfo.UserID ?? 0);
            return msg;
        }

        public long GetNewID()
        {
            return logicQuery.GetNewID();
        }

    }


   
}
