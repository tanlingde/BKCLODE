using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Tools;
using System.Web;
namespace BK.Cloud.Logic
{

    public class LogicLog : BusQuery, BK.Cloud.Logic.ILogicLog
    {
        public bool WriteLog(string model, string msginfo, long uid)
        {
            var res = RunDb(db => 
            {
                try
                {
                    string NowStr = DateTime.Now.ToString("yyyyMMdd");
                    string NowDateTimeSTr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string sqlCreateTable = @"SET FOREIGN_KEY_CHECKS=0;
CREATE TABLE IF NOT EXISTS`tb_operationlog{0}` (
  `LogId` bigint(20) NOT NULL COMMENT '日志ID',
  `LogModel` varchar(1000) DEFAULT NULL COMMENT '日志模块',
  `LogMessage` varchar(5000) DEFAULT NULL COMMENT '日志消息',
  `LogUser` bigint(20) DEFAULT NULL COMMENT '日志用户',
  `LastUpdateDate` datetime DEFAULT NULL COMMENT '更新日期',
  PRIMARY KEY (`LogId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='系统用户日志';
SET FOREIGN_KEY_CHECKS=1;";
                    sqlCreateTable = string.Format(sqlCreateTable, NowStr);
                    string sqlInsert = @"Insert into tb_operationlog{0}(LogId,LastUpdateDate,LogModel,LogMessage,LogUser)
values({1},'{2}','{3}','{4}',{5});";
                    sqlInsert = string.Format(sqlInsert, NowStr, (new IDFactory()).Next(), NowDateTimeSTr, model, "[IP:" + GetIp() + "]" + msginfo, uid);
                    db.Database.BeginTransaction();
                    db.Database.ExecuteNonQuery((Fireasy.Data.SqlCommand)sqlCreateTable);
                    db.Database.ExecuteNonQuery((Fireasy.Data.SqlCommand)sqlInsert);
                    db.Database.CommitTransaction();
                    return true;
                }
                catch (Exception ex)
                {
                    db.Database.RollbackTransaction();
                    return false;
                }
            });
            return res;
        }

        public Model.Customer.PageResult QueryLog(Model.Customer.Condition condition, long? orgid)
        {
            string sql = "select a.*,b.UserName,b.OrgID from TB_OperationLog{0} a join tb_user b on a.LogUser=b.UserID";
            
            if (condition.where.ContainsKey("LastUpdateDate"))
            {        
                string strDate = DateTime.Parse(condition.where["LastUpdateDate"].ToString()).ToString("yyyyMMdd");
                sql = string.Format(sql, strDate);
                //condition.where["lastupdatedate"] = DateTime.Parse(condition.where["LastUpdateDate"].ToString()).ToString("yyyy-MM-dd");
                condition.where.Remove("lastupdatedate");
            }
            else
            {
                string strDate = DateTime.Now.ToString("yyyyMMdd");
                sql = string.Format(sql, strDate);
            }
            condition.datatype = sql;
            condition["OrgID"] = orgid.ToString();
            //condition.sort = "LastUpdateDate";
            //condition.order = "desc";
            using (DbContext db = new DbContext())
            {
                try
                {
                    return base.QueryPageData(condition, db);
                }
                catch
                {
                    return new PageResult() { total=0,rows=new System.Data.DataTable()};
                }
            }
        }

        public string GetIp()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return String.Empty;
            }

            //获取代理后的真实IP
            var clientIp = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //判断是否已经取得IP，如果没有则表示没有用代理或其它问题
            if (string.IsNullOrEmpty(clientIp))
            {
                //直接获取IP
                clientIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return clientIp;

        }

        public static class HttpRequestMessageExtensions
        {
            private const string httpContext = "MS_HttpContext";
            private const string RemoteEndpointMessage =
                "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
            private const string OwinContext = "MS_OwinContext";

            public static string GetClientIpAddress(HttpRequestMessage request)
            {

                // Web-hosting. Needs reference to System.Web.dll
                if (request.Properties.ContainsKey(httpContext))
                {
                    dynamic ctx = request.Properties[httpContext];
                    if (ctx != null)
                    {
                        return ctx.Request.UserHostAddress;
                    }
                }

                // Self-hosting. Needs reference to System.ServiceModel.dll. 
                if (request.Properties.ContainsKey(RemoteEndpointMessage))
                {
                    dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                    if (remoteEndpoint != null)
                    {
                        return remoteEndpoint.Address;
                    }
                }

                // Self-hosting using Owin. Needs reference to Microsoft.Owin.dll. 
                if (request.Properties.ContainsKey(OwinContext))
                {
                    dynamic owinContext = request.Properties[OwinContext];
                    if (owinContext != null)
                    {
                        return owinContext.Request.RemoteIpAddress;
                    }
                }

                return null;
            }
        }
    }
}
