using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Model.Customer
{

    /// <summary>
    /// 错误代码表
    /// </summary>
    public class ErrCode
    {
        static List<ErrCode> codes = new List<ErrCode>();
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <param name="codeId"></param>
        /// <param name="errMsg"></param>
        public ErrCode(int codeId, string errMsg)
        {
            this.CodeId = codeId;
            this.ErrMsg = errMsg;
            codes.Add(this);
        }

        /// <summary>
        /// 代码ID
        /// </summary>
        public int CodeId { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 格式化字符信息
        /// </summary>
        public string String
        {
            get { return "err:" + CodeId + "|" + ErrMsg; }
        }

        public static ErrCode GetCodeById(int codeId)
        {
            return codes.Find(o => o.CodeId == codeId);
        }

        public static ErrCode Parse(string codestr)
        {
            codestr = codestr.Replace("err:", "");
            string[] arrs = codestr.Split('|');
            return new ErrCode(int.Parse(arrs[0]), arrs[1]);
        }

        #region 一般错误信息
        public static readonly ErrCode 参数录入错误 = new ErrCode(-1, "参数录入错误");

        public static readonly ErrCode 设备从没上线 = new ErrCode(-2, "设备从没上线");


        public static readonly ErrCode 设备不在线 = new ErrCode(-3, "设备不在线");

        public static readonly ErrCode 命令参数错误 = new ErrCode(-4, "命令参数错误");

        public static readonly ErrCode 变量名错误 = new ErrCode(-5, "变量名错误");

        public static readonly ErrCode 变量值错误 = new ErrCode(-6, "变量值错误");

        public static readonly ErrCode 设备ID错误 = new ErrCode(-7, "设备ID错误");

        public static readonly ErrCode 变量名重复 = new ErrCode(-8, "变量名重复");

        public static readonly ErrCode 信息点格式错误 = new ErrCode(-9, "信息点格式错误");

        public static readonly ErrCode 时间跨度过大 = new ErrCode(-10, "时间跨度过大");

        public static readonly ErrCode 设备应用配置错误 = new ErrCode(-11, "设备应用配置错误");

        public static readonly ErrCode 公司ID错误 = new ErrCode(-12, "公司ID错误");

        public static readonly ErrCode 应用代码错误 = new ErrCode(-13, "应用代码错误");

        public static readonly ErrCode 设备号错误 = new ErrCode(-14, "设备号错误");

        public static readonly ErrCode 密码修改不成功 = new ErrCode(-15, "密码修改不成功");

        public static readonly ErrCode 没有权限 = new ErrCode(-16, "没有权限");

        public static readonly ErrCode 没有找到数据 = new ErrCode(-17, "没有找到数据");

        public static readonly ErrCode 页码错误 = new ErrCode(-18, "页码错误");

        public static readonly ErrCode 设备应用不存在参数变量 = new ErrCode(-19, "设备应用不存在参数变量");

        public static readonly ErrCode 设备无数据 = new ErrCode(-101, "设备无数据");

        public static readonly ErrCode 用户名密码错误 = new ErrCode(-102, "用户名密码错误");

        public static readonly ErrCode 命令执行错误 = new ErrCode(-103, "命令执行错误");

        public static readonly ErrCode 用户没有设备 = new ErrCode(-104, "用户没有设备");

        public static readonly ErrCode 时间格式错误 = new ErrCode(-105, "时间格式错误");

        #endregion

        #region 命令错误参数

        public static readonly ErrCode 布尔传值错误 = new ErrCode(-23, "BCD布尔传值错误");

        public static readonly ErrCode 单精度类型命令格式错误 = new ErrCode(-24, "单精度类型命令格式错误");

        public static readonly ErrCode BCD长整形命令格式错误 = new ErrCode(-30, "BCD长整形命令格式错误");

        public static readonly ErrCode 长整形命令格式错误 = new ErrCode(-25, "长整形命令格式错误");

        public static readonly ErrCode 无符号长整形命令格式错误 = new ErrCode(-26, "无符号长整形命令格式错误");

        public static readonly ErrCode 短整型格式不正确 = new ErrCode(-27, "短整型格式不正确");

        public static readonly ErrCode 无符号短整形命令格式错误 = new ErrCode(-28, "无符号短整形命令格式错误");

        public static readonly ErrCode BCD类形命令格式错误 = new ErrCode(-29, "BCD类形命令格式错误");

        public static readonly ErrCode Byte类型格式错误 = new ErrCode(-31, "Byte类型格式错误");

        public static readonly ErrCode 不能识别数据类型 = new ErrCode(-32, "不能识别数据类型");

        public static readonly ErrCode 数据转换异常 = new ErrCode(-33, "数据转换异常");

        #endregion


        public static readonly ErrCode 未知异常 = new ErrCode(-1000, "未知异常");

        public static readonly ErrCode 网络异常 = new ErrCode(-900, "网络异常");

        public static readonly ErrCode 数据库异常 = new ErrCode(-901, "数据库异常");

    }

}
