using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic.Interface
{
    public interface IRedisManage
    {
        /// <summary>
        /// 获取对应KEY集合
        /// </summary>
        /// <param name="HasKey"></param>
        /// <returns></returns>
        List<KeyValuePair<string, TRK_LW_DETAIL>> GetRedisLW(string HasKey);
    }
}
