using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Tools
{

    public class IDFactory
    {

        public static readonly IDFactory Instance = new IDFactory();
        /// <summary> /// Generate a new <see cref="Guid"/> using the comb algorithm. 

        /// </summary> 

        private byte[] GenerateComb()
        {

            byte[] guidArray = Guid.NewGuid().ToByteArray();



            DateTime baseDate = new DateTime(1970, 1, 1);

            DateTime now = DateTime.Now;



            // Get the days and milliseconds which will be used to build    

            //the byte string    

            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);

            TimeSpan msecs = now.TimeOfDay;



            // Convert to a byte array        

            // Note that SQL Server is accurate to 1/300th of a    

            // millisecond so we divide by 3.333333    

            byte[] daysArray = BitConverter.GetBytes(days.Days);

            byte[] msecsArray = BitConverter.GetBytes((long)

              (msecs.TotalMilliseconds / 3.333333));



            // Reverse the bytes to match SQL Servers ordering    

            Array.Reverse(daysArray);

            Array.Reverse(msecsArray);



            // Copy the bytes into the guid    

            Array.Copy(daysArray, daysArray.Length - 2, guidArray,

              guidArray.Length - 6, 2);

            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray,

              guidArray.Length - 4, 4);
            return guidArray;
            //return new Guid(guidArray);
        }

        public long Next()
        {
            return Snowflake.Instance().GetId();
            // var guid = GenerateComb();
            // return BitConverter.ToInt64(guid, 0);
        }


        public static long NextGuidLong()
        {
            return Snowflake.Instance().GetId();
            // byte[] buffer = Guid.NewGuid().ToByteArray();
            //return BitConverter.ToInt64(buffer, 0);
        }


        public ushort NextInt16()
        {
            var guid = GenerateComb();
            return BitConverter.ToUInt16(guid, 0);
        }

        public IDFactory()
        {
            LoadIPGroup();
            InitMillisecond = (ulong)(DateTime.Now - DateTime.Parse("2000-1-1")).TotalMilliseconds;
            mWatch.Restart();
        }

        private ulong mCurrentMillisecond = 0;
        private byte mSeed;
        private System.Diagnostics.Stopwatch mWatch = new System.Diagnostics.Stopwatch();
        public void LoadIPGroup()
        {
            string hostName = Dns.GetHostName();
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Group = (byte)(ip.Address >> 24);
                    break;
                }
            }
        }

        public byte Group
        {
            get;
            set;
        }
        private ulong InitMillisecond;
        public ulong NextULong()
        {
            ulong result = 0;
            System.Threading.SpinLock slock = new System.Threading.SpinLock();
            bool gotLock = false;
            try
            {
                while (!gotLock)
                {
                    slock.Enter(ref gotLock);
                    if (gotLock)
                    {
                        ulong cms = (ulong)mWatch.Elapsed.TotalMilliseconds + InitMillisecond;
                        if (cms != mCurrentMillisecond)
                        {
                            mSeed = 0;
                            mCurrentMillisecond = cms;
                        }
                        result = ((ulong)Group << 58) | (mCurrentMillisecond << 8) | (ulong)mSeed;
                        mSeed++;
                    }
                }
            }
            finally
            {
                if (gotLock) slock.Exit();
            }
            return result;
        }
    }


    /// <summary>
    ///  Snowflake 雪花算法动态生产有规律的ID
    /// </summary>
    public class Snowflake
    {
        private static long machineId; //机器ID
        private static long datacenterId = 0L; //数据ID
        private static long sequence = 0L; //计数从零开始

        private static long twepoch = 687888001020L; //唯一时间随机量

        private static long machineIdBits = 5L; //机器码字节数
        private static long datacenterIdBits = 5L; //数据字节数
        public static long maxMachineId = -1L ^ -1L << (int)machineIdBits; //最大机器ID
        private static long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits); //最大数据ID

        private static long sequenceBits = 12L; //计数器字节数，12个字节用来保存计数码        
        private static long machineIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
        private static long datacenterIdShift = sequenceBits + machineIdBits;

        private static long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits;
        //时间戳左移动位数就是机器码+计数器总字节数+数据字节数

        public static long sequenceMask = -1L ^ -1L << (int)sequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        private static long lastTimestamp = -1L; //最后时间戳

        private static object syncRoot = new object(); //加锁对象
        static readonly Snowflake snowflake = new Snowflake();

        public static Snowflake Instance()
        {
            return snowflake;
            //if (snowflake == null)
            //    snowflake = new Snowflake();
            //return snowflake;
        }

        public Snowflake()
        {
            Snowflakes(0L, -1);
        }

        public Snowflake(long machineId)
        {
            Snowflakes(machineId, -1);
        }

        public Snowflake(long machineId, long datacenterId)
        {
            Snowflakes(machineId, datacenterId);
        }

        private void Snowflakes(long machineId, long datacenterId)
        {
            if (machineId >= 0)
            {
                if (machineId > maxMachineId)
                {
                    throw new Exception("机器码ID非法");
                }
                Snowflake.machineId = machineId;
            }
            if (datacenterId >= 0)
            {
                if (datacenterId > maxDatacenterId)
                {
                    throw new Exception("数据中心ID非法");
                }
                Snowflake.datacenterId = datacenterId;
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns>毫秒</returns>
        private static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            if (timestamp <= lastTimestamp)
            {
                timestamp = GetTimestamp();
            }
            return timestamp;
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            lock (syncRoot)
            {
                long timestamp = GetTimestamp();
                if (Snowflake.lastTimestamp == timestamp)
                {
                    //同一微妙中生成ID
                    sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = GetNextTimestamp(Snowflake.lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID
                    sequence = 0L;
                }
                if (timestamp < lastTimestamp)
                {
                    throw new Exception("时间戳比上一次生成ID时时间戳还小，故异常");
                }
                Snowflake.lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                long Id = ((timestamp - twepoch) << (int)timestampLeftShift)
                          | (datacenterId << (int)datacenterIdShift)
                          | (machineId << (int)machineIdShift)
                          | sequence;
                return Id;
            }
        }
    }
}
