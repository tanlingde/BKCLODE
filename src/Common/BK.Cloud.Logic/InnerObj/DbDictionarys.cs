using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Fireasy.Common.Logging;
using BK.Cloud.Model.Data.Model;

namespace BK.Cloud.Logic.InnerObj
{
    public class CacheDbDictionarys
    {
        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        private static readonly ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary> dbDictionarys =
            new ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary>();
        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        private static ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary> _dbDictionarys = null;

        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        public static ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary> DbDictionarys
        {
            get
            {
                return Init();
            }
        }

        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary> dbCarDictionarys =
            new ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary>();
        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        private static ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary> _CarDictionarys = null;

        /// <summary>
        /// 缓存当前字典集合
        /// </summary>
        public static ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary> CarDictionarys
        {
            get
            {
                return CarDictionaryInit();
            }
        }


        public static ConcurrentDictionary<long, TB_Dictionary> Init()
        {
            try
            {
                //有缓存,则适用缓存
                if (HttpContext.Current.Cache.Get("DbDictionarys") == null)
                {

                    if (dbDictionarys.Count == 0)
                    {
                        lock (dbDictionarys)
                        {
                            if (dbDictionarys.Count == 0)
                            {
                                using (var dbcontenxt = new BK.Cloud.Model.Data.Model.DbContext())
                                {
                                    var dList = dbcontenxt.TB_Dictionaries.ToList();
                                    foreach (var data in dList)
                                    {
                                        dbDictionarys.TryAdd(data.DictionaryId, data);
                                    }
                                }
                            }
                        }
                    }
                    return dbDictionarys;
                }

                if (_dbDictionarys == null)
                {
                    if (HttpContext.Current.Cache.Get("DbDictionarys") == null)
                    {
                        _dbDictionarys = new ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary>();
                        HttpContext.Current.Cache.Insert("DbDictionarys", _dbDictionarys);
                    }
                    else
                    {
                        _dbDictionarys = HttpContext.Current.Cache["DbDictionarys"] as
                            ConcurrentDictionary<long, BK.Cloud.Model.Data.Model.TB_Dictionary>;
                    }
                    if (_dbDictionarys != null)
                    {
                        using (var dbcontenxt = new BK.Cloud.Model.Data.Model.DbContext())
                        {
                            var dList = dbcontenxt.TB_Dictionaries.ToList();
                            foreach (var data in dList)
                            {
                                _dbDictionarys.TryAdd(data.DictionaryId, data);
                            }
                        }
                    }
                    else
                    {
                        DefaultLogger.Instance.Fatal("初始化字典异常,无值");
                    }
                }
                return _dbDictionarys;
            }
            catch (Exception ex) {
                DefaultLogger.Instance.Fatal("初始化字典异常,无值",ex);
                return null;
            }
        }

        public static ConcurrentDictionary<string, TB_CarDictionary> CarDictionaryInit()
        {
            try
            {
                //有缓存,则适用缓存
                if (HttpContext.Current.Cache.Get("CarDictionarys") == null)
                {

                    if (dbCarDictionarys.Count == 0)
                    {
                        lock (dbCarDictionarys)
                        {
                            if (dbCarDictionarys.Count == 0)
                            {
                                using (var dbcontenxt = new BK.Cloud.Model.Data.Model.DbContext())
                                {
                                    var dList = dbcontenxt.TB_CarDictionary.ToList();
                                    foreach (var data in dList)
                                    {
                                        dbCarDictionarys.TryAdd(data.SeriesName, data);
                                    }
                                }
                            }
                        }
                    }
                    return dbCarDictionarys;
                }

                if (_CarDictionarys == null)
                {
                    if (HttpContext.Current.Cache.Get("CarDictionarys") == null)
                    {
                        _CarDictionarys = new ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary>();
                        HttpContext.Current.Cache.Insert("CarDictionarys", _dbDictionarys);
                    }
                    else
                    {
                        _CarDictionarys = HttpContext.Current.Cache["CarDictionarys"] as
                            ConcurrentDictionary<string, BK.Cloud.Model.Data.Model.TB_CarDictionary>;
                    }
                    if (_CarDictionarys != null)
                    {
                        using (var dbcontenxt = new BK.Cloud.Model.Data.Model.DbContext())
                        {
                            var dList = dbcontenxt.TB_CarDictionary.ToList();
                            foreach (var data in dList)
                            {
                                _CarDictionarys.TryAdd(data.SeriesName, data);
                            }
                        }
                    }
                    else
                    {
                        DefaultLogger.Instance.Fatal("初始化字典异常,无值");
                    }
                }
                return _CarDictionarys;
            }
            catch (Exception ex)
            {
                DefaultLogger.Instance.Fatal("初始化字典异常,无值", ex);
                return null;
            }
        }
    }
}
