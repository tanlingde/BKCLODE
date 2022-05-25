using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Facade.Interface
{
    public interface IFacadeFireCut
    {
        /// <summary>
        /// 查询火切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">火切时间</param>
        /// <returns></returns>
        List<TRK_FC_RECORD> SelFireCutRecord(string BLANK_CARD_ID, string TIME);
    }
}
