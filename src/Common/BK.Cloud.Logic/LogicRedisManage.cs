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
        public bool AddRedisByKey(string HasKey, string IDno)
        {
            RunDb((DbContext) =>
            {
                if (!string.IsNullOrEmpty(IDno))
                {
                    int dataStuat = 0; //默认是长库存
                  //查询短库存需要开始跟踪的最新一条物料记录
                   var sw = DbContext.TRK_SW_DETAIL.Where(o => o.BLANK_CARD_ID.StartsWith(IDno) && o.BLANK_STATE == 3).OrderBy(o => o.BLANK_CARD_ID).FirstOrDefault();
           
                    
                 //长库存修改出入库状态和跟踪状态
                    var lw = DbContext.TRK_LW_DETAIL.Where(o => o.BLANK_CARD_ID.StartsWith(IDno) && o.BLANK_STATE == 3).OrderBy(o => o.BLANK_CARD_ID).FirstOrDefault();
          

                    //return true;
                    switch (HasKey)
                    {
                        case ServiceConst.LIFTING_DATA:
                            break;
                        case ServiceConst.FURNAROLLER_DATA1:
                            break;
                        case ServiceConst.FURNAROLLER_DATA2:
                    
                            break;
                        case ServiceConst.FURNAROLLER_DATA3:
                            break;
                        case ServiceConst.FURNAROLLER_DATA4:
                            break;
                        case ServiceConst.PREHEAT_DATA:
                            break;
                        case ServiceConst.FURNACE_DATA1:
                            break;
                        case ServiceConst.FURNACE_DATA2:
                            break;
                        case ServiceConst.FURNACE_DATA3:
                            break;
                        case ServiceConst.FURNACE_DATA4:
                            break;
                        case ServiceConst.FURNACE_DATA5:
                            break;
                        case ServiceConst.SOAKING_DATA1:
                            break;
                        case ServiceConst.SOAKING_DATA2:
                            break;
                        case ServiceConst.BEHINDFURNACE_DATA:
                            break;
                        case ServiceConst.TRANSPORTATIONCHAIN_DATA:
                            break;
                        case ServiceConst.PIERCING_DATA:
                            break;
                        case ServiceConst.PIPETRACK_DATA:
                            break;
                        case ServiceConst.PIPE_DATA:
                            break;
                        case ServiceConst.REDUCINGROLLER_DATA:
                            break;
                        case ServiceConst.REDUCING_DATA:
                            break;
                        case ServiceConst.COOLINGROLLER_DATA:
                            break;
                        case ServiceConst.COOLING_DATA1:
                            break;
                        case ServiceConst.COOLING_DATA2:
                            break;
                        case ServiceConst.COOLING_DATA3:
                            break;
                        case ServiceConst.STRAIGHTENINGROLLER_DATA:
                            break;
                        case ServiceConst.STRAIGHTENING_DATA:
                            break;
                    }
                }

                using (var client = ClientsManager.GetClient())
                {
                    if (client.Hashes[HasKey].ContainsKey(IDno.ToLower()))
                    {
                        client.Hashes[HasKey].Add(IDno, tRK_LW);
                    }
                }
            });
        }
    }
}
