using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fireasy.Data;
using Fireasy.Data.Provider;
using BK.Cloud.Model;
using BK.Cloud.Model.Data.Model;
using Fireasy.Common.Extensions;
using System.Collections.Concurrent;
using ServiceStack.Redis;
using BK.Cloud.Tools;
using System.Collections;
using Fireasy.Data.Identity;
using Fireasy.Data.Extensions;
using BK.Cloud.Model.Customer;

namespace BK.Cloud.Logic
{
    public class BaseLogic : IDisposable
    {

        //private DbContext _dbContext = null;


        protected DbContext STDBContext
        {
            get
            {
                return new DbContext();
            }
        }

     

        protected T RunDb<T>(Func<DbContext, T> runFunc, bool istransaction = false)
        {
            using (var context = STDBContext)
            {
                try
                {

                    if (runFunc != null)
                    {
                        context.Database.Timeout = 100 * 1000;
                        if (istransaction)
                        {
                            context.BeginTransaction();
                        }
                        T res = runFunc(context);
                        if (istransaction)
                        {
                            context.CommitTransaction();
                        }
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    if (istransaction)
                    {
                        context.RollbackTransaction();
                    }
                    throw ex;
                }
            }
            return default(T);
        }

        protected void RunDb(Action<DbContext> runAction, bool istransaction = false)
        {
            using (var context = STDBContext)
            {
                try
                {

                    if (runAction != null)
                    {
                        if (istransaction)
                        {
                            context.BeginTransaction();
                        }
                        runAction(context);
                        if (istransaction)
                        {
                            context.CommitTransaction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (istransaction)
                    {
                        context.RollbackTransaction();
                    }
                    throw ex;
                }
            }
        }

      

        protected List<IRedisPubSubServer> PubSubs = new List<IRedisPubSubServer>();

      
        public BaseLogic()
        {
            //DbContext content = new DbContext();


        }


        protected DataTable QueryTable(string sql, DbContext dbcontext, ParameterCollection paramArray = null)
        {
            //dbcontext = dbcontext ?? STDBContext;
            return dbcontext.Database.ExecuteDataTable(new SqlCommand(sql), parameters: paramArray);
        }

        protected DataTable QueryPage(string sql, int pageIndex, int pageSize, out int totalcnt, DbContext dbcontext, ParameterCollection paramArray = null)
        {

            //DapperOp dapper = new DapperOp(dbcontext);
            //long total = 0;
            //var tb1 = dapper.GetTable(sql, ref total, paramArray, pageIndex, pageSize);
            //totalcnt = (int)total;
            //return tb1;

            dbcontext = dbcontext ?? STDBContext;
            var page = new DataPager(pageSize, pageIndex - 1);
            var tb = dbcontext.Database.ExecuteDataTable(new SqlCommand(sql), segment: page, parameters: paramArray);
            totalcnt = page.RecordCount;
            return tb;
        }

        protected T ExecuteObject<T>(string sql, DbContext dbcontext, ParameterCollection paramArray = null)
        {
            dbcontext = dbcontext ?? STDBContext;
            return dbcontext.Database.ExecuteScalar<T>(new SqlCommand(sql), paramArray);
        }

        protected int Execute(string sql, DbContext dbcontext, ParameterCollection paramArray = null)
        {
            dbcontext = dbcontext ?? STDBContext;
            return dbcontext.Database.ExecuteNonQuery(new SqlCommand(sql), paramArray);
        }

        /// <summary>
        /// 设置数据库表的主键
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>主键sql条件</returns>
        protected string SetTbPrimaryKey(DataTable table, DbContext dbcontext)
        {
            dbcontext = dbcontext ?? STDBContext;
            var provider = dbcontext.Database.Provider.GetService<Fireasy.Data.Schema.ISchemaProvider>();
            List<DataColumn> primarykeys = new List<DataColumn>();
            BK.Cloud.Model.Customer.Condition condition = new BK.Cloud.Model.Customer.Condition();
            string sqlstr = "1=1 ";
            foreach (var column in provider.GetSchemas<Fireasy.Data.Schema.Column>(dbcontext.Database, s => s.TableName == table.TableName))
            {
                if (column.IsPrimaryKey)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.ColumnName.ToLower() == column.Name.ToLower())
                        {
                            primarykeys.Add(col);
                            sqlstr += string.Format("and {0} in(@" + col.ColumnName + ")", col.ColumnName);
                            break;
                        }
                    }
                }
            }
            table.PrimaryKey = primarykeys.ToArray();
            return sqlstr;
        }

