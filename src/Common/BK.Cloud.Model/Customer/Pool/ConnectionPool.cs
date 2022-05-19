using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fireasy.Data;

namespace BK.Cloud.Model.Customer
{
    public class ConnectionPool
    {
        private static ConnectionPool cpool = null;//池管理对象
        private static Object objlock = typeof(ConnectionPool);//池管理对象实例
        private int size = 500;//池中连接数
        private int useCount = 0;//已经使用的连接数
        // private readonly List<IDatabase> pool = new List<IDatabase>();//连接保存的集合
        private readonly ConcurrentStack<IDatabase> pool = new ConcurrentStack<IDatabase>();
        private String ConnectionStr = "";//连接字符串

        private readonly object lockobj = new object();

        public ConnectionPool(string consr)
        {
            //数据库连接字符串
            ConnectionStr = "server=localhost;User ID=root;Password=123456;database=test;";
            if (!string.IsNullOrEmpty(consr))
            {
                ConnectionStr = consr;
            }

        }


        #region 创建获取连接池对象
        public static ConnectionPool GetPool(string constr)
        {
            lock (objlock)
            {
                if (cpool == null)
                {
                    cpool = new ConnectionPool(constr);
                }
                return cpool;
            }
        }
        #endregion

        #region 获取池中的连接
        public IDatabase GetDb()
        {
            IDatabase tmp = null;
            lock (lockobj)
            {
                if (useCount <= size)
                {
                    try
                    {
                        //创建连接
                        tmp = CreateConnection();
                    }
                    catch
                    {
                        // ignored
                    }
                    return tmp;
                }
            }
            //可用连接数量大于0
            if (pool.Count > 0)
            {
                lock (pool)
                {
                    pool.TryPop(out tmp);
                }
                //lock (lockobj)
                //{
                //    useCount--;
                //}
            }
            ////连接为null
            //if (tmp == null)
            //{
            //    //达到最大连接数递归调用获取连接否则创建新连接
            //    if (useCount <= size)
            //    {
            //        tmp = GetDb();
            //    }
            //    else
            //    {
            //        tmp = CreateConnection();
            //    }
            //}
            return tmp;
        }

        #endregion

        #region 创建连接
        private IDatabase CreateConnection()
        {
            //创建连接
            IDatabase conn = DatabaseFactory.CreateDatabase(ConnectionStr);
            //pool.Push(conn);
            //conn.Open();
            //可用的连接数加上一个
            lock (lockobj)
            {
                useCount++;
            }
            return conn;
        }
        #endregion

        #region 关闭连接,加连接回到池中
        public void closeConnection(IDatabase con)
        {
            lock (pool)
            {
                if (con != null)
                {
                    //将连接添加在连接池中
                    pool.Push(con);
                }
            }
        }
        #endregion

        #region 目的保证所创连接成功,测试池中连接
        private bool isUserful(IDatabase con)
        {
            //主要用于不同用户
            bool result = true;
            if (con != null)
            {
                string sql = "select 1";//随便执行对数据库操作
                //MySqlCommand cmd = new MySqlCommand(sql, con);
                try
                {
                    con.ExecuteScalar((SqlCommand)sql);
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
        #endregion
    }

}
