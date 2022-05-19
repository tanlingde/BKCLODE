using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Model.Data.Model;
using Fireasy.Data.Entity;

namespace BK.Cloud.Model.Customer
{
    /// <summary>
    /// 获取用户信息接口参数
    /// </summary>
    public class UserInfoParams
    {
        public string name { get; set; }//输入的用户名
    }

    /// <summary>
    /// 获取用户信息接口返回数据
    /// </summary>
    public class UserInfoData
    {
        [PropertyMapping]
        public long UserID { get; set; }//用户ID
        [PropertyMapping]
        public string Pwd { get; set; }//用户密码
        [PropertyMapping]
        public string Phone { get; set; }//用户联系方式
        [PropertyMapping]
        public string CarNumber { get; set; }//用户绑定车辆
        [PropertyMapping]
        public long OrgID { get; set; }//用户所属机构
        [PropertyMapping]
        public string OrgName { get; set; }//所属机构名
    }
}
