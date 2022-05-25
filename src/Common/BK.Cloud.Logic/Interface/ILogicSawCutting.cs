using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public interface ILogicSawCutting
    {
        /// <summary>
        /// 查询锯切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        List<TRK_SC_RECORD> SelSawCuttingRecord(string BLANK_CARD_ID, string TIME);
    }
}
