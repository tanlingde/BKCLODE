using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fireasy.Data;
using BK.Cloud.Model.Customer;
using BK.Cloud.Tools;
using Fireasy.Data.Syntax;
using BK.Cloud.Model.Data.Model;
using Fireasy.Data.Schema;
using System.Net;
using System.Xml;
using System.IO;
//using Dapper;
//using Dapper.Contrib;
//using Dapper.Contrib.Extensions;
namespace BK.Cloud.Logic
{
    public class BusQuery : BaseLogic
    {

        static string msyntax = "";

        static BusQuery()
        {
            if (string.IsNullOrEmpty(msyntax))
            {
                using (var dbcontext = new DbContext())
                {
                    var syntax = dbcontext.Database.Provider.GetService<ISyntaxProvider>();
                    msyntax = syntax.Quote[0] + syntax.Quote[1];
                }
            }
        }

        protected string StColFix
        {
            get
            {
                return "" + msyntax[0];
            }
        }

        protected string EdColFix
        {
            get
            {
                return "" + msyntax[1];
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected PageResult QueryPageData(Condition condition, DbContext DbContext)
        {
            PageResult result = new PageResult();
            string wheresql;
            string groupbystr;
            string orderstr;
            GetSql(condition, out wheresql, out groupbystr, out orderstr);
            int cnt;
            if (condition.datatype.Trim().ToLower().StartsWith("select"))
            {
                condition.datatype = "(" + condition.datatype + ") a";
            }
            string sql = string.Format("select {0} from {1} where 1=1 {2} {3}{4}", condition.showcols,
                                       condition.datatype, wheresql, groupbystr, orderstr);
            DataTable res;
            if (DbContext != null)
            {
                res = QueryPage(sql, condition.page, condition.rows, out cnt, DbContext);
            }
            else
            {
                using (DbContext db = new DbContext())
                {
                    res = QueryPage(sql, condition.page, condition.rows, out cnt, db);
                }
            }
            res = SetTableLongToString(res);
            result.total = cnt;
            result.rows = res;
            return result;
        }


        /// <summary>
        /// 分页查询(需要设置condition.datatype属性值，并且wheret条件已拼成sql传给datatype)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected PageResult QueryPageDataBySql(Condition condition, DbContext DbContext, ParameterCollection par = null)
        {
            PageResult result = new PageResult();

            string orderstr = "";
            orderstr = string.Empty;
            if (!string.IsNullOrEmpty(condition.sort) && !string.IsNullOrEmpty(condition.order))
            {
                orderstr = " order by  ";
                condition.sort = DataValidator.FilterSql(condition.sort);
                condition.order = DataValidator.FilterSql(condition.order);
                if (!string.IsNullOrEmpty(condition.order) && !string.IsNullOrEmpty(condition.sort))
                {
                    orderstr += condition.sort.Replace("|", ",") + " " + condition.order;
                }
            }
            int cnt;
            string datatype = condition.datatype;
            if (datatype.Trim().ToLower().StartsWith("select"))
            {
                datatype = "(" + datatype + ") a";
            }
            string sql = string.Format("select {0} from {1} where 1=1  {2}  ", condition.showcols,
                                       datatype, orderstr);
            var res = QueryPage(sql, condition.page, condition.rows, out cnt, DbContext, par);
            res = SetTableLongToString(res);
            result.total = cnt;
            result.rows = res;
            return result;
        }

        /// <summary>
        /// 将DataTable long转换为String
        /// </summary>
        /// <param name="res"></param>
        protected static DataTable SetTableLongToString(DataTable res)
        {
            if (res != null)
            {
                DataTable newtb = res.Clone();
                for (var i = 0; i < newtb.Columns.Count; i++)
                {
                    if (newtb.Columns[i].DataType == typeof(long))
                    {
                        newtb.Columns[i].DataType = typeof(string);
                    }
                }
                foreach (DataRow row in res.Rows)
                {
                    newtb.LoadDataRow(row.ItemArray, true);
                }
                return newtb;
            }
            return null;
        }


        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected int DeleteData(Condition condition, DbContext DbContext)
        {
            string wheresql;
            string groupbystr;
            string orderstr;
            GetSql(condition, out wheresql, out groupbystr, out orderstr);
            if (string.IsNullOrEmpty(wheresql))
            {
                throw new Exception("删除条件不能为空");
            }

            string sql = string.Format("delete from {0} where 1=1 {1}", condition.datatype, wheresql);
            int res = DbContext.Database.ExecuteNonQuery((SqlCommand)sql);

            //LogicProvide.LogicLog.WriteLog("[" + sql + "][删除记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(condition) + ",删除结果:" + (res > 0 ? "删除记录成功" : "删除记录失败"), ((LoginInfo)System.Web.HttpContext.Current.Session["LoginInfo"]).UserID ?? 0);

            return res;
        }

        /// <summary>
        /// 条件删除
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected UpMsg DeleteDataRet(Condition condition, DbContext DbContext)
        {
            string wheresql;
            string groupbystr;
            string orderstr;
            GetSql(condition, out wheresql, out groupbystr, out orderstr);
            if (string.IsNullOrEmpty(wheresql))
            {
                throw new Exception("删除条件不能为空");
            }

            string sql = string.Format("delete from {0} where 1=1 {1}", condition.datatype, wheresql);
            int res = DbContext.Database.ExecuteNonQuery((SqlCommand)sql);
            //LogicProvide.LogicLog.WriteLog("[" + sql + "][删除记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(condition) + ",删除结果:" + (res > 0 ? "删除记录成功" : "删除记录失败"), ((LoginInfo)System.Web.HttpContext.Current.Session["LoginInfo"]).UserID ?? 0);
            return res > 0 ? UpMsg.SuccessMsg : UpMsg.FailMsg;
        }

        /// <summary>
        /// 条件查找数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected DataTable QueryData(Condition condition, DbContext DbContext)
        {
            string wheresql;
            string groupbystr;
            string orderstr;
            GetSql(condition, out wheresql, out groupbystr, out orderstr);
            if (!condition.datatype.Trim().StartsWith("(") && condition.datatype.IndexOf("select") != -1)
            {
                condition.datatype = "(" + condition.datatype + ") a1";
            }
            var res =
                QueryTable("select " + condition.showcols + " from  " + condition.datatype + " where 1=1 " + wheresql +
                           " " + groupbystr + orderstr, DbContext);
            res = SetTableLongToString(res);


            if (condition.isaddblank && res != null)
            {
                var newrow = res.NewRow();
                for (var i = 0; i < res.Columns.Count; i++)
                {
                    if (res.Columns[i].DataType == typeof(string))
                    {
                        newrow[i] = "";
                    }
                }
                res.Rows.InsertAt(newrow, 0);

            }
            return res;
        }
        /// <summary>
        /// 条件查找数据
        /// </summary>
        /// <param name="querysql"></param>
        /// <returns></returns>
        protected DataTable QueryData(string querysql, DbContext dbContext)
        {
            var res = QueryTable(querysql, dbContext);
            res = SetTableLongToString(res);
            return res;
        }

        //void TestQuery() {
        //    QueryData(new Condition() { showcols = "*", datatype = "tb_org" }); 
        //}

        /// <summary>
        /// 判断是否新数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected bool CheckIsNewData(Condition condition, DbContext DbContext)
        {
            string wheresql;
            string groupbystr;
            string orderstr;
            GetSql(condition, out wheresql, out groupbystr, out orderstr);
            var retval = ExecuteObject<int>("select count(1) from " + condition.datatype + " where 1=1 " + wheresql, DbContext);
            return retval == 0;
        }

        /// <summary>
        /// 根据条件获取 wheresql,groupbysql,orderstr
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="wheresql"></param>
        /// <param name="groupbystr"></param>
        /// <param name="orderstr"></param>
        private void GetSql(Condition condition, out string wheresql, out string groupbystr, out string orderstr)
        {
            if (string.IsNullOrEmpty(condition.datatype))
                throw new Exception("查询集为空");
            wheresql = string.Empty;

            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> ands = new Dictionary<string, List<string>>();
            foreach (string key in condition.where.Keys)
            {
                string startsql = " and (1=2 ";
                string onewhere = startsql;
                string datainfo = condition.where[key];
                if (!string.IsNullOrEmpty(datainfo))
                {
                    int findupindex = datainfo.IndexOf(ServiceConst.UpKeySplit, StringComparison.Ordinal);
                    if (findupindex != -1)
                    {
                        datainfo = datainfo.Replace(ServiceConst.UpKeySplit, "$");
                    }
                    string[] datas = datainfo.Split(',');
                    onewhere = BuildWhereCondition(condition.op, key, groups, ands, datas, onewhere);
                    if (onewhere != startsql)
                    {
                        wheresql += onewhere + ") ";
                    }
                }
            }

            //分组.不同组之间or查询.比如name=k1|aa,name2=k1|33.则生成sql语句(name='aa' or name2='33'),目前只支持=操作
            foreach (var key in groups.Keys)
            {
                //key表示分组标识,如：k1，   keyvals结构：name2,33【字段名，属性值】
                var keyvals = groups[key];
                wheresql += " and (2=1 ";
                string t1 = wheresql;
                keyvals.ForEach(o =>
                    {
                        var valarr = o.Split(',');
                        getonedatawhere(ref t1, valarr[1], valarr[0], null, true);
                    });
                wheresql = t1;
                wheresql += ") ";
            }

            //同名字段间 and组合sql语句
            foreach (var key in ands.Keys)
            {
                var keyvals = ands[key];
                wheresql += " and (1=1 ";
                string t1 = wheresql;
                keyvals.ForEach(o => getonedatawhere(ref t1, o, key, null, false));
                wheresql = t1;
                wheresql += ") ";
            }

            groupbystr = condition.groupbycols;
            if (!string.IsNullOrEmpty(groupbystr))
            {
                groupbystr = DataValidator.FilterSql(groupbystr);
                if (!string.IsNullOrEmpty(groupbystr))
                {
                    groupbystr = "group by " + groupbystr + " ";
                }
            }

            orderstr = string.Empty;
            if (!string.IsNullOrEmpty(condition.sort))
            {
                if (string.IsNullOrEmpty(condition.order))
                {
                    condition.order = "asc";
                }
                orderstr = " order by  ";
                condition.sort = DataValidator.FilterSql(condition.sort);

                if (!string.IsNullOrEmpty(condition.sort) && !condition.sort.Trim().StartsWith("`"))
                {
                    if (condition.sort.IndexOf(",") == -1)
                    {
                        condition.sort = "`" + condition.sort + "`";
                    }
                    else
                    {
                        condition.sort = "`" + condition.sort.Replace(",", "`,`") + "`";
                    }
                }

                condition.order = DataValidator.FilterSql(condition.order);
                if (!string.IsNullOrEmpty(condition.order) && !string.IsNullOrEmpty(condition.sort))
                {
                    orderstr += condition.sort.Replace("|", ",") + " " + condition.order;
                }
            }
            //  ServiceFactory.Fac.Query
            if (condition.datatype.IndexOf("(", System.StringComparison.Ordinal) == -1 &&
                !condition.datatype.StartsWith("[proc]") && condition.datatype.IndexOf(".") == -1)
            {
                if (!condition.datatype.Trim().StartsWith("select"))
                    condition.datatype = StColFix + condition.datatype + EdColFix;
            }
        }


        /// <summary>
        /// 得到单个字段的where条件表达式。
        /// </summary>
        /// <param name="op">综合操作符号</param>
        /// <param name="urlParName">url参数名</param>
        /// <param name="groups">不同组之间or查询.比如name=k1|aa,name2=k1|33.则生成sql语句(name='aa' or name2='33')</param>
        /// <param name="ands">单字段于集合.$符号隔开字符组.A$B表示（urlParName=A and urlParName=B)</param>
        /// <param name="datas">或字段组合.,符号隔开字符</param>
        /// <param name="onewhere">当前的条件集合</param>
        /// <returns>附加当前字段where条件的字段集合</returns>
        private string BuildWhereCondition(string op, string urlParName, Dictionary<string, List<string>> groups,
                                           Dictionary<string, List<string>> ands, string[] datas, string onewhere)
        {
            foreach (string mydata in datas)
            {
                string data = DataValidator.FilterSql(mydata);
                if (string.IsNullOrEmpty(data))
                    continue;
                if (!string.IsNullOrEmpty(data))
                {

                    string[] andarrs = data.Split('$');
                    string[] grouparrs = data.Split('|');
                    //处理分组
                    if (grouparrs.Length > 1)
                    {
                        if (!groups.ContainsKey(grouparrs[0]))
                        {
                            groups.Add(grouparrs[0], new List<string>());
                        }
                        groups[grouparrs[0]].Add(urlParName + "," + grouparrs[1]);
                    }

                    //处理同名formname。and查询。比如时间之间范围查询.可传参数'data=[<=]2014-1-8&[>=]2014-1-4'
                    else if (andarrs.Length > 1)
                    {
                        if (!ands.ContainsKey(urlParName))
                        {
                            ands.Add(urlParName, new List<string>());
                        }
                        for (int i1 = 0; i1 < andarrs.Length; i1++)
                        {
                            ands[urlParName].Add(andarrs[i1]);
                        }
                    }
                    else
                    {
                        getonedatawhere(ref onewhere, data, urlParName, op, true);
                    }
                }
            }
            return onewhere;
        }


        protected void getonedatawhere(ref string onewhere, string data, string urlParName)
        {
            if (data == null)
                return;
            string[] mdatas = data.Split(',');
            foreach (var mdata in mdatas)
            {
                var rmdata = mdata.ToLower();
                getonedatawhere(ref onewhere, rmdata, urlParName, "", false);
            }
        }

        /// <summary>
        /// 获取一个字段的where查询条件。
        /// </summary>
        /// <param name="onewhere">原生where</param>
        /// <param name="data">数据值</param>
        /// <param name="urlParName">字段名</param>
        /// <param name="allop">全部查询标识（query表示模糊查询，queryext表示精确模糊查询）</param>
        /// <param name="isor">是否or连接。</param>
        private void getonedatawhere(ref string onewhere, string data, string urlParName, string allop, bool isor)
        {
            string endsql = allop != "query"
                                ? (string.Format("= '{0}'", data))
                                : string.Format(" like '%{0}%'", data);
            if (allop == "queryext")
            {
                endsql = string.Format(" like '{0}'", data);
            }
            string orbuild = " and ";
            if (isor)
                orbuild = " or ";

            if (data == "NULL" || data == "null" || data == "[null]")
            {
                onewhere += orbuild + urlParName + " is null ";
                return;
            }
            if (data == "NOTNULL" || data == "notnull" || data == "[notnull]")
            {
                onewhere += orbuild + urlParName + " is not null ";
                return;
            }
            if (data.StartsWith("[date]"))
            {
                data = data.Replace("[date]", "");
                if (!string.IsNullOrEmpty(data))
                    onewhere += string.Format(orbuild + " convert(varchar,[" + urlParName + "],120) like '{0}%' ", data);
                return;
            }

            if (data.StartsWith("[query]"))
            {
                data = data.Replace("[query]", "");
                if (!string.IsNullOrEmpty(data))
                    onewhere += string.Format(orbuild + StColFix + urlParName + EdColFix + " like '%{0}%' ", data);
                return;
            }
            if (data.StartsWith("[queryext]"))
            {
                data = data.Replace("[queryext]", "");
                if (!string.IsNullOrEmpty(data))
                    onewhere += string.Format(orbuild + StColFix + urlParName + EdColFix + " like '{0}' ", data);
                return;
            }

            if (data.StartsWith("[not]"))
            {
                data = data.Replace("[not]", "");
                if (!string.IsNullOrEmpty(data))
                    onewhere +=
                        string.Format(
                            orbuild + "(" + StColFix + urlParName + EdColFix + " <> '{0}' or  " + StColFix + urlParName +
                            EdColFix + " is null  " + ")", data);
                return;
            }
            //not in操作。。表示一个表的某个字段数据在另一个表里不存在
            if (data.StartsWith("[notintype]"))
            {
                data = data.Replace("[notintype]", "");
                if (!string.IsNullOrEmpty(data))
                {
                    var tbinfos = data.Split('@');
                    if (tbinfos.Length < 2)
                    {
                        return;
                    }
                    var tbname = tbinfos[0];
                    var colname = tbinfos[1];
                    onewhere +=
                        string.Format(
                            orbuild + StColFix + urlParName + EdColFix +
                            " not in (select {0} from {1} where {0} is not null)  ", colname, tbname);
                }
                return;
            }

            //in操作。。表示一个表的某个字段数据在另一个表里存在
            if (data.StartsWith("[intype]"))
            {
                data = data.Replace("[intype]", "");
                if (!string.IsNullOrEmpty(data))
                {
                    var tbinfos = data.Split('@');
                    if (tbinfos.Length < 2)
                    {
                        return;
                    }

                    var tbname = tbinfos[0];
                    var colname = tbinfos[1];
                    onewhere +=
                        string.Format(
                            orbuild + StColFix + urlParName + EdColFix +
                            " in (select {0} from {1} where {0} is not null)  ", colname, tbname);
                }
                return;
            }
            if (data.StartsWith("[length]"))
            {
                data = data.Replace("[length]", "");
                if (!string.IsNullOrEmpty(data))
                {
                    int len;
                    bool isint = int.TryParse(data, out len);
                    if (!isint)
                        return;
                    onewhere += orbuild + "  length(" + StColFix + urlParName + EdColFix + ")=" + len;
                }
                return;
            }
            if (data.StartsWith("[between]"))
            {
                var c1 = data.Substring(data.IndexOf("[between]"), data.IndexOf("[and]"));
                if (!string.IsNullOrEmpty(c1))
                {
                    c1 = c1.Replace("[between]", "");
                    onewhere += string.Format(orbuild + "  (" + StColFix + urlParName + EdColFix + " >= '{0}' ", c1);
                }
                c1 = data.Substring(data.IndexOf("[and]"));
                if (!string.IsNullOrEmpty(c1))
                {
                    c1 = c1.Replace("[and]", "");
                    onewhere += string.Format(" and  " + StColFix + urlParName + EdColFix + " <= '{0}' ", c1);
                }
                if (!string.IsNullOrEmpty(c1) || !string.IsNullOrEmpty(c1))
                {
                    onewhere += ")";
                }
                return;
            }
            string operation = getOperation(ref data);
            if (!string.IsNullOrEmpty(operation))
            {
                onewhere += string.Format(orbuild + StColFix + urlParName + EdColFix + operation, data);
                //onewhere += string.Format(orbuild + StColFix + urlParName + EdColFix + operation + "  '{0}' ", data);
                return;
            }
            else
            {
                onewhere += orbuild + StColFix + urlParName + EdColFix + endsql;
            }
        }

        /// <summary>
        /// 获取数据的操作符号
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string getOperation(ref string data)
        {
            if (data.StartsWith("[>=]"))
            {
                data = data.Replace("[>=]", "");
                if (!string.IsNullOrEmpty(data))
                    return ">='" + data + "' ";
            }
            if (data.StartsWith("[<=]"))
            {
                data = data.Replace("[<=]", "");
                if (!string.IsNullOrEmpty(data))
                    return "<='" + data + "' ";
            }
            if (data.StartsWith("[>]"))
            {
                data = data.Replace("[>]", "");
                if (!string.IsNullOrEmpty(data))
                    return ">'" + data + "' ";
            }
            if (data.StartsWith("[<]"))
            {
                data = data.Replace("[<]", "");
                if (!string.IsNullOrEmpty(data))
                    return "<'" + data + "' ";
            }
            //if (data.StartsWith("[length]"))
            //{
            //    data = data.Replace("[length]", "");
            //    if (!string.IsNullOrEmpty(data))
            //        return "<'" + data + "' ";
            //}
            if (data.StartsWith("[queryext]"))
            {
                data = data.Replace("[queryext]", "");
                return " like '" + data + "' ";
            }
            if (data.StartsWith("[not]"))
            {
                data = data.Replace("[not]", "");
                return " <> '" + data + "' ";
            }
            if (data.StartsWith("[query]"))
            {
                data = data.Replace("[query]", "");
                return " like '%" + data + "%' ";
            }
            if (data == "NULL" || data == "null")
            {
                return " is null ";
            }

            return string.Empty;
        }


        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <param name="viewName">数据实体名</param>
        /// <param name="jsonCondition">查询条件</param>
        /// <returns></returns>
        protected DataTable QueryAllData(string viewName, string jsonCondition, DbContext db = null)
        {
            if (db == null)
            {
                return RunDb((DbContext) =>
                {

                    Condition condition = GetCondition(jsonCondition);
                    condition.datatype = viewName;
                    return QueryData(condition, DbContext);
                });
            }
            else
            {
                Condition condition = GetCondition(jsonCondition);
                condition.datatype = viewName;
                return QueryData(condition, db);
            }
        }

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <param name="viewName">数据实体名</param>
        /// <param name="jsonCondition">查询条件</param>
        /// <returns></returns>
        protected DataTable QueryAllData(Condition condition, DbContext db = null)
        {
            if (db == null)
            {
                return RunDb((DbContext) =>
                {
                    //Stopwatch watch = new Stopwatch();
                    //watch.Start();
                    var result = QueryData(condition, DbContext);
                    result = SetTableLongToString(result);
                    //watch.Stop();
                    //Fireasy.Common.Logging.DefaultLogger.Instance.Info(string.Format("方法：{0},耗时{1}", "QueryPageData", watch.Elapsed.TotalMilliseconds));
                    return result;
                });
            }
            else
            {
                var result = QueryData(condition, db);
                result = SetTableLongToString(result);
                return result;
            }
        }
        /// <summary>
        /// 查询所有数据,返回UpMsg
        /// </summary>
        /// <param name="viewName">数据实体名</param>
        /// <param name="jsonCondition">查询条件</param>
        /// <returns></returns>
        protected UpMsg QueryAll(Condition condition, DbContext db = null)
        {
            UpMsg upmsg = new UpMsg("查询数据成功", true);

            if (db == null)
            {
                return RunDb((DbContext) =>
                {
                    //Stopwatch watch = new Stopwatch();
                    //watch.Start();
                    var result = QueryData(condition, DbContext);
                    result = SetTableLongToString(result);
                    upmsg.data = result;
                    //watch.Stop();
                    //Fireasy.Common.Logging.DefaultLogger.Instance.Info(string.Format("方法：{0},耗时{1}", "QueryPageData", watch.Elapsed.TotalMilliseconds));
                    return upmsg;
                });
            }
            else
            {
                var result = QueryData(condition, db);
                result = SetTableLongToString(result);
                upmsg.data = result;
                return upmsg;
            }
        }
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页数据量</param>
        /// <param name="viewName">数据实体名</param>
        /// <param name="jsonCondition">查询条件</param>
        /// <returns></returns>
        protected PageResult QueryPageData(Condition condition)
        {
            return RunDb((DbContext) =>
            {
                //Stopwatch watch = new Stopwatch();
                //watch.Start();
                var result = QueryPageData(condition, DbContext);
                //watch.Stop();
                //Fireasy.Common.Logging.DefaultLogger.Instance.Info(string.Format("方法：{0},耗时{1}", "QueryPageData", watch.Elapsed.TotalMilliseconds));
                return result;
            });
        }


        /// <summary>
        /// 获取条件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static Condition GetCondition(string jsonCondition)
        {
            var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            var dic = serializer.Deserialize<Dictionary<string, string>>(jsonCondition);

            List<string> names = dic.Keys.ToList();
            Condition conditon = new Condition();
            Type t = conditon.GetType();
            foreach (string name in names)
            {
                string keyname = name;
                var prop = t.GetProperty(keyname);
                if (prop != null)
                {

                    string val = dic[keyname];
                    if (val == null)
                    {
                        val = dic[keyname.ToLower()];
                    }
                    if (val.IndexOf(",", System.StringComparison.Ordinal) > 0 && keyname != "showcols" && keyname != "groupbycols")
                    {
                        conditon.where[keyname] = val.Split(',')[1];
                        val = val.Split(',')[0];
                    }
                    prop.SetValue(conditon, CommonHelper.ChangeType(prop.PropertyType, val), null);
                }
                else
                {
                    conditon.where[keyname] = dic[keyname];
                }
            }
            return conditon;
        }




        /// <summary>
        /// 删除数据实体
        /// </summary>
        /// <param name="objName">对象名称</param>
        /// <param name="jsonCondition">删除条件，以不为空数据作为删除条件</param>
        /// <returns>1，成功。0.失败</returns>
        protected int DelObj(string objName, string jsonCondition, DbContext db = null)
        {
            var condition = GetCondition(jsonCondition);
            condition.datatype = objName;
            if (condition.where.Count == 0)
            {
                throw new Exception("删除条件不能为空");
            }

            return db != null ? (DeleteData(condition, db)) : RunDb((DbContext) =>
            {
                DeleteData(condition, DbContext);
                return 1;
            });
        }

        /// <summary>
        /// 删除数据实体
        /// </summary>
        /// <param name="condition">删除条件，以不为空数据作为删除条件</param>
        /// <returns>1，成功。0.失败</returns>
        protected int DelObj(Condition condition, DbContext db = null)
        {

            if (string.IsNullOrEmpty(condition.datatype))
            {
                throw new Exception("删除对象不能为空");
            }
            if (condition.where.Count == 0)
            {
                throw new Exception("删除条件不能为空");
            }
            condition.datatype = condition.datatype.Trim();
            return db != null ? (DeleteData(condition, db)) : RunDb((DbContext) =>
            {
                DeleteData(condition, DbContext);
                return 1;
            });
        }



        /// <summary>
        /// 增加记录时，前端调用获取唯一键值
        /// </summary>
        /// <returns></returns>
        public long GetNewID()
        {

            return Convert.ToInt64((new IDFactory()).Next());

        }



        /// <summary>
        /// 单个或批量添加数据
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="jsonData"></param>
        /// <returns>操作成功返回1</returns>
        protected int InsertObj(string objName, string jsonData, DbContext db = null)
        {
            objName = objName.Trim();
            jsonData = jsonData.Replace("\"[null]\"", "null").Replace("\"\"", "null").Trim();

            if (!jsonData.StartsWith("["))
            {
                jsonData = "[" + jsonData + "]";
            }

            var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            var tbname = objName;

            // var tb = DbContext.Database.ExecuteDataTable(new SqlCommand("select * from " + tbname + " where 1=2"));
            DataTable table = serializer.Deserialize<DataTable>(jsonData);
            table.TableName = tbname;
            if (db != null)
            {
                HandleInsert(table, db);
                db.Database.Update(table);
                return 1;
            }

            return RunDb((DbContext) =>
            {

                HandleInsert(table, DbContext);
                DbContext.Database.Update(table);
                return 1;
            });
        }



        /// <summary>
        /// 暂只支持全部新数据倒入，或修改
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public int ImportTable(DataTable datatable)
        {
            datatable.TableName = datatable.TableName.Trim();

            return RunDb((DbContext) =>
            {
                HandleUpdate(datatable, DbContext);
                List<string> conditionsql = new List<string>();
                List<ParameterCollection> tparams = new List<ParameterCollection>();

                var vals = GetNoPrimaryValue(datatable, datatable.TableName, conditionsql, tparams, DbContext);
                DataTable tb = datatable.Clone();
                try
                {

                    DbContext.Database.BeginTransaction();
                    List<Fireasy.Data.Entity.IEntity> objs = new List<Fireasy.Data.Entity.IEntity>();

                    for (var i = 0; i < vals.Count; i++)
                    {

                        string sql = "update " + StColFix + datatable.TableName + EdColFix + "set {0} " + conditionsql[i];
                        string upsql = "";
                        foreach (string val in vals[i].Keys)
                        {
                            upsql += (upsql == "" ? string.Format("{0}=@{0}", val) : string.Format(",{0}=@{0}", val));
                            tparams[i].Add(val, vals[i][val]);
                        }
                        int cnt = 0;
                        if (!string.IsNullOrEmpty(upsql))
                        {
                            cnt = DbContext.Database.ExecuteNonQuery((SqlCommand)string.Format(sql, upsql), parameters: tparams[i]);
                        }

                        if (cnt == 0)
                        {
                            tb.LoadDataRow(datatable.Rows[i].ItemArray, true);

                        }
                    }


                    if (tb.Rows.Count > 0)
                    {
                        foreach (DataRow row in tb.Rows)
                        {
                            row.SetAdded();
                        }
                        DbContext.Database.Update(tb);
                        //Fireasy.Data.Entity.EntityPersisterHelper a1 = new Fireasy.Data.Entity.EntityPersisterHelper(DbContext.Database, dataType, null);
                        //a1.Save(objs);
                    }
                    DbContext.Database.CommitTransaction();
                }
                catch (Exception ex)
                {
                    DbContext.Database.RollbackTransaction();
                    throw ex;
                }
                return 1;
            });
        }

        /// <summary>
        /// 查找表的结构
        /// </summary>
        /// <returns></returns>
        public DataTable QueryTableStructs(string tablename)
        {
            return RunDb((DbContext) =>
            {
                var tb = DbContext.Database.ExecuteDataTable((SqlCommand)"select * from " + tablename + " where 1=2");
                tb.TableName = tablename;
                return tb;
            });
        }

        /// <summary>
        /// 根据条件批量更新
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="jsonData"></param>
        /// <returns>操作成功返回1</returns>
        protected int UpDateObj(string objName, string jsonData)
        {
            return RunDb((dbContext) => UpDateObj(objName, jsonData, dbContext), true);
        }

        /// <summary>
        /// 根据条件批量更新
        /// 更新日期：2017-08-18  修正某个表没有记录更新时,返回更新不成功信息。
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="jsonData"></param>
        /// <returns>操作成功返回1</returns>
        private int UpDateObj(string objName, string jsonData, DbContext db)
        {
            objName = objName.Trim();
            jsonData = jsonData.Replace("\"\"", "[null]");
            List<string> conditionsql = new List<string>();
            List<ParameterCollection> tparams = new List<ParameterCollection>();

            if (!jsonData.Trim().StartsWith("["))
            {
                jsonData = "[" + jsonData + "]";
            }
            DataTable tb = (new Fireasy.Common.Serialization.JsonSerializer()).Deserialize<DataTable>(jsonData);
            HandleUpdate(tb, db);
            var vals = GetNoPrimaryValue(tb, objName, conditionsql, tparams, db);
            int upeffectCnt = 0;
            for (var i = 0; i < vals.Count; i++)
            {
                int rowcnt = 0;
                string tablename = (objName.StartsWith(StColFix) ? "" : StColFix) + objName +
                                   (objName.EndsWith(EdColFix) ? "" : EdColFix);
                string sql = "update " + tablename + " set {0} " + conditionsql[i];
                string queryexistssql = "select count(1) from " + tablename + conditionsql[i];

                ParameterCollection queryparams = new ParameterCollection();
                foreach (var tbparam in tparams[i])
                {
                    queryparams.Add(tbparam.ParameterName, tbparam.Value);
                }
                rowcnt += db.Database.ExecuteScalar<int>((SqlCommand)queryexistssql, queryparams);


                string upsql = "";
                foreach (string val in vals[i].Keys)
                {
                    upsql += (upsql == "" ? string.Format("{0}=@{0}", val) : string.Format(",{0}=@{0}", val));
                    tparams[i].Add(val, vals[i][val]);
                }
                if (!string.IsNullOrEmpty(upsql))
                {
                    rowcnt += db.Database.ExecuteNonQuery((SqlCommand)string.Format(sql, upsql), parameters: tparams[i]);
                    //LogicProvide.LogicLog.WriteLog("[" + sql + "][修改记录]", "传入参数:" + Newtonsoft.Json.JsonConvert.SerializeObject(jsonData) + ",删除结果:" + (rowcnt > 0 ? "删除记录成功" : "删除记录失败"), ((LoginInfo)System.Web.HttpContext.Current.Session["LoginInfo"]).UserID ?? 0);
                }
                upeffectCnt += rowcnt > 0 ? 1 : 0;
            }
            return upeffectCnt;
        }
        /// <summary>
        /// 单个或批量更新数据
        /// </summary>
        /// <param name="objName"></param>
        /// <param name="jsonData"></param>
        /// <returns>操作成功返回1</returns>
        protected int UpDateObjByCondition(string objName, string jsonData)
        {
            objName = objName.Trim();
            var tbname = objName;
            // var tb = DbContext.Database.ExecuteDataTable(new SqlCommand("select * from " + tbname + " where 1=2"));

            DataTable table = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(jsonData);
            table.TableName = tbname;

            table.AcceptChanges();
            RunDb((DbContext) =>
            {
                string conditionsql = SetTbPrimaryKey(table, DbContext);
                foreach (DataRow row in table.Rows)
                {
                    row.SetModified();
                    string sql = "select * from " + StColFix + objName + EdColFix + " where " + conditionsql;
                    ParameterCollection tparams = new ParameterCollection();
                    foreach (var key in table.PrimaryKey)
                    {
                        tparams.Add(new Parameter(key.ColumnName, row[key.ColumnName]));
                    }

                    DataTable tb = DbContext.Database.ExecuteDataTable((SqlCommand)sql, parameters: tparams);
                    foreach (DataRow drow in tb.Rows)
                    {
                        foreach (DataColumn col in table.Columns)
                        {
                            if (row[col.ColumnName] == null || row[col.ColumnName] == DBNull.Value)
                            {
                                row[col.ColumnName] = drow[col.ColumnName];
                            }
                            else if ("[null]".Equals((row[col.ColumnName].ToString())))
                            {
                                row[col.ColumnName] = null;
                            }
                        }
                    }

                }

                DbContext.Database.Update(table);
                table.AcceptChanges();
            });
            return 1;
        }


        public UpMsg ConditionInsertObj(Condition condition, DbContext db = null)
        {
            return InsertObj(condition.datatype, Newtonsoft.Json.JsonConvert.SerializeObject(condition.where), db) > 0 ? UpMsg.SuccessMsg : UpMsg.FailMsg;
        }

        public UpMsg ConditionUpDateObj(Condition condition, DbContext db = null)
        {
            return db == null ? RunDb(dbcontext => UpDateObj(condition.datatype, Newtonsoft.Json.JsonConvert.SerializeObject(condition.where), dbcontext) > 0 ? UpMsg.SuccessMsg : UpMsg.FailMsg) :

                (UpDateObj(condition.datatype, Newtonsoft.Json.JsonConvert.SerializeObject(condition.where), db) > 0 ? UpMsg.SuccessMsg : UpMsg.FailMsg);
        }

        public UpMsg ConditionDeleteObj(Condition condition, DbContext db = null)
        {
            return DelObj(condition.datatype, Newtonsoft.Json.JsonConvert.SerializeObject(condition.where), db) > 0 ? UpMsg.SuccessMsg : UpMsg.FailMsg;
        }


        public PageResult ConditionQueryData(Condition condition, DbContext db = null)
        {

            return QueryPageData(condition, db);
        }


        public DataTable ConditionQueryAll(Condition condition, DbContext db = null)
        {

            return QueryAllData(condition, db);
            // return QueryAllData(condition.datatype, Newtonsoft.Json.JsonConvert.SerializeObject(condition.where), db);
        }

        /// <summary>
        /// 根据wgs84ll（ GPS经纬度）获取地理位置信息
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public string GetAddress(string lng, string lat)
        {
            /*
             coordtype
否
bd09ll
bd09ll 百度经纬度坐标
坐标的类型，目前支持的坐标类型包括：bd09ll（百度经纬度坐标）、gcj02ll（国测局经纬度坐标）、wgs84ll（ GPS经纬度）
location
是
无
38.76623,116.43213 lat<纬度>,lng<经度>
根据经纬度坐标获取地址
pois
否
0
0
是否显示指定位置周边的poi，0为不显示，1为显示。当值为1时，显示周边100米内的poi。
             */
            try
            {
                // string url = @"http://api.map.baidu.com/geocoder/v2/?ak=j0Q1irYcfkYlAhZdjHPrd82O&callback=renderReverse&location={0},{1}&output=xml&pois=0";
                string url = @"http://api.map.baidu.com/geocoder/v2/?ak=j0Q1irYcfkYlAhZdjHPrd82O&callback=renderReverse&location=" + lat + "," + lng + @"&output=xml&pois=0&coordtype=wgs84ll";
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                XmlDocument xmlDoc = new XmlDocument();
                string sendData = xmlDoc.InnerXml;
                byte[] byteArray = Encoding.Default.GetBytes(sendData);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("utf-8"));
                string responseXml = reader.ReadToEnd();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(responseXml);
                string status = xml.DocumentElement.SelectSingleNode("status").InnerText;
                if (status == "0")
                {

                    XmlNodeList nodes = xml.DocumentElement.GetElementsByTagName("formatted_address");
                    if (nodes.Count > 0)
                    {
                        return nodes[0].InnerText;
                    }
                    else
                        return "未获取到位置信息,错误码3";
                }
                else
                {
                    return "未获取到位置信息,错误码1";
                }
            }
            catch (System.Exception ex)
            {
                return "未获取到位置信息,错误码2" + ex.Message + ex.StackTrace;
            }
        }

        /// <summary>
        /// 生成查询语句Sql
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="dataType">数据类型.0,历史数据。1.原始数据。2.调试日志。3.事件信息。4.GPS信息</param>
        /// <returns></returns>
        public string GetHisDataSql(string devcode, string defaulttb = "",
            string begindatestr = "", string endDatestr = "", int dataType = 0, string showcols = "*", string sort = null, string order = null)
        {
            showcols = showcols.Replace("select ", "");
            if (string.IsNullOrEmpty(showcols))
            {
                showcols = "*";
            }
            bool issavebyday = Tools.CommonHelper.GetAppConfig("issavebyday") == "1";
            if (!issavebyday)
                return defaulttb;

            DateTime begindate = !string.IsNullOrEmpty(begindatestr) ? Convert.ToDateTime(begindatestr) : DateTime.Now.Date;


            DateTime endDate = !string.IsNullOrEmpty(endDatestr) ? Convert.ToDateTime(endDatestr) : DateTime.Now;

            //默认情况下，只查一天历史数据
            if (string.IsNullOrEmpty(begindatestr) && !string.IsNullOrEmpty(endDatestr))
            {
                begindate = endDate.Date;
            }
            else if (string.IsNullOrEmpty(endDatestr) && !string.IsNullOrEmpty(begindatestr))
            {
                endDate = begindate.Date.AddDays(1);
            }

            if (begindate < new DateTime(2012, 1, 1))
            {
                begindate = new DateTime(2012, 1, 1);
            }

            StringBuilder buildsql = new StringBuilder();
            string tbname = "systemhistoryday_201806.tb_hisdata_20180620";
            var msort = sort;
            var morder = order;
            msort = msort == "" ? null : msort;
            morder = morder == "" ? null : morder;
            sort = msort ?? "DataTime";
            order = morder ?? "asc";
            string devcodecol = "DevCode";
            string datestr = "DataTime";
            if (sort.ToLower() == "sysdate")
            {
                datestr = "sysdate";
            }
            if (dataType == 1)
            {
                tbname = "cloudlog_201806.tb_hisdataorg_20180620";
            }
            else if (dataType == 2)
            {
                tbname = "cloudlog_201806.tb_debuglog_20180620";
                sort = msort ?? "LogDate";
                devcodecol = "LogEquipment";
                datestr = "LogDate";
            }
            else if (dataType == 3)
            {
                tbname = "systemhistoryday_201806.tb_activehisevent_20180620";
                sort = msort ?? "LastUpdateDate";
                order = morder ?? "desc";
                devcodecol = "EquipmentCode";
                datestr = "BeginDate";
            }
            else if (dataType == 4)
            {
                tbname = "systemhistoryday_201806.tb_gps_20180620";
                sort = msort ?? "RecDate";
                order = morder ?? "desc";
                devcodecol = "DevCode";
                datestr = "RecDate";
            }

            Dictionary<string, string> dbnames = new Dictionary<string, string>();

            //string tbnames = " ";
            string prevdbname = "";
            for (var curdate = begindate; curdate <= endDate; curdate = curdate.AddDays(1))
            {
                string curtbname = tbname.Replace("20180620", curdate.ToString("yyyyMMdd")).Replace("201806", curdate.ToString("yyyyMM"));
                string[] spinfos = curtbname.Split('.');
                string dbname = spinfos[0];
                string mtbname = spinfos[1];
                if (prevdbname != dbname)
                {

                    dbnames.Add(dbname, " ");
                    if (!string.IsNullOrEmpty(prevdbname))
                        dbnames[prevdbname] = dbnames[prevdbname].Remove(dbnames[prevdbname].Length - 1, 1);
                }
                dbnames[dbname] += "'" + mtbname + "',";
                prevdbname = dbname;
            }

            if (!string.IsNullOrEmpty(prevdbname))
                dbnames[prevdbname] = dbnames[prevdbname].Remove(dbnames[prevdbname].Length - 1, 1);



            using (DbContext db = new DbContext())
            {
                List<string> tablenames = new List<string>();
                StringBuilder sb = new StringBuilder("select * from (");
                foreach (var dbname in dbnames.Keys)
                {
                    if (dbnames[dbname].EndsWith(","))
                    {
                        dbnames[dbname] = dbnames[dbname].Remove(dbnames[dbname].Length - 1, 1);
                    }
                    sb.Append("(select table_name from information_schema.TABLES where table_schema='" + dbname + "' and table_name in (" + dbnames[dbname] + ")) union all ");
                    //SqlCommand tbexistssql = "select table_name from information_schema.TABLES where table_schema='" + dbname + "' and table_name in (" + dbnames[dbname] + ")";

                }
                sb.Append("(select table_name from information_schema.TABLES where 1=2)) dbinfo");

                var tblist = db.Database.ExecuteEnumerable<string>(new SqlCommand(sb.ToString())).Select(o => o.ToLower()).ToList();
                tablenames.AddRange(tblist);

                if (order != "desc")
                {
                    for (var curdate = begindate; curdate <= endDate; curdate = curdate.AddDays(1))
                    {
                        string curtbname = tbname.Replace("20180620", curdate.ToString("yyyyMMdd")).Replace("201806", curdate.ToString("yyyyMM"));
                        string[] spinfos = curtbname.Split('.');
                        string dbname = spinfos[0];
                        string mtbname = spinfos[1];
                        if (!tablenames.Contains(mtbname.ToLower()))
                        {
                            continue;
                        }
                        string datewheresql = "";
                        if (!string.IsNullOrEmpty(devcode))
                        {
                            datewheresql = devcodecol + "='" + devcode + "'";
                        }
                        else
                        {
                            datewheresql = " 1=1 ";
                        }

                        if (curdate == begindate)
                        {
                            datewheresql += " and " + datestr + ">'" + curdate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        }
                        else if (curdate.Date == endDate.Date)
                        {
                            datewheresql += " and " + datestr + "<='" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        }
                        string sql = "(select " + showcols + " from " + curtbname + " where " + datewheresql + " order by " + sort + " " + order + ")";
                        buildsql.Append(sql).Append(" union all ");
                    }
                }
                else
                {
                    for (var curdate = endDate; curdate >= begindate; curdate = curdate.AddDays(-1))
                    {
                        string curtbname = tbname.Replace("20180620", curdate.ToString("yyyyMMdd")).Replace("201806", curdate.ToString("yyyyMM"));
                        string[] spinfos = curtbname.Split('.');
                        string dbname = spinfos[0];
                        string mtbname = spinfos[1];
                        if (!tablenames.Contains(mtbname.ToLower()))
                        {
                            continue;
                        }
                        string datewheresql = "";

                        if (!string.IsNullOrEmpty(devcode))
                        {
                            datewheresql = devcodecol + "='" + devcode + "'";
                        }
                        else
                        {
                            datewheresql = " 1=1 ";
                        }

                        if (curdate == endDate)
                        {
                            datewheresql += " and " + datestr + "<'" + curdate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        }
                        else if (curdate.Date == begindate.Date)
                        {
                            datewheresql += " and " + datestr + ">='" + begindate.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        }
                        string sql = "(select " + showcols + " from " + curtbname + " where " + datewheresql + " order by " + sort + " " + order + ")";
                        buildsql.Append(sql).Append(" union all ");
                    }
                }
            }
            if (buildsql.Length > 10)
            {
                buildsql.Remove(buildsql.Length - 10, 10);
                return " (" + buildsql.ToString() + ") ";
            }
            throw new Exception("指定时间段无数据");
        }



    }


}
