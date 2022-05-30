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
        /// <summary>
        /// 添加队列信息
        /// </summary>
        /// <param name="HasKey">区域ID</param>
        /// <param name="IDno">端部识别的物料ID，带J的是短库存</param>
        /// <returns></returns>
        void AddRedisByKey(string HasKey, string IDno);
        /// <summary>
        /// 下线物料
        /// </summary>
        /// <param name="HasKey">队列区域ID</param>
        /// <param name="ID">物料ID</param>
        ///<param name="HsaName">队列区域名称</param>
        void DelteReids(string HasKey, string ID, string HsaName);
    }
}
