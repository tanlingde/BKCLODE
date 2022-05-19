using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Model.Customer
{
    public class Enums
    {
        /// <summary>
        /// 系统资源类型
        /// </summary>
        public class restype
        {
            public const int id = 1;
            public const int 菜单资源 = 2;
            public const int 页面资源 = 3;
            public const int 组件资源 = 4;
            public const int 图片资源 = 5;
            public const int 视频资源 = 6;
            public const int 配置文件资源 = 7;
            public const int 按纽资源 = 8;
            public const int 设备资源 = 9;
            public const int 设备下拉资源 = 53;
            public const int 车辆类型资源 = 58;
            public const int 自定义资源 = 101;
        }


        /// <summary>
        /// 任务状态
        /// </summary>
        public class taskstatus
        {
            public const int id = 10;
            public const int 订单创建 = 11;
            public const int 完成派车 = 12;
            public const int 救援车已确认 = 13;
            public const int 新能源车已确认 = 46;
            public const int 救援已发车 = 14;
            public const int 已开始充电 = 15;
            public const int 充电结束 = 16;
            public const int 订单结束 = 17;
            public const int 订单已取消 = 18;
        }


        /// <summary>
        /// 支付状态
        /// </summary>
        public class taskpay
        {
            public const int id = 19;
            public const int 未支付 = 20;
            public const int 支付押金 = 21;
            public const int 支付完成 = 22;
            public const int 未退款 = 23;
            public const int 退款完成 = 24;
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        public class usertype
        {
            public const int id = 25;
            public const int 平台用户 = 26;
            public const int 新能源车用户 = 27;
            public const int 救援车用户 = 28;
            public const int 平台管理员 = 51;
            public const int 普通用户 = 0;
            public const int 客户 = 1;
            public const int 管理者 = 2;
            public const int 售后 = 3;
            public const int 内勤 = 4;
            public const int 研究人员 = 5;
        }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public class cartype
        {
            public const int id = 29;
            public const int 新能源车 = 30;
            public const int 救援车 = 31;
            public const int 定点充电车 = 52;
            public const int 海马高速车 = 56;
            public const int 海马低速车 = 57;
            public const int 远大空调 = 69;
            public const int 科达设备 = 70;
            public const int 国标通用设备 = 71;
            public const int 湘潭人医设备 = 72;
            public const int 凯龙高科设备 = 73;
            public const int 科达旧设备 = 74;
            public const int 中大机械 = 75;
            public const int 迈科科技 = 76;
            public const int 迈科碎木机 = 77;
            public const int 五新隧装设备 = 78;
            public const int 中铁五新设备 = 80;
            public const int 远大机房 = 81;
            public const int 国标海马设备 = 98;
            public const int 五星隧装新设备 = 138;
            public const int 拱架车部标 = 75;
        }



        /// <summary>
        /// 设备状态
        /// </summary>
        public class statustype
        {
            public const int id = 35;
            public const int 从未上线 = 36;
            public const int 离线 = 37;
            public const int 停车 = 38;
            public const int 空闲 = 39;
            public const int 救援中 = 43;
            public const int 充电中 = 44;
            public const int 报警 = 45;
            public const int 在线 = 54;
            public const int 行驶 = 55;

            //public const int 充电车在线 = 36;
        }



        public class payway
        {
            public const int id = 47;

            /// <summary>
            /// 支付宝支付
            /// </summary>
            public const int zhifubao = 48;

            /// <summary>
            /// 微信支付
            /// </summary>
            public const int webchart = 49;

            /// <summary>
            /// 线下支付
            /// </summary>

            public const int offline = 50;

        }

        /// <summary>
        /// 角色类型 对应 role>roletype
        /// </summary>
        public class roletype
        {
            public const int id = 62;

            /// <summary>
            /// 角色
            /// </summary>
            public const int 角色 = 63;

            /// <summary>
            /// 车队
            /// </summary>
            public const int 车队 = 64;

        }

        /// <summary>
        /// 加载树类型 
        /// </summary>
        public class treetype
        {
            public const int id = 59;

            /// <summary>
            /// 区域树
            /// </summary>
            public const int 区域树 = 60;

            /// <summary>
            /// 车队树
            /// </summary>
            public const int 车队树 = 61;

        }

        /// <summary>
        /// 事件处理方式 
        /// </summary>
        public class channelno
        {

            public const int id = 65;

            /// <summary>
            /// 告警产生.说明告警发生后，未作任何处理
            /// </summary>
            public const int 告警产生 = 66;

            /// <summary>
            /// 已手动消除,人为手动消除报警或事件
            /// </summary>
            public const int 已手动消除 = 67;
            /// <summary>
            /// 已自动消除,系统自动消除报警或事件
            /// </summary>
            public const int 已自动消除 = 68;

        }


        /// <summary>
        /// 协议类型
        /// </summary>
        public class Pacttype
        {

            public const int id = 90;

            /// <summary>
            /// 告警产生.说明告警发生后，未作任何处理
            /// </summary>
            public const int 国标协议 = 91;

            /// <summary>
            /// 已手动消除,人为手动消除报警或事件
            /// </summary>
            public const int 部标协议 = 92;
            /// <summary>
            /// 已自动消除,系统自动消除报警或事件
            /// </summary>
            public const int 远程服务协议 = 93;

            /// <summary>
            /// 告警产生.说明告警发生后，未作任何处理
            /// </summary>
            public const int 新能源车协议 = 94;

            /// <summary>
            /// 已手动消除,人为手动消除报警或事件
            /// </summary>
            public const int 充电车协议 = 95;

            /// <summary>
            /// 已自动消除,系统自动消除报警或事件
            /// </summary>
            public const int JSON协议 = 96;

            /// <summary>
            /// 已自动消除,系统自动消除报警或事件
            /// </summary>
            public const int 深拓协议 = 97;
 
        }

    }

}
