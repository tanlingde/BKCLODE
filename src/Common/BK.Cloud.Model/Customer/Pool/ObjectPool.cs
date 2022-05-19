using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Fireasy.Data;
using Fireasy.Data.Provider;
using BK.Cloud.Model.Data.Model;

namespace BK.Cloud.Model.Customer.Pool
{
    public abstract class ObjectPool
    {
        //Last Checkout time of any object from the pool.
        private long lastCheckOut;

        //Hashtable of the check-out objects.
        private static Hashtable locked;

        //Hashtable of available objects
        private static Hashtable unlocked;

        //Clean-Up interval
        protected static long GARBAGE_INTERVAL = 90 * 1000; //90 seconds
        static ObjectPool()
        {
            locked = Hashtable.Synchronized(new Hashtable());
            unlocked = Hashtable.Synchronized(new Hashtable());
        }

        protected ObjectPool()
        {
            lastCheckOut = DateTime.Now.Ticks;

            //Create a Time to track the expired objects for cleanup.
            Timer aTimer = new Timer();
            aTimer.Enabled = true;
            aTimer.Interval = GARBAGE_INTERVAL;
            aTimer.Elapsed += new ElapsedEventHandler(CollectGarbage);
        }

        protected abstract object Create();

        protected abstract bool Validate(object o);

        protected abstract void Expire(object o);

        protected object GetObjectFromPool()
        {
            long now = DateTime.Now.Ticks;
            lastCheckOut = now;
            object o = null;
            try
            {
                foreach (DictionaryEntry myEntry in unlocked)
                {
                    o = myEntry.Key;
                    unlocked.Remove(o);
                    if (Validate(o))
                    {
                        locked.Add(o, now);
                        return o;
                    }
                    else
                    {
                        Expire(o);
                        o = null;
                    }
                }
            }
            catch (Exception) { }
            o = Create();
            locked.Add(o, now);
            return o;
        }

        protected void ReturnObjectToPool(object o)
        {
            if (o != null)
            {
                lock (this)
                {
                    locked.Remove(o);
                    unlocked.Add(o, DateTime.Now.Ticks);
                }
            }
        }

        private void CollectGarbage(object sender, ElapsedEventArgs ea)
        {
            lock (this)
            {
                object o;
                long now = DateTime.Now.Ticks;
                IDictionaryEnumerator e = unlocked.GetEnumerator();

                try
                {
                    while (e.MoveNext())
                    {
                        o = e.Key;

                        if ((now - (long)unlocked[o]) > GARBAGE_INTERVAL)
                        {
                            unlocked.Remove(o);
                            Expire(o);
                            o = null;
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }


    public sealed class DBPool : ObjectPool
    {
        private DBPool() { }

        public static readonly DBPool Instance =
            new DBPool();

        private static string connectionString = "mysqlhis";

        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        protected override object Create()
        {
            //DbContext context=new DbContext();
            //IDatabase conn = new Database(@"Data Source=192.168.3.10;port=3307;database=systemhistory;User Id=root;password=yu-123456;pooling=true;charset=utf8;Max Pool Size=1000;",MySqlProvider.Instance);
            //DatabaseFactory.CreateDatabase(connectionString);
            return DatabaseFactory.CreateDatabase(connectionString);
        }

        protected override bool Validate(object o)
        {
            try
            {
                IDatabase conn = (IDatabase)o;

                return !conn.Connection.State.Equals(ConnectionState.Closed);
            }
            catch (SqlException)
            {
                return false;
            }
        }

        protected override void Expire(object o)
        {
            try
            {
                IDatabase conn = (IDatabase)o;
                conn.Dispose();
            }
            catch (SqlException) { }
        }

        public IDatabase GetDatabase()
        {
            try
            {
                return (IDatabase)base.GetObjectFromPool();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReturnDatabase(IDatabase conn)
        {
            base.ReturnObjectToPool(conn);
        }
    }
}
