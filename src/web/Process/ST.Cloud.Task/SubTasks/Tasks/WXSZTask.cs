//using Fireasy.Common.Logging;
//using BK.Cloud.Logic.WXSZ;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BK.Cloud.Logic.MKKJ;

//namespace BK.Cloud.Task.SubTasks.Tasks
//{

//    /// <summary>
//    /// 五星遂装任务
//    /// </summary>
//    public class WXSZTask : BaseTask
//    {
//        public override void Init(Dictionary<string, Action<MethodApp>> Methods)
//        {
//            //计算设备运行时长及日运行时长
//            Methods["WXSZ_DevOnLineCount"] = (app) =>
//            {
//                string OrgCode = app.Params["OrgCode"];
//                string CarType = app.Params["cartype"];
//                MKKJLogicProvide.LogicMKTask.SetDayRunTimeAndDetail(1, OrgCode, CarType);
//            };

//            //定时判断在线车辆是否需要锁车解锁
//            Methods["WXSZ_OnLineDevLockOrUnLock"] = (app) =>
//           {
//               string orgid = app.Params["orgid"];
//               //string CarType = app.Params["cartype"];


//               WXSZLogicProvide.LogicTaskData.ExeJob(orgid);
//           };

//            //定时自动给五星隧装1设备重命名
//            Methods["WXSZ_ModifyNameByDeviceNo"] = (app) =>
//            {
//                string OrgCode = app.Params["OrgCode"];
//                string CarType = app.Params["cartype"];
//                string orgid = app.Params["orgid"];
//                WXSZLogicProvide.LogicTaskData.UpdateDeviceNumByNo(OrgCode, orgid, CarType);
//            };

//            //定时自动给五星隧装统计历史表数据
//            Methods["WXSZ_WorkCount"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                WXSZLogicProvide.LogicTaskData.HistoryCount(orgid);
//            };


//            //定时自动给五星隧装统计历史表数据
//            Methods["WXSZ_historycountTotol"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                WXSZLogicProvide.LogicTaskData.HistoryCountToloe(orgid);
//            };

//            //五新隧装转发数据
//            Methods["WXSZ_realOrgData"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                string ip = app.Params["ip"];
//                string port = app.Params["port"];
//                WXSZLogicProvide.LogicTaskData.WXSZ_realOrgData(orgid, ip,port);
//            };

//            //五新隧装根据CAN帧自动命名和第一次自动下发验证码
//            Methods["WXSZ_UpdateCarNmae"] = (app) =>
//            {
//                string OrgCode = app.Params["OrgCode"];
//                string CarType = app.Params["cartype"];
//                WXSZLogicProvide.LogicTaskData.WXSZ_UpdateCarNmae(OrgCode, CarType);
//            };
//            //初始化解锁机数据
//            Methods["Init_CarLock"] = (app) =>
//            {
//                string OrgCode = app.Params["OrgCode"];
//                string CarType = app.Params["cartype"];
//                WXSZLogicProvide.LogicTaskData.Init_CarLock(OrgCode, CarType);
//            };

//            //同步设备所在城市到设备表
//            Methods["WXSZ_DeviceRealAddress"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始解析Gps所在城市");
//                WXSZLogicProvide.LogicTaskData.SaveGBDeviceRealAddress(orgid);
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "结束解析Gps所在城市");
//            };
//            //远程redis同步数据库
//            Methods["WXSZ_saveRedisData"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始远程redis同步数据库");
//                WXSZLogicProvide.LogicTaskData.saveRedisData(orgid);
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "结束远程redis同步数据库");
//            };
//            //湿喷机国标泵送时间统计
//            Methods["WXSZ_saveBStime"] = (app) =>
//            {
//                //DateTime datetime = DateTime.Parse("2021-01-01 14:55:00");

//                //TimeSpan nowDt = DateTime.Now.TimeOfDay;

//                //TimeSpan workStartDT = datetime.AddMinutes(-2).TimeOfDay;
//                //TimeSpan workEndDT = datetime.AddMinutes(2).TimeOfDay;
//                //if (nowDt > workStartDT && nowDt < workEndDT)
//                //{
//                //}

//                string orgid = app.Params["orgid"];
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始湿喷机国标泵送时间统计");
//                WXSZLogicProvide.LogicTaskData.saveBStime(orgid);
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "结束湿喷机国标泵送时间统计");
//            };
//            //湿喷机维修维保报表
//            Methods["WXSZ_saveDj"] = (app) =>
//            {
//                string orgid = app.Params["orgid"];
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "开始湿喷机维修维保报表统计");
//                WXSZLogicProvide.LogicTaskData.WXSZ_saveDj(orgid);
//                DefaultLogger.Instance.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "结束湿喷机维修维保报表统计");
//            };
           
//        }
//    }

//}
