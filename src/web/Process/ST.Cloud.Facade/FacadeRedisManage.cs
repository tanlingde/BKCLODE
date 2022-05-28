using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;

namespace BK.Cloud.Facade
{
    public class FacadeRedisManage: BaseWeb, IFacadeRedisManage
    {
        /// <summary>
        /// 获取redis集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, TRK_LW_DETAIL>> GetRedisLW(string key)
        {
            return LogicProvide.RedisManage.GetRedisLW(key);
        }
    }
}
