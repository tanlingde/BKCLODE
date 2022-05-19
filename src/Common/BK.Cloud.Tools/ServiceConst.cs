using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Tools
{
    public class ServiceConst
    {
        /// <summary>
        /// redis订阅命令接收频道（服务之间命令）
        /// </summary>
        public const string REDIS_CMD = "REDIS_CMD";

        /// <summary>
        /// redis历史命令接收频道（服务之间命令）
        /// </summary>
        public const string REDIS_CMD_HIS = "REDIS_CMD_HIS";
        /// <summary>
        /// 写变量
        /// </summary>
        public const string REDIS_CMD_WRITEVAR = "REDIS_CMD_WRITEVAR";

        /// <summary>
        /// redis服务状态监控日志信息
        /// </summary>
        public const string REDIS_LOG_MESSAGE = "status_log_message";

        /// <summary>
        /// redis服务错误监控日志信息
        /// </summary>
        public const string REDIS_ERROR_MESSAGE = "status_error_message";

        /// <summary>
        /// redis服务调试日志信息
        /// </summary>
        public const string REDIS_DEBUG_MESSAGE = "status_debug_message";

        /// <summary>
        /// redis历史命令记录
        /// </summary>
        public const string REDIS_COMMOND_HISTORY = "status_commond_history";


        /// <summary>
        /// redis回复命令前缀
        /// </summary>
        public const string CMDREPLY_PREFIX = "CMDREPLY_";
        /// <summary>
        /// redis 深拓协议数据发送接收频道
        /// </summary>
        public const string REDIS_ST_PROTOCOL = "STPROTOCOL";

        /// <summary>
        /// redis 部标协议数据发送接收频道
        /// </summary>
        public const string REDIS_HM_PROTOCOL = "HM_PROTOCOL";


        /// <summary>
        /// redis 四信GPS数据发送接收频道
        /// </summary>
        public const string REDIS_GPS_SiXingPROTOCOL = "GPSSiXingPROTOCOL";


        /// <summary>
        /// redis 命令发送接收频道
        /// </summary>
        public const string REDIS_COMMOND_PROTOCOL = "REDIS_COMMOND_PROTOCOL";


        /// <summary>
        /// 设备连接状态通知频道
        /// </summary>
        public const string REDIS_CONNSTATECHANGE = "REDIS_CONNSTATECHANGE";


        /// <summary>
        /// redis 设备数据保存前缀
        /// </summary>
        public const string REDIS_DATA_PREFIX = "data_";

        /// <summary>
        /// 在线在线状态前缀
        /// </summary>
        public const string REDIS_LINE_DEVS = "line_devs";


        /// <summary>
        /// 缓存命令字典
        /// </summary>
        public const string REDIS_CURRENT_CACHECMDS = "REDIS_CURRENT_CACHECMDS";


        /// <summary>
        /// 在线在线状态前缀
        /// </summary>
        public const string REDIS_STATUS_CARS = "line_car_status";

        /// <summary>
        /// 实时数据保存KEY
        /// </summary>
        public const string REDIS_REAL_DATA_KEY = "REAL_DATA";

        /// <summary>
        /// 实时GPS数据保存KEY
        /// </summary>
        public const string REDIS_REAL_GPS_KEY = "REAL_GPS";



        /// <summary>
        /// 设置当前监控设备号的命令
        /// </summary>
        public const string SET_DEVNO = "setdevno";

        /// <summary>
        /// 查看当前运行状态信息
        /// </summary>
        public const string SET_STATUS = "show";

        /// <summary>
        /// 退出容器服务命令
        /// </summary>
        public const string SET_EXIT = "exit";

        /// <summary>
        /// 原始数据索引
        /// </summary>
        public const string ELASTIC_ORGINDEX = "orgdata";

        /// <summary>
        /// 原始数据类型前缀
        /// </summary>
        public const string ELASTIC_ORGTYPE_PREFIX = "org_";

        /// <summary>
        /// GPS数据索引
        /// </summary>
        public const string ELASTIC_GPSINDEX = "gpsdata";

        /// <summary>
        /// GPS数据类型前缀
        /// </summary>
        public const string ELASTIC_GPSTYPE_PREFIX = "gps_";



        /// <summary>
        /// EVENT数据类型前缀
        /// </summary>
        public const string ELASTIC_EVENT_PREFIX = "event_";

        /// <summary>
        /// 响应命令类型前缀
        /// </summary>
        public const string ELASTIC_CONTROL_CMDFIX = "controlcmd_";


        /// <summary>
        /// 响应命令保存KEY
        /// </summary>
        public const string ELASTIC_CONTROL_CMD = "CONTROL_CMD";
        /// <summary>
        /// 接入服务IP地址与协议字符分割符号
        /// </summary>
        public const char ACCESSSERVER_SPLIT_IP = 'ā';


        /// <summary>
        /// 保存实时数据固定列名.接收数据时间
        /// </summary>
        public const string DATA_RECDATE_NAME = "数据时间";

        /// <summary>
        /// 保存实时数据固定列名.数据保存时间
        /// </summary>
        public const string DATA_SYSDATE_NAME = "更新时间";


        /// <summary>
        /// 保存实时数据固定列名.数据保存时间
        /// </summary>
        //public const string DATA_SYSDATE_NAME = "更新时间";

        /// <summary>
        /// 数据保存服务
        /// </summary>
        public const string Service_Default_DataSave = "DataSave";

        /// <summary>
        /// Gps数据保存服务名称
        /// </summary>
        public const string Service_Default_GpsDataSave = "GPSSave";

        /// <summary>
        /// 告警
        /// </summary>
        public const string Redis_Warn = "Warn";

        /// <summary>
        /// 修改记录时,旧主键和修改后的主键分割符号
        /// </summary>
        public const string UpKeySplit = "[~KeyField]";


        public const string PROTOCOL_STPROTOCOL = "STPROTOCOL";

        public const string PROTOCOL_GPSSiXingPROTOCOL = "GPSSiXingPROTOCOL";

        public const string PROTOCOL_CANPROTOCOL = "CANPROTOCOL";

        public const string PROTOCOL_GBCANPROTOCOL = "GBCANPROTOCOL";

        #region 科大模块变量

        /// <summary>
        /// 锁车命令集合
        /// </summary>
        public const string REDIS_KDJT_LOCKCMD = "REDIS_KDJT_LOCKCMD";

        /// <summary>
        /// redis已锁车设备集合
        /// </summary>
        public const string REDIS_KDJT_CANJIHUO = "REDIS_KDJT_CANJIHUO";

        /// <summary>
        /// redis当前执行命令
        /// </summary>
        public const string REDIS_KDJT_CURRENTCMD = "REDIS_KDJT_CURRENTCMD";

        #endregion

        /// <summary>
        /// 异常数据
        /// </summary>
        public const int DATA_EXCEPTION = 999999;

        /// <summary>
        /// 无效数据
        /// </summary>
        public const int DATA_INVALID =999998;
    }
}
