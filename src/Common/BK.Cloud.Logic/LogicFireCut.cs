using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public class LogicFireCut : BusQuery, ILogicFireCut
    {
        /// <summary>
        /// 查询火切记录
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TIME">火切时间</param>
        /// <returns></returns>
        public List<TRK_FC_RECORD> SelFireCutRecord(string BLANK_CARD_ID, string TIME)
        {
            var res = RunDb((db) => {
                try
                {
                    List<TRK_FC_RECORD> list = new List<TRK_FC_RECORD>();
                    if (BLANK_CARD_ID != "" && TIME == "")
                    {
                        list = db.TRK_FC_RECORD.Where(p => p.BLANK_CARD_ID.Contains(BLANK_CARD_ID)).ToList();
                    }
                    else if (BLANK_CARD_ID == "" && TIME != "")
                    {
                        list = db.TRK_FC_RECORD.Where(p => p.CREATE_TIME == DateTime.Parse(TIME)).ToList();
                    }
                    else if (BLANK_CARD_ID != "" && TIME != "")
                    {
                        list = db.TRK_FC_RECORD.Where(p => p.BLANK_CARD_ID.Contains(BLANK_CARD_ID) && p.CREATE_TIME == DateTime.Parse(TIME)).ToList();
                    }
                    else
                    {
                        list = db.TRK_FC_RECORD.ToList();
                    }
                    return list;
                }
                catch (Exception)
                {
                    return new List<TRK_FC_RECORD>();
                }

            });
            return res;
        }
    }
}
