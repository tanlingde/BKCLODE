using BK.Cloud.Logic.Interface;
using BK.Cloud.Model.Data.Model;
using BK.Cloud.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic
{
    public class LogicRedisManage : BusQuery, IRedisManage
    {
        /// <summary>
        /// 获取事件区域对应KEY集合
        /// </summary>
        /// <param name="HasKey">事件区域ID</param>
        /// <returns></returns>
        public List<KeyValuePair<string, TRK_LW_DETAIL>> GetRedisLW(string HasKey)
        {
            using (var client = ClientsManager.GetClient())
            {
                var data = client.As<TRK_LW_DETAIL>().GetHash<string>(HasKey).ToConcurrentDictionary().OrderBy(o => o.Key).ToList();
                return data;
            }
        }
        /// <summary>
        /// 添加队列信息
        /// </summary>
        /// <param name="HasKey">区域ID</param>
        /// <param name="IDno">端部识别的物料ID，带J的是短库存</param>
        /// <returns></returns>
        public void AddRedisByKey(string HasKey, string IDno)
        {
            RunDb((DbContext) =>
            {
                if (!string.IsNullOrEmpty(IDno))
                {
                    int dataStuat = 0; //默认是长库存
                    TRK_SW_DETAIL sw = new TRK_SW_DETAIL();
                    TRK_LW_DETAIL lw = new TRK_LW_DETAIL();
                    string nowQue = string.Empty; //当前队列区域
                    string nextQue = string.Empty;//下一队列区域
                    if (IDno.Contains("J"))//短库存
                    {
                        dataStuat = 1;
                        sw = DbContext.TRK_SW_DETAIL.Where(o => o.BLANK_CARD_ID.StartsWith(IDno) && o.BLANK_STATE != 3&& o.BLANK_STATE !=0).OrderBy(o => o.BLANK_CARD_ID).FirstOrDefault();
                    }
                    else {  //长库存
                        lw = DbContext.TRK_LW_DETAIL.Where(o => o.BLANK_CARD_ID.StartsWith(IDno) && o.BLANK_STATE != 3 && o.BLANK_STATE != 0).OrderBy(o => o.BLANK_CARD_ID).FirstOrDefault();
                    }
                    //return true;
                    switch (HasKey)
                    {
                        case ServiceConst.LIFTING_DATA:
                            nowQue = ServiceConst.LIFTING_DATA;
                            nextQue = ServiceConst.FURNAROLLER_DATA1;
                            break;
                        case ServiceConst.FURNAROLLER_DATA1:
                            nowQue = ServiceConst.FURNAROLLER_DATA1;
                            nextQue = ServiceConst.FURNAROLLER_DATA2;
                            break;
                        case ServiceConst.FURNAROLLER_DATA2:
                            nowQue = ServiceConst.FURNAROLLER_DATA2;
                            nextQue = ServiceConst.FURNAROLLER_DATA3;
                            if (dataStuat == 0) //识别通过后改变库存状态
                            {
                                lw.BLANK_STATE = 2;
                                DbContext.TRK_LW_DETAIL.Update(lw);
                            }
                            else {
                                sw.BLANK_STATE = 2;
                                DbContext.TRK_SW_DETAIL.Update(sw);
                            }
                            break;
                        case ServiceConst.FURNAROLLER_DATA3:
                            nowQue = ServiceConst.FURNAROLLER_DATA3;
                            nextQue = ServiceConst.FURNAROLLER_DATA4;
                            break;
                        case ServiceConst.FURNAROLLER_DATA4:
                            nowQue = ServiceConst.FURNAROLLER_DATA4;
                            nextQue = ServiceConst.PREHEAT_DATA;
                            break;
                        case ServiceConst.PREHEAT_DATA:
                            nowQue = ServiceConst.PREHEAT_DATA;
                            nextQue = ServiceConst.FURNACE_DATA1;
                            break;
                        case ServiceConst.FURNACE_DATA1:
                            nowQue = ServiceConst.FURNACE_DATA1;
                            nextQue = ServiceConst.FURNACE_DATA2;
                            break;
                        case ServiceConst.FURNACE_DATA2:
                            nowQue = ServiceConst.FURNACE_DATA2;
                            nextQue = ServiceConst.FURNACE_DATA3;
                            break;
                        case ServiceConst.FURNACE_DATA3:
                            nowQue = ServiceConst.FURNACE_DATA3;
                            nextQue = ServiceConst.FURNACE_DATA4;
                            break;
                        case ServiceConst.FURNACE_DATA4:
                            nowQue = ServiceConst.FURNACE_DATA4;
                            nextQue = ServiceConst.FURNACE_DATA5;
                            break;
                        case ServiceConst.FURNACE_DATA5:
                            nowQue = ServiceConst.FURNACE_DATA5;
                            nextQue = ServiceConst.SOAKING_DATA1;
                            break;
                        case ServiceConst.SOAKING_DATA1:
                            nowQue = ServiceConst.SOAKING_DATA1;
                            nextQue = ServiceConst.SOAKING_DATA2;
                            break;
                        case ServiceConst.SOAKING_DATA2:
                            nowQue = ServiceConst.SOAKING_DATA2;
                            nextQue = ServiceConst.BEHINDFURNACE_DATA;
                            break;
                        case ServiceConst.BEHINDFURNACE_DATA:
                            nowQue = ServiceConst.BEHINDFURNACE_DATA;
                            nextQue = ServiceConst.TRANSPORTATIONCHAIN_DATA;
                            break;
                        case ServiceConst.TRANSPORTATIONCHAIN_DATA:
                            nowQue = ServiceConst.TRANSPORTATIONCHAIN_DATA;
                            nextQue = ServiceConst.PIERCING_DATA;
                            break;
                        case ServiceConst.PIERCING_DATA:
                            nowQue = ServiceConst.PIERCING_DATA;
                            nextQue = ServiceConst.PIPETRACK_DATA;
                            break;
                        case ServiceConst.PIPETRACK_DATA:
                            nowQue = ServiceConst.PIPETRACK_DATA;
                            nextQue = ServiceConst.PIPE_DATA;
                            break;
                        case ServiceConst.PIPE_DATA:
                            nowQue = ServiceConst.PIPE_DATA;
                            nextQue = ServiceConst.REDUCINGROLLER_DATA;
                            break;
                        case ServiceConst.REDUCINGROLLER_DATA:
                            nowQue = ServiceConst.REDUCINGROLLER_DATA;
                            nextQue = ServiceConst.REDUCING_DATA;
                            break;
                        case ServiceConst.REDUCING_DATA:
                            nowQue = ServiceConst.REDUCING_DATA;
                            nextQue = ServiceConst.COOLINGROLLER_DATA;
                            break;
                        case ServiceConst.COOLINGROLLER_DATA:
                            nowQue = ServiceConst.COOLINGROLLER_DATA;
                            nextQue = ServiceConst.COOLING_DATA1;
                            break;
                        case ServiceConst.COOLING_DATA1:
                            nowQue = ServiceConst.COOLING_DATA1;
                            nextQue = ServiceConst.COOLING_DATA2;
                            break;
                        case ServiceConst.COOLING_DATA2:
                            nowQue = ServiceConst.COOLING_DATA2;
                            nextQue = ServiceConst.COOLING_DATA3;
                            break;
                        case ServiceConst.COOLING_DATA3:
                            nowQue = ServiceConst.COOLING_DATA3;
                            nextQue = ServiceConst.STRAIGHTENINGROLLER_DATA;
                            break;
                        case ServiceConst.STRAIGHTENINGROLLER_DATA:
                            nowQue = ServiceConst.STRAIGHTENINGROLLER_DATA;
                            nextQue = ServiceConst.STRAIGHTENING_DATA;
                            break;
                        case ServiceConst.STRAIGHTENING_DATA:
                            //结束跟踪
                            nowQue = ServiceConst.STRAIGHTENING_DATA;

                            break;
                    }
                    using (var client = ClientsManager.GetClient())
                    {
                        if (dataStuat == 0)
                        {
                            if (!client.Hashes[nowQue].ContainsKey(sw.BLANK_CARD_ID))
                            {
                                client.Hashes[nowQue].Add(sw.BLANK_CARD_ID, ArrayObjToStr(sw));//添加第一条队列
                            }
                            else
                            {
                                client.Hashes[nowQue].Remove(sw.BLANK_CARD_ID);//删除当前队列KEY
                                if (!client.Hashes[nextQue].ContainsKey(sw.BLANK_CARD_ID))
                                {
                                    //添加下一队列消息
                                    client.Hashes[nextQue].Add(sw.BLANK_CARD_ID, ArrayObjToStr(sw));
                                }
                            }
                        }
                        else {
                            if (!client.Hashes[nowQue].ContainsKey(lw.BLANK_CARD_ID))
                            {
                                client.Hashes[nowQue].Add(lw.BLANK_CARD_ID, ArrayObjToStr(lw));//添加第一条队列
                            }
                            else
                            {
                                client.Hashes[nowQue].Remove(lw.BLANK_CARD_ID);//删除当前队列KEY
                                if (!client.Hashes[nextQue].ContainsKey(lw.BLANK_CARD_ID))
                                {
                                    //添加下一队列消息
                                    client.Hashes[nextQue].Add(lw.BLANK_CARD_ID, ArrayObjToStr(lw));
                                }
                            }
                        }
                    
                    }
                }
        
            });
        }

        /// <summary>
        /// 下线物料
        /// </summary>
        /// <param name="HasKey">队列区域ID</param>
        /// <param name="ID">物料ID</param>
        ///<param name="HsaName">队列区域名称</param>
        public void DelteReids(string HasKey, string ID,string HsaName)
        {
            using (var client = ClientsManager.GetClient())
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    if (client.Hashes[HasKey].ContainsKey(ID))
                    {
                        LogicProvide.LogicDebug.LogCommondMessage(ID, "下线", HsaName + "下线物料ID：" + ID);
                        client.Hashes[HasKey].Remove(ID);//删除当前队列KEY
                        
                    }
                    
                }
            }
        }
    }
}
