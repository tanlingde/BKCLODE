using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public class LogicLongInventory : BusQuery, ILogicLongInventory
    {
        /// <summary>
        /// 查询计划是否存在
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TRK_PRO_PLAN PlanExist(TRK_PRO_PLANMetadata data)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    TRK_PRO_PLAN trk_pro_plan = db.TRK_PRO_PLAN.Where(p => p.BLANK_CARD_ID == data.BLANK_CARD_ID.ToString()
                              && p.BLANK_SPECS == (int)data.BLANK_SPECS && p.BLANK_WEIGHT == (int)data.BLANK_WEIGHT
                              && p.BLANK_LENGTH == (int)data.BLANK_LENGTH && p.COUNT == (int)data.COUNT).First();
                    return trk_pro_plan;
                }
                catch (Exception)
                {
                    return new TRK_PRO_PLAN();
                }
            });
            return res;
        }

        /// <summary>
        /// 新增长坯库待入库记录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<TRK_LW_DETAIL> AddLongBlankStockPending(TRK_PRO_PLANMetadata data)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    List<TRK_LW_DETAIL> list = new List<TRK_LW_DETAIL>();
                    for (int i = 0; i < int.Parse(data.COUNT.ToString()); i++)
                    {
                        TRK_LW_DETAIL trk_lw_detail = new TRK_LW_DETAIL();
                        trk_lw_detail.BLANK_CARD_ID = data.BLANK_CARD_ID.ToString() + (i + 1);
                        trk_lw_detail.BLANK_SPECS = int.Parse(data.BLANK_SPECS.ToString());
                        trk_lw_detail.BLANK_WEIGHT = int.Parse(data.BLANK_WEIGHT.ToString());
                        trk_lw_detail.BLANK_LENGTH = int.Parse(data.BLANK_LENGTH.ToString());
                        string str = data.BLANK_CARD_ID.ToString().Substring(0, 1);
                        //根据卡号第一个字母区分H：轧坯，F：锻坯，其他都是连铸坯
                        switch (str)
                        {
                            case "H":
                                trk_lw_detail.BLANK_TYPE = 2;
                                break;
                            case "F":
                                trk_lw_detail.BLANK_TYPE = 3;
                                break;
                            default:
                                trk_lw_detail.BLANK_TYPE = 1;
                                break;
                        }
                        trk_lw_detail.BLANK_STATE = 0;
                        trk_lw_detail.CREATE_TIME = DateTime.Now;
                        list.Add(trk_lw_detail);
                    }
                    return list;
                }
                catch (Exception)
                {
                    return new List<TRK_LW_DETAIL>();
                }
            });
            return res;
        }

        /// <summary>
        /// 修改长坯库状态为已入库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankState(TRK_LW_DETAILMetadata data)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    TRK_LW_DETAIL trk_lw_detail = db.TRK_LW_DETAIL.Where(p => p.BLANK_CARD_ID.Contains(data.BLANK_CARD_ID.ToString())
                    && p.BLANK_STATE == 0).OrderByDescending(p => p.CREATE_TIME).First();
                    trk_lw_detail.BLANK_STATE = 1;
                    trk_lw_detail.FUR_NO = data.FUR_NO.ToString();
                    trk_lw_detail.PLATE_ID = data.PLATE_ID.ToString();
                    trk_lw_detail.UPDATE_TIME = DateTime.Now;
                    return trk_lw_detail;
                }
                catch (Exception)
                {
                    return new TRK_LW_DETAIL();
                }
            });
            return res;
        }

        /// <summary>
        /// 修改长坯库状态为已出库
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankStateEX(TRK_LW_DETAILMetadata data)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    TRK_LW_DETAIL trk_lw_detail = db.TRK_LW_DETAIL.Where(p => p.BLANK_CARD_ID.Contains(data.BLANK_CARD_ID.ToString())
                    && p.BLANK_STATE == 1).OrderByDescending(p => p.CREATE_TIME).First();
                    trk_lw_detail.BLANK_STATE = 2;
                    //trk_lw_detail.FUR_NO = data.FUR_NO.ToString();
                    //trk_lw_detail.PLATE_ID = data.PLATE_ID.ToString();
                    trk_lw_detail.UPDATE_TIME = DateTime.Now;
                    return trk_lw_detail;
                }
                catch (Exception)
                {
                    return new TRK_LW_DETAIL();
                }
            });
            return res;
        }

        /// <summary>
        /// 修改长坯库状态为已删除(吊走下线)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TRK_LW_DETAIL UpdLongBlankStateDel(TRK_LW_DETAILMetadata data)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    TRK_LW_DETAIL trk_lw_detail = db.TRK_LW_DETAIL.Where(p => p.BLANK_CARD_ID == data.BLANK_CARD_ID.ToString()).First();
                    trk_lw_detail.BLANK_STATE = 3;
                    //trk_lw_detail.FUR_NO = data.FUR_NO.ToString();
                    //trk_lw_detail.PLATE_ID = data.PLATE_ID.ToString();
                    trk_lw_detail.UPDATE_TIME = DateTime.Now;
                    return trk_lw_detail;
                }
                catch (Exception)
                {
                    return new TRK_LW_DETAIL();
                }
            });
            return res;
        }

        /// <summary>
        /// 根据卡号修改计划的完成支数/未完成支数
        /// </summary>
        /// <param name="BLANK_CARD_ID">卡号</param>
        /// <param name="TYPE">类型</param>
        /// <returns></returns>
        public TRK_PRO_PLAN UpdProPlan(string BLANK_CARD_ID, int TYPE)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    TRK_PRO_PLAN trk_pro_plan = db.TRK_PRO_PLAN.Where(p => p.BLANK_CARD_ID == BLANK_CARD_ID).First();
                    //根据TYPE区分1：已出库/已入库，2：吊走下线 修改已完成支数和未完成支数
                    switch (TYPE)
                    {
                        case 1:
                            trk_pro_plan.ACCOMPLISH_COUNT = trk_pro_plan.ACCOMPLISH_COUNT + 1;
                            break;
                        case 2:
                            trk_pro_plan.UNFINISHED_COUNT = trk_pro_plan.UNFINISHED_COUNT + 1;
                            break;
                        default:
                            break;
                    }
                    return trk_pro_plan;
                }
                catch (Exception)
                {
                    return new TRK_PRO_PLAN();
                }
            });
            return res;
        }

        /// <summary>
        /// 根据卡号、时间、状态查询长坯库记录
        /// </summary>
        /// <param name="BLANK_CARD_ID"></param>
        /// <param name="TIME"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<TRK_LW_DETAIL> SelLongBankRecord(string BLANK_CARD_ID, string TIME, int state)
        {
            var res = RunDb((db) =>
            {
                try
                {
                    List<TRK_LW_DETAIL> list = new List<TRK_LW_DETAIL>();
                    if (BLANK_CARD_ID != "" && TIME == "")
                    {
                        list = db.TRK_LW_DETAIL.Where(p => p.BLANK_CARD_ID.Contains(BLANK_CARD_ID) && p.BLANK_STATE == state).ToList();
                    }
                    else if (BLANK_CARD_ID == "" && TIME != "")
                    {
                        list = db.TRK_LW_DETAIL.Where(p => p.CREATE_TIME == DateTime.Parse(TIME) && p.BLANK_STATE == state).ToList();
                    }
                    else if (BLANK_CARD_ID != "" && TIME != "")
                    {
                        list = db.TRK_LW_DETAIL.Where(p => p.BLANK_CARD_ID.Contains(BLANK_CARD_ID) && p.CREATE_TIME == DateTime.Parse(TIME) && p.BLANK_STATE == state).ToList();
                    }
                    else
                    {
                        list = db.TRK_LW_DETAIL.Where(p => p.BLANK_STATE == state).ToList();
                    }
                        return list;
                }
                catch (Exception)
                {
                    return new List<TRK_LW_DETAIL>();
                }
            });
            return res;
        }
    }
}
