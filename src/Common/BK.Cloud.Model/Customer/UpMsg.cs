using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BK.Cloud.Model.Customer
{
    [Serializable]
    public class UpMsg
    {
        public bool success { get; set; }

        public string errCode { get; set; }

        public string message { get; set; }

        public string retid { get; set; }

        public object data { get; set; }

        public string code { get; set; }

        public UpMsg()
        {
      
        }

        public UpMsg Clone()
        {
            return new UpMsg() { success = this.success, message = this.message, retid = this.retid };
        }

        public UpMsg(string tmessage, bool success = true, string retid = null)
        {
            this.success = success;
            this.message = tmessage;
            this.retid = retid;
        }


        #region 数据更新消息
        public static UpMsg SuccessMsg = new UpMsg("数据更新成功");
        public static UpMsg ExceptionMsg = new UpMsg("出现异常,数据更新失败", false);
        public static UpMsg FailMsg = new UpMsg("数据更新失败", false);
        #endregion

        #region 删除数据消息
        public static UpMsg DelMsgSuccess = new UpMsg("数据删除成功");
        public static UpMsg DelMsgFail = new UpMsg("数据删除失败", false);
        public static UpMsg DelMsgException = new UpMsg("数据删除失败", false);
        public static UpMsg DelMsgRelationException = new UpMsg("数据存在关联，不能删除", false);
        #endregion

        public static UpMsg NotFindObjMsg = new UpMsg("方法执行错误，没有实现", false);

        public static UpMsg UnFindObjMsg = new UpMsg("没有找到指定对象", false);
        public static UpMsg QueryExceptionMsg = new UpMsg("数据获取失败", false);
    }


}
