using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public interface ILogicFireCut
    {
        /// <summary>
        /// 查询火切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID"></param>
        /// <param name="TIME"></param>
        /// <returns></returns>
        List<TRK_FC_RECORD> SelFireCutRecord(string BLANK_CARD_ID, string TIME);
    }
}
