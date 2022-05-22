using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public class LogicShortInventory : BusQuery, ILogicShortInventory
    {
        /// <summary>
        /// 查询坯料是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool BlankExist(TRK_SW_SUMMARYMetadata data)
        {
            return RunDb((db) =>
            {
                return db.TRK_SW_SUMMARY.Any(s => s.BLANK_CARD_ID == data.BLANK_CARD_ID.ToString()
                              && s.FUR_NO == data.FUR_NO.ToString() && s.PLATE_ID == data.PLATE_ID.ToString());
            });
        }
        /// <summary>
        /// 短坯库库存加1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddShortBlankStock(TRK_SW_SUMMARYMetadata data)
        {
            var res = RunDb((db) => {
                try
                {
                    string sql = string.Format(@"Update TRK_SW_SUMMARY set BLANK_WEIGHT=BLANK_WEIGHT+'{0}',BLANK_LONG_STOCK=BLANK_LONG_STOCK+1 
                            where BLANK_CARD_ID='{1}' and FUR_NO='{2}' and PLATE_ID='{3}'", data.BLANK_WEIGHT, data.BLANK_CARD_ID, data.FUR_NO, data.PLATE_ID);
                    db.Database.BeginTransaction();
                    db.Database.ExecuteNonQuery((Fireasy.Data.SqlCommand)sql);
                    db.Database.CommitTransaction();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            });
            return res;
        }
        /// <summary>
        /// 短坯库库存减1
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SubtractShortBlankStock(TRK_SW_EXWAREHOUSEMetadata data)
        {
            var res = RunDb((db) => {
                try
                {
                    string sql = string.Format(@"Update TRK_SW_SUMMARY set BLANK_WEIGHT=BLANK_WEIGHT-'{0}',BLANK_LONG_STOCK=BLANK_LONG_STOCK-1 
                            where BLANK_CARD_ID='{1}' and FUR_NO='{2}' and PLATE_ID='{3}'", data.BLANK_WEIGHT, data.BLANK_CARD_ID, data.FUR_NO, data.PLATE_ID);
                    db.Database.BeginTransaction();
                    db.Database.ExecuteNonQuery((Fireasy.Data.SqlCommand)sql);
                    db.Database.CommitTransaction();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            });
            return res;
        }

        /// <summary>
        /// 查询短坯库入库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">入库时间</param>
        /// <returns></returns>
        public DataTable SelShortBlankDetailRecord(string PLATE_ID,string TIME)
        {
            var res = RunDb((db) => {
                try
                {
                    //if (PLATE_ID == "" || PLATE_ID == null)
                    //{
                    //    return db.TRK_LW_DETAIL.ToList();
                    //}
                    //else
                    //{
                    //    return db.TRK_LW_DETAIL.Select(a => a.PLATE_ID == PLATE_ID).ToList();
                    //}
                    string sql = "select * from TRK_SW_DETAIL where 1=1 ";
                    if (PLATE_ID != "")
                    {
                        sql += " and PLATE_ID=" + PLATE_ID + " ";
                    }
                    if (TIME != "")
                    {
                        sql += " and TIME=" + DateTime.Parse(TIME) + "";
                    }
                    db.Database.BeginTransaction();
                    DataTable dt = db.Database.ExecuteDataTable((Fireasy.Data.SqlCommand)sql);
                    db.Database.CommitTransaction();
                    return dt;
                }
                catch (Exception)
                {
                    return new DataTable();
                }

            });
            return res;
        }

        /// <summary>
        /// 查询短坯库出库记录
        /// </summary>
        /// <param name="PLATE_ID">牌号/钢号</param>
        /// <param name="TIME">出库时间</param>
        /// <returns></returns>
        public DataTable SelShortBlankExRecord(string PLATE_ID,string TIME)
        {
            var res = RunDb((db) => {
                try
                {
                    //if (PLATE_ID == "" || PLATE_ID == null)
                    //{
                    //    return db.TRK_SW_EXWAREHOUSE.ToList();
                    //}
                    //else
                    //{
                    //    return db.TRK_SW_EXWAREHOUSE.Select(a => a.PLATE_ID == PLATE_ID).ToList();
                    //}
                    string sql = "select * from TRK_SW_EXWAREHOUSE where 1=1 ";
                    if (PLATE_ID != "")
                    {
                        sql += " and PLATE_ID=" + PLATE_ID + " ";
                    }
                    if (TIME != "")
                    {
                        sql += " and TIME=" + DateTime.Parse(TIME) + "";
                    }
                    db.Database.BeginTransaction();
                    DataTable dt = db.Database.ExecuteDataTable((Fireasy.Data.SqlCommand)sql);
                    db.Database.CommitTransaction();
                    return dt;
                }
                catch (Exception)
                {
                    return new DataTable();
                }

            });
            return res;
        }
    }
}
