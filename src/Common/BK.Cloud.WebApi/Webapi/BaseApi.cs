using BK.Cloud.Facade;
using BK.Cloud.Model.Customer;
using BK.Cloud.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Net.Http.Formatting;
using System.Data;
using ServiceStack.Redis;


namespace BK.Cloud.WebApi.Webapi
{
    public class BaseApi : ApiController
    {
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
                        var res = new PooledRedisClientManager(readhosts, writehosts, new RedisClientManagerConfig() { MaxWritePoolSize = 100, MaxReadPoolSize = 100 });

                        clientsManager = res;
                        return res;
                    }
                }
                return clientsManager;
            }
        }
        protected new JsonResult<T> Json<T>(T obj) where T : class, new()
        {
            //if (obj is Condition)
            //{
            //    obj = SetCondition(obj as Condition) as T;
            //}
            //else
            //{
            //    obj = SetObjectVal(obj);
            //}
            var serialset = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddTHH:mm:ss" };
            serialset.DefaultValueHandling = DefaultValueHandling.Ignore;
            serialset.Formatting = Formatting.None;
            return Json(obj, serialset);
        }

        protected new JsonResult<T> MJson<T>(T obj) where T : class, new()
        {
            //if (obj is Condition)
            //{
            //    obj = SetCondition(obj as Condition) as T;
            //}
            //else
            //{
            //    obj = SetObjectVal(obj);
            //}
            var serialset = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddTHH:mm:ss" };
            serialset.DefaultValueHandling = DefaultValueHandling.Include;
            serialset.Formatting = Formatting.None;
            return Json(obj, serialset);
        }

        /// <summary>
        /// 日期格式化不包含T。完整的数据结构返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected new JsonResult<T> NoTJson<T>(T obj) where T : class, new()
        {
            var serialset = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-dd HH:mm:ss" };
            serialset.DefaultValueHandling = DefaultValueHandling.Include;
            serialset.Formatting = Formatting.None;
            return Json(obj, serialset);
        }

        public WebSession Session
        {
            get
            {
                return new WebSession(HttpContext.Current, false, 60 * 1000);
            }
        }
        public string ArrayObjToStr(object obj)
        {

            if (!(obj is IEnumerable))
            {
                return "[" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "]";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public string QueryObjToStr(object obj)
        {
            //if (obj is IEnumerable)
            //{
            //    throw new Exception("查询集不支持迭代");
            //}
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }


        public string QueryString(string paramter)
        {
            return HttpContext.Current.Request[paramter] ?? "";
        }

        public Condition SetCondition(Condition condition)
        {
            condition = condition ?? new Condition();

            List<string> formnames = HttpContext.Current.Request.Form.AllKeys.ToList();
            List<string> querynames = HttpContext.Current.Request.QueryString.AllKeys.ToList();

            var dic = HttpContext.Current.Request;
            Type t = condition.GetType();

            foreach (string name in querynames)
            {
                var prop = t.GetProperty(name);
                if (prop == null)
                {
                    condition.where[name.ToLower()] = dic.QueryString[name.Trim()];
                }
                else
                {
                    if (dic[name] != null)
                    {
                        prop.SetValue(condition, CommonHelper.ChangeType(prop.PropertyType, dic[name]), null);
                    }
                }
            }

            foreach (string name in formnames)
            {
                var prop = t.GetProperty(name);
                if (prop == null)
                {
                    if (!condition.where.ContainsKey(name.ToLower()))
                    {
                        condition.where[name.ToLower()] = dic.Form[name.Trim()];
                    }
                    //更新时,主键不相等.则分割主键提交后台更新
                    else if (!condition.where[name.ToLower()].Equals(dic.Form[name.Trim()]))
                    {
                        //旧主键+分隔符+新主键
                        condition.where[name.ToLower()] = condition.where[name.ToLower()] + ServiceConst.UpKeySplit + dic.Form[name.Trim()];
                    }
                }
                else
                {
                    if (dic[name] != null)
                    {
                        prop.SetValue(condition, CommonHelper.ChangeType(prop.PropertyType, dic.Form[name]), null);
                    }
                }
            }

            //修正where条件不能正常赋值的webapibug.webapi在处理继承类方面有点问题
            foreach (string key in condition.Keys.ToList())
            {
                condition[key] = condition[key];
            }

            return condition;
        }

        public Condition SetQueryCondition(Condition condition)
        {
            condition = condition ?? new Condition();

            List<string> formnames = HttpContext.Current.Request.Form.AllKeys.ToList();
            List<string> querynames = HttpContext.Current.Request.QueryString.AllKeys.ToList();

            var dic = HttpContext.Current.Request;
            Type t = condition.GetType();

            foreach (string name in querynames)
            {
                var prop = t.GetProperty(name);
                if (prop == null)
                {
                    condition.where[name.ToLower()] = dic.QueryString[name.Trim()];
                }
                else
                {
                    if (dic[name] != null)
                    {
                        prop.SetValue(condition, CommonHelper.ChangeType(prop.PropertyType, dic[name]), null);
                    }
                }
            }

            foreach (string name in formnames)
            {
                var prop = t.GetProperty(name);
                if (prop == null)
                {
                    if (!condition.where.ContainsKey(name.ToLower()))
                    {
                        condition.where[name.ToLower()] = dic.Form[name.Trim()];
                    }
                    //更新时,主键不相等.则分割主键提交后台更新
                    else if (!condition.where[name.ToLower()].Equals(dic.Form[name.Trim()]))
                    {
                        //旧主键+分隔符+新主键
                        condition.where[name.ToLower()] = condition.where[name.ToLower()] + "$" + dic.Form[name.Trim()];
                    }
                }
                else
                {
                    if (dic[name] != null)
                    {
                        prop.SetValue(condition, CommonHelper.ChangeType(prop.PropertyType, dic.Form[name]), null);
                    }
                }
            }

            //修正where条件不能正常赋值的webapibug.webapi在处理继承类方面有点问题
            foreach (string key in condition.Keys.ToList())
            {
                condition[key] = condition[key];
            }

            return condition;
        }
        public Condition ObjToCondition<T>(T obj, string objName) where T : class,new()
        {
            return obj.ConvertoObjToCondition(objName);
        }

        public T SetObjectVal<T>(T obj) where T : class,new()
        {
            if (obj == default(T))
            {
                obj = new T();
            }
            List<string> names = HttpContext.Current.Request.Form.AllKeys.ToList();
            names.AddRange(HttpContext.Current.Request.QueryString.AllKeys);
            names = names.Select(o => o.ToLower()).ToList();
            var dic = HttpContext.Current.Request;
            Type t = obj.GetType();
            var propertys = t.GetProperties();
            foreach (var property in propertys)
            {
                if (dic[property.Name] != null && names.Contains(property.Name.ToLower()))
                {
                    string retval = dic[property.Name];
                    //if (dic[property.Name] == "" && property.PropertyType == typeof(string))
                    //{
                    //    retval = "[null]";
                    //}
                    property.SetValue(obj, CommonHelper.ChangeType(property.PropertyType, retval), null);
                }
            }

            //foreach (string name in names)
            //{
            //    var prop = t.GetProperty(name.Trim());
            //    if (prop != null)
            //    {
            //        if (prop.GetValue(obj) == null && !string.IsNullOrEmpty(dic[name]))
            //        {
            //            prop.SetValue(obj, dic[name], null);
            //        }
            //    }
            //}
            return obj;
        }

        public StringContent RetJSON(object obj)
        {
            //WebUtil.SetWebContext();
            var serialset = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddTHH:mm:ss" };
            serialset.DefaultValueHandling = DefaultValueHandling.Ignore;
            serialset.Formatting = Formatting.None;
            return new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj, serialset));
        }


        public UpMsg UpLoadFile(string fileid)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string s_rpath = FileHelper.GetUploadPath();
            string Datedir = fileid;
            //DateTime.Now.ToString("yy-MM-dd");
            string updir = Path.Combine(s_rpath, Datedir);
            HttpPostedFile httpFile = HttpContext.Current.Request.Files[0];
            //获取要保存的文件信息
            string filerealname = httpFile.FileName;

            //获得文件扩展名
            string fileNameExt = System.IO.Path.GetExtension(filerealname);

            var provider = new MultipartFormDataStreamProvider(updir);
            try
            {
                StringBuilder sb = new StringBuilder();
                var task = Request.Content.ReadAsMultipartAsync(provider);
                task.Wait();
                return new UpMsg("http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/" + System.Configuration.ConfigurationManager.AppSettings["uploaddir"] + "/" + fileid + "." + fileNameExt, true) { data = fileid + "." + fileNameExt };
                //foreach (var file in provider.FileData)
                //{
                //    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                //   // sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                //}

                //return Request.CreateResponse(HttpStatusCode.OK, UpMsg.SuccessMsg);
                //return new HttpResponseMessage()
                //{
                //    //Content = new StringContent(UpMsg.SuccessMsg)
                //    Content = new ObjectContent<UpMsg>(UpMsg.SuccessMsg, new JsonMediaTypeFormatter())
                //};
            }
            catch (System.Exception e)
            {
                return new UpMsg(e.Message, false);
                //Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


    }
}