        protected static void HandleInsert(DataTable table, DbContext DbContext = null)
        {

            if (table.Columns.Contains("LastUpdateDate"))
            {
                foreach (DataRow row in table.Rows)
                {
                    row["LastUpdateDate"] = DateTime.Now;
                }
            }

            if (table.Columns.Contains("CreateTime"))
            {
                foreach (DataRow row in table.Rows)
                {
                    row["CreateTime"] = DateTime.Now;
                }
            }

            if (DbContext != null)
            {
                var provider = DbContext.Database.Provider.GetService<Fireasy.Data.Schema.ISchemaProvider>();
                var findtbschemas = provider.GetSchemas<Fireasy.Data.Schema.Column>(DbContext.Database, s => s.TableName.ToLower() == table.TableName.ToLower());
                int primarycnt = 0;
                string primaryKey = "";
                string keyType = "";
                foreach (var column in findtbschemas)
                {
                    if (column.IsPrimaryKey)
                    {
                        primarycnt++;
                        primaryKey = column.Name;
                        keyType = column.DataType;
                        if (!table.Columns.Contains(column.Name))
                        {
                            table.Columns.Add(column.Name);
                        }
                    }

                }

                var delcols = new List<DataColumn>();

                foreach (DataColumn col in table.Columns)
                {
                    if (findtbschemas.FirstOrDefault(o => o.Name.ToLower() == col.ColumnName.ToLower()) == null)
                    {
                        delcols.Add(col);
                    }
                }

                foreach (var delcol in delcols)
                {
                    table.Columns.Remove(delcol);
                }

                if (!string.IsNullOrEmpty(primaryKey) && primarycnt == 1 && keyType == "bigint")
                {
                    if (!table.Columns.Contains(primaryKey))
                    {
                        table.Columns.Add(primaryKey, typeof(long));
                    }
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[primaryKey] == null || row[primaryKey] == DBNull.Value)
                        {
                            row[primaryKey] = (new IDFactory()).Next();
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(primaryKey) && primarycnt == 1 && keyType == "int")
                {
                    if (!table.Columns.Contains(primaryKey))
                    {
                        table.Columns.Add(primaryKey, typeof(int));
                    }
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[primaryKey] == null || row[primaryKey] == DBNull.Value)
                        {
                            var generator = DbContext.Database.Provider.GetService<IGeneratorProvider>();
                            //var generator = DbContext.Provider.GetService<IGeneratorProvider>();
                            row[primaryKey] = generator.GenerateValue(DbContext.Database, table.TableName) + 1;
                        }
                    }
                }
            }
        }

        protected static void HandleUpdate(DataTable table, DbContext DbContext)
        {
            if (table.Columns.Contains("LastUpdateDate"))
            {
                foreach (DataRow row in table.Rows)
                {
                    row["LastUpdateDate"] = DateTime.Now;
                }
            }

            var delcols = new List<DataColumn>();
            var provider = DbContext.Database.Provider.GetService<Fireasy.Data.Schema.ISchemaProvider>();
            var findtbschemas = provider.GetSchemas<Fireasy.Data.Schema.Column>(DbContext.Database, s => s.TableName.ToLower() == table.TableName.ToLower());
            foreach (DataColumn col in table.Columns)
            {
                if (findtbschemas.FirstOrDefault(o => o.Name.ToLower() == col.ColumnName.ToLower()) == null)
                {
                    delcols.Add(col);
                }
            }
            foreach (var delcol in delcols)
            {
                table.Columns.Remove(delcol);
            }
        }

        /// <summary>
        /// 获取非主键的值
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>主键sql条件</returns>
        protected List<Dictionary<string, object>> GetNoPrimaryValue(DataTable table, string objname, List<string> wheresqls, List<ParameterCollection> parameterss, DbContext dbcontext)
        {
            dbcontext = dbcontext ?? STDBContext;
            List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();

            var provider = dbcontext.Database.Provider.GetService<Fireasy.Data.Schema.ISchemaProvider>();
            var findtbschemas = provider.GetSchemas<Fireasy.Data.Schema.Column>(dbcontext.Database, s => s.TableName == objname);
            List<DataColumn> primarykeys = new List<DataColumn>();
            Condition condition = new Condition();
            int keycount = 0;
            foreach (DataRow row in table.Rows)
            {
                string wheresql = "where 1=1 ";
                var parameters = new ParameterCollection();

                Dictionary<string, object> diclist = new Dictionary<string, object>();
                objs.Add(diclist);
                foreach (var column in findtbschemas)
                {
                    if (!column.IsPrimaryKey)
                    {
                        if (column.Name == "LastUpdateDate")
                        {
                            diclist[column.Name] = DateTime.Now;
                            continue;
                        }
                        foreach (DataColumn col in table.Columns)
                        {
                            if (col.ColumnName.ToLower() == column.Name.ToLower())
                            {
                                var val = row[col.ColumnName];
                                if (val != null && val != DBNull.Value)
                                {
                                    diclist[col.ColumnName] = val.ToStringSafely() == "[null]" ? null : val;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataColumn col in table.Columns)
                        {
                            if (col.ColumnName.ToLower() == column.Name.ToLower())
                            {

                                string vals = row[col.ColumnName].ToStringSafely();
                                if (string.IsNullOrEmpty(vals) || string.IsNullOrEmpty(vals.Trim()))
                                {
                                    throw new Exception("更新时主键必须有值");
                                }
                                string[] myvals = vals.Split(',');
                                keycount++;
                                int findupindex = vals.IndexOf(ServiceConst.UpKeySplit, StringComparison.Ordinal);
                                wheresql += "and (1=2 ";
                                if (findupindex == -1)
                                {
                                    int cnt = 0;

                                    foreach (var myval in myvals)
                                    {
                                        cnt++;
                                        if (!string.IsNullOrEmpty(myval) && !string.IsNullOrEmpty(myval.Trim()))
                                        {
                                            wheresql += string.Format("or {0}=@{0}" + cnt + " ", col.ColumnName);
                                            parameters.Add(new Parameter(col.ColumnName + cnt, myval));
                                        }
                                    }
                                }
                                else
                                {
                                    string oldkey = vals.Substring(0, findupindex);
                                    string newkey = vals.Substring(findupindex + ServiceConst.UpKeySplit.Length);
                                    wheresql += string.Format("or {0}=@{0}old", col.ColumnName);
                                    parameters.Add(new Parameter(col.ColumnName + "old", oldkey));
                                    diclist[col.ColumnName] = newkey;
                                }

                                wheresql += " ) ";
                                //diclist[col.ColumnName] = table.Rows[0][col.ColumnName];
                                break;
                            }
                        }
                    }
                }

                if (keycount == 0)
                {
                    throw new Exception("更新时，请必须在主键设置值");
                }

                wheresqls.Add(wheresql);
                parameterss.Add(parameters);
            }
            return objs;
        }

        // Flag for already disposed
        private bool alreadyDisposed = false;
        // Implementation of IDisposable.
        // Call the virtual Dispose method.
        // Suppress Finalization.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 必须，以备程序员忘记了显式调用Dispose方法
        /// </summary>
        ~BaseLogic()
        {
            //必须为false
            Dispose(false);
        }

        // Virtual Dispose method
        protected virtual void Dispose(bool isDisposing)
        {
            // Don't dispose more than once.
            if (alreadyDisposed)
                return;
            if (isDisposing)
            {
                //if (DbContext != null)
                //{
                //    DbContext.Dispose();
                //}
                // elided: free managed resources here.
            }
            // elided: free unmanaged resources here.
            // Set disposed flag:
            alreadyDisposed = true;
        }






  

        public static string ArrayObjToStr(object obj)
        {

            if (!(obj is IEnumerable))
            {
                return "[" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "]";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string QueryObjToStr(object obj)
        {
            //if (obj is IEnumerable)
            //{
            //    throw new Exception("查询集不支持迭代");
            //}
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        private PooledRedisClientManager clientsManager = null;

        private static readonly object obj = new object();

        public PooledRedisClientManager ClientsManager
        {
            get
            {
                if (clientsManager == null)
                {
                    lock (obj)
                    {
                        if (clientsManager != null)
                        {
                            return clientsManager;
                        }
                        string url = CommonHelper.GetAppConfig("redisurl");
                        var urls = url.Split('|');
                        var readhosts = urls[0].Split(';');
                        var writehosts = readhosts;
                        if (urls.Length > 1)
                        {
                            writehosts = urls[1].Split(';');
                        }
                        var res = new PooledRedisClientManager(readhosts, writehosts, new RedisClientManagerConfig() { MaxWritePoolSize = 100, MaxReadPoolSize = 100, AutoStart = true });
                        clientsManager = res;
                        return res;
                    }
                }
                return clientsManager;
            }
        }
    }
}
