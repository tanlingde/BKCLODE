using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Facade.Interface
{
    public interface IFacadeSawCutting
    {
        /// <summary>
        /// 查询锯切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">锯切时间</param>
        /// <returns></returns>
        List<TRK_SC_RECORD> SelSawCuttingRecord(string BLANK_CARD_ID, string TIME);
    }
}
