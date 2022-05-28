using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Facade.Interface
{
    public interface IFacadeRedisManage
    {
        /// <summary>
        /// 获取redis集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<KeyValuePair<string, TRK_LW_DETAIL>> GetRedisLW(string key);
    }
}
