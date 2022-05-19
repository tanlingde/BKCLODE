/*********************************************************
 * 开发人员：admin
 * 创建时间：5/22/2013 10:44:10 AM
 * 文件信息：CommonHelper
 * 描述说明：
 * *******************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Management;

namespace BK.Cloud.Tools
{
    public static class CommonHelper
    {
        /// <summary>
        /// 根据文件后缀来获取MIME类型字符串
        /// </summary>
        /// <param name="extension">文件后缀</param>
        /// <returns></returns>
        public static string GetMimeType(string extension)
        {
            string mime = string.Empty;
            extension = extension.ToLower();
            switch (extension)
            {
                case ".avi": mime = "video/x-msvideo"; break;
                case ".bin":
                case ".exe":
                case ".msi":
                case ".dll":
                case ".class": mime = "application/octet-stream"; break;
                case ".csv": mime = "text/comma-separated-values"; break;
                case ".html":
                case ".htm":
                case ".shtml": mime = "text/html"; break;
                case ".css": mime = "text/css"; break;
                case ".js": mime = "text/javascript"; break;
                case ".doc":
                case ".dot":
                case ".docx": mime = "application/msword"; break;
                case ".xla":
                case ".xls":
                case ".xlsx": mime = "application/msexcel"; break;
                case ".ppt":
                case ".pptx": mime = "application/mspowerpoint"; break;
                case ".gz": mime = "application/gzip"; break;
                case ".gif": mime = "image/gif"; break;
                case ".bmp": mime = "image/bmp"; break;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                case ".png": mime = "image/jpeg"; break;
                case ".mpeg":
                case ".mpg":
                case ".mpe":
                case ".wmv": mime = "video/mpeg"; break;
                case ".mp3":
                case ".wma": mime = "audio/mpeg"; break;
                case ".pdf": mime = "application/pdf"; break;
                case ".rar": mime = "application/octet-stream"; break;
                case ".txt": mime = "text/plain"; break;
                case ".7z":
                case ".z": mime = "application/x-compress"; break;
                case ".zip": mime = "application/x-zip-compressed"; break;
                default:
                    mime = "application/octet-stream";
                    break;
            }
            return mime;
        }
        //加密解密函数
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            string key = (string)settingsReader.GetValue("SecurityKey",
                                                             typeof(String));

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = (string)settingsReader.GetValue("SecurityKey",
                                                         typeof(String));

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /// <summary>
        /// 检查含有,分隔符的字符串中是否有指定的目标字符串
        /// </summary>
        /// <param name="strSplit">未分割的字符串</param>
        /// <param name="split">分割符号</param>
        /// <param name="targetValue">目标字符串</param>
        /// <returns></returns>
        public static bool ISStringIncludeTargetValue(string strSplit, char split, string targetValue)
        {
            string[] strList = strSplit.Split(split);
            foreach (string str in strList)
            {
                if (targetValue == str)
                    return true;
            }
            return false;
        }
        #region 编码

        /// <summary>
        /// Encodes non-US-ASCII characters in a string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Determines if the character needs to be encoded.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Encodes a non-US-ASCII character.
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }

            return builder.ToString();
        }


        #endregion


        /// <summary>
        /// 将objectd对象转换为指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ChangeType(Type type, object value)
        {
          
            //var val = value as string;
            //if (val != null && !type.IsPrimitive)
            //{
            //    if (val.ToLower() == "null")
            //    {
            //        return null;
            //    }
            //}

            if ((value == null) && type.IsGenericType)
            {

                return Activator.CreateInstance(type);

            }
            if ((value as string) == string.Empty && type.IsGenericType)
                return null;

            if (value == null)
            {

                return null;

            }

            if (type == value.GetType())
            {

                return value;

            }

            if (type.IsEnum)
            {

                if (value is string)
                {

                    return Enum.Parse(type, value as string);

                }

                return Enum.ToObject(type, value);

            }

            if (!type.IsInterface && type.IsGenericType)
            {

                Type type1 = type.GetGenericArguments()[0];

                object obj1 = ChangeType(type1, value);
                if (obj1 == null)
                    return null;
                return Activator.CreateInstance(type, new object[] { obj1 });
            }

            if ((value is string) && (type == typeof(Guid)))
            {

                return new Guid(value as string);

            }

            if ((value is string) && (type == typeof(Version)))
            {

                return new Version(value as string);

            }

            if (!(value is IConvertible))
            {

                return value;

            }
            if (type.IsPrimitive && value is string && value.ToString() == "")
            {
                return null;
            }
            if (type == typeof(bool))
            {
                if ((value as string) == "1")
                {
                    return Convert.ChangeType("true", type);
                }
                else if ((value as string) == "0")
                {
                    return Convert.ChangeType("false", type);
                }
            }
            return Convert.ChangeType(value, type);

        }



        /// <summary>
        /// 将其他日期格式的字符串转换为ISo8601
        /// </summary>
        /// <param name="dt">其他日期格式字符串</param>
        /// <param name="defaultdt">转换不成功时,默认日期</param>
        /// <returns>ISo8601日期字符串</returns>
        public static string DateTimeToIso8601(string dt, DateTime? defaultdt = null)
        {
            DateTime bgdate;
            bool isbgdate = DateTime.TryParse(dt, out bgdate);
            if (!isbgdate)
            {
                bgdate = defaultdt ?? new DateTime(2010, 1, 1);
            }
            return bgdate.ToString("yyyy-MM-ddTHH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
            //return bgdate.ToString("yyyy-MM-ddTHH:mm:ss.fff+08:00", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 将日期转换为ISo8601
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>ISo8601日期字符串</returns>
        public static string DateTimeToIso8601(DateTime? dt, DateTime? defaultdt = null)
        {
            dt = dt ?? defaultdt;
            return !dt.HasValue ? "" : dt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo);
            // return !dt.HasValue ? "" : dt.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff+08:00", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// 方法JS utc时间转换
        /// </summary>
        /// <param name="jsutc"></param>
        /// <returns></returns>

        public static DateTime SetJsUtc(Int64 jsutc)
        {
            DateTime _jsutc1970 = Convert.ToDateTime("1970-01-01 0:0:0");
            // DateTime _localtime = _jsutc1970.AddMilliseconds(jsutc);
            //  return _localtime.ToLocalTime();
            return _jsutc1970.AddMilliseconds(jsutc);
        }

        /// <summary>
        /// 方法JS utc时间转换
        /// </summary>
        /// <param name="standdate"></param>
        /// <returns></returns>
        public static double GetJsUtc(DateTime standdate)
        {
            DateTime _jsutc1970 = Convert.ToDateTime("1970-01-01 0:0:0");
            if (standdate < _jsutc1970)
            {
                standdate = DateTime.Now;
            }

            //TimeSpan _a = standdate - _jsutc1970;
            return standdate.Subtract(_jsutc1970).TotalMilliseconds;
            //_a.TotalMilliseconds;
        }


        private const double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// Gps坐标距离
        /// </summary>
        /// <param name="n1">纬度</param>
        /// <param name="e1">经度</param>
        /// <param name="n2">纬度1</param>
        /// <param name="e2">经度1</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
            Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            if (s < 0.0002)
            {
                return Math.Round(1000 * s, 2);
            }

            s = s * EARTH_RADIUS;
            s = Math.Round(Math.Round(s * 10000) / 10000, 2);
            return s;
        }

        public static T WrapRunTime<T>(string methodName, Func<T> func)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            T result = func();
            watch.Stop();
            Fireasy.Common.Logging.DefaultLogger.Instance.Debug(string.Format("方法：{0},耗时{1}", methodName, watch.Elapsed.TotalMilliseconds));
            return result;
        }


        public static void WrapRunTime(string methodName, Action action)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            Fireasy.Common.Logging.DefaultLogger.Instance.Debug(string.Format("方法：{0},耗时{1}", methodName, watch.Elapsed.TotalMilliseconds));
        }

        public static T WrapRunTime<T>(string methodName, Func<T> func, ref string msg)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            T result = func();
            watch.Stop();
            msg += string.Format("方法：{0},耗时{1}", methodName, watch.Elapsed.TotalMilliseconds);
            return result;
        }


        public static void WrapRunTime(string methodName, Action action, ref string msg)
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            msg += string.Format("方法：{0},耗时{1}", methodName, watch.Elapsed.TotalMilliseconds);
        }


        //获取本机的IP
        public static string GetLocalIP()
        {
            string hostName = Dns.GetHostName();//本机名   
            //System.Net.IPAddress[] addressList = Dns.GetHostByName(hostName).AddressList;//会警告GetHostByName()已过期，我运行时且只返回了一个IPv4的地址   
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6   
            return addressList[0].ToString();
        }

        //获取本机的MAC
        public static string GetLocalMac()
        {
            string mac = null;
            ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection queryCollection = query.Get();
            foreach (ManagementObject mo in queryCollection)
            {
                if (mo["IPEnabled"].ToString() == "True")
                    mac = mo["MacAddress"].ToString().Replace(":", "-");
            }
            return (mac);
        }

        public static int GenerateRandomInt(int min, int max)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int randomInt = random.Next(min, max);
            return randomInt;
        }

        /// <summary>
        /// 生成8位无重复数字
        /// </summary>
        /// <returns></returns>
        public static int GenerateRandomInt()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int randomInt = random.Next(10000000, 99999999);
            return randomInt;
        }

        /// <summary>
        /// 获取变量名称 bool test_name = true;
        /// string tips = 类名.GetVarName(it => test_name);
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>return string</returns>
        public static string GetVarName(System.Linq.Expressions.Expression<Func<object, object>> exp)
        {

            return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;
        }

        /// <summary>
        /// 获取本机公网IP，需要联网
        /// </summary>
        /// <returns></returns>
        public static string GetPublicIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据

                int start = all.IndexOf("您的IP地址是：[") + 9;
                int end = all.IndexOf("]", start);
                tempip = all.Substring(start, end - start);
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }


        public static Configuration GetConfig(string filepath = "")
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "portal.config");
            }
            if (!File.Exists(filepath))
            {
                string xPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config");
                if (File.Exists(xPath))
                {
                    filepath = xPath;
                }
                else
                {
                    return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
            }
            map.ExeConfigFilename = filepath;// 这里对应你app1文件的路径
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            return config;
            //config.AppSettings
            //string connstr = config.ConnectionStrings.ConnectionStrings["connStr"].ConnectionString;
            //string key = config.AppSettings.Settings["key"].Value;
        }

        public static string GetAppConfig(string key, string filepath = "")
        {
            Configuration config = GetConfig(filepath);
            var keyval = config.AppSettings.Settings[key];
            return keyval == null ? "" : keyval.Value; ;
            //config.AppSettings
            //string connstr = config.ConnectionStrings.ConnectionStrings["connStr"].ConnectionString;
            //string key = config.AppSettings.Settings["key"].Value;
        }
        public static Dictionary<string, string> AppConfig
        {
            get
            {
                Configuration config = GetConfig();
                var result = config.AppSettings.Settings;
                var resultdic = new Dictionary<string, string>();
                foreach (KeyValueConfigurationElement setting in result)
                {
                    resultdic.Add(setting.Key, setting.Value);
                }
                return resultdic;
            }
        }

        public static void SetAppConfig(string key,string value,string filepath = "")
        {

            Configuration config = GetConfig(filepath);
            config.AppSettings.Settings[key].Value = value;
            config.Save();
        }
    }
}
