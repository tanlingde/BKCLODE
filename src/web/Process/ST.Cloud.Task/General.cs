using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Task
{
    class General
    {
        /// <summary>
        /// 通过指定url返回json结果集
        /// </summary>
        /// <param name="strURL">url</param>
        /// <returns>json结果集</returns>
        public static Newtonsoft.Json.Linq.JArray GetJsonByUrl(string strURL)
        {
            System.Net.HttpWebRequest request;
            // 创建一个HTTP请求
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;

            //request.Method="get";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();

            Newtonsoft.Json.Linq.JArray ja = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);
            return ja;
        }
    }
}
