using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fireasy.Data.Entity;
using BK.Cloud.Model.Data.Model;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
namespace BK.Cloud.Model
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public class DapperOp
    {

        private class DB : EntityContext
        {
            public DB()
                : base(null)
            {

            }

            public DB(string dbname)
                : base(dbname)
            {

            }
        }

        private string constr = "";

        private EntityContext db = null;

        public DapperOp(string constr)
        {
            this.constr = constr;
        }
        public DapperOp()
        {

        }
        public DapperOp(DbContext db)
        {
            this.db = db;
        }

        #region 查询系




        private EntityContext GetDestDb()
        {
            return db ?? (string.IsNullOrEmpty(constr) ? new DB() : new DB(constr));
        }

        /// <summary>
        /// 获取Model-Key为int类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T Get<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.Get<T>(id, transaction, commandTimeout);
                }
            }
            return db.Database.Connection.Get<T>(id, transaction, commandTimeout);

        }
        /// <summary>
        /// 获取Model-Key为long类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(object id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.GetAsync<T>(id, transaction, commandTimeout);
                }
            }
            return db.Database.Connection.GetAsync<T>(id, transaction, commandTimeout);
        }


        /// <summary>
        /// 获取Model-Key为long类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql, ref long total, object param = null, int curpage = 1, int pageSize = 20, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            DataTable dt = new DataTable();
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendFormat("SELECT COUNT(1) FROM ({0}) a;", sql);
            StringBuilder sb2 = new StringBuilder();
            sb2.AppendFormat("SELECT * FROM ({0}) a  LIMIT {1},{2}", sql, (curpage - 1) * pageSize, pageSize);
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    //  using (var reader = conn.Database.Connection.QueryMultiple(sql, param, transaction, commandTimeout)){
                    total = conn.Database.Connection.ExecuteScalar<long>(sb1.ToString(), param, transaction, commandTimeout);
                    dt.Load(conn.Database.Connection.ExecuteReader(sb2.ToString(), param, transaction, commandTimeout));
                    // }
                }
            }
            else
            {
                total = db.Database.Connection.ExecuteScalar<long>(sb1.ToString(), param, transaction, commandTimeout);
                dt.Load(db.Database.Connection.ExecuteReader(sb2.ToString(), param, transaction, commandTimeout));
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T QueryFirstOrDefault<T>(string sql, object param, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout);
                }
            }
            return db.Database.Connection.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout);

        }

        /// <summary>
        /// 获取Model集合（没有Where条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll<T>() where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.GetAll<T>();
                }
            }
            return db.Database.Connection.GetAll<T>();
        }
        #endregion

        #region 增删改
        /// <summary>
        /// 插入一个Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="sqlAdapter"></param>
        /// <returns></returns>
        public long Insert<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.Insert<T>(model, transaction, commandTimeout);
                }
            }
            return db.Database.Connection.Insert<T>(model, transaction, commandTimeout);
        }

        public Task<int> ExecuteSync(string sql)
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.ExecuteAsync(sql);
                }
            }
            return db.Database.Connection.ExecuteAsync(sql);
        }

        public int Execute(string sql)
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    return conn.Database.Connection.Execute(sql);
                }
            }
            return db.Database.Connection.Execute(sql);
        }

        /// <summary>
        /// 更新一个Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public T Update<T>(T model, IDbTransaction transaction = null, int? commandTimeout = null) where T : class, new()
        {
            if (db == null)
            {
                using (var conn = GetDestDb())
                {
                    bool b = conn.Database.Connection.Update<T>(model, transaction, commandTimeout);
                    if (b) { return model; }
                    else { return null; }
                }
            }

            bool b1 = db.Database.Connection.Update<T>(model, transaction, commandTimeout);
            if (b1) { return model; }
            else { return null; }
        }
        #endregion


    }
}
