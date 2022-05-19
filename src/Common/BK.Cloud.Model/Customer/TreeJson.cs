using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace BK.Cloud.Model.Customer
{

    /// <summary>
    /// easyui 渲染tree的对象模型
    /// </summary>
    [Serializable]
    public class TreeJson
    {
        public TreeJson()
        {
            children = new List<TreeJson>();
        }

        public TreeJson(string idv, string textv)
        {
            id = idv;
            text = textv;
        }
        /// <summary>
        /// id主键
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 文本内容
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 页面路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 组织机构id
        /// </summary>
        public string orgid { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string other { get; set; }

        /// <summary>
        /// 层次等级
        /// </summary>
        public string level { get; set; }

        /// <summary>
        /// 树节点css样式
        /// </summary>
        public string iconCls { get; set; }

        ///// <summary>
        ///// ztree专用
        /// /Content/Themesextend/default/st_devicetree/images/data[i].split(',')[1].png
        ///// </summary>
        //public string icon
        //{
        //    get { return iconCls; }
        //}

        ///// <summary>
        ///// 树节点css样式 ztree专用
        ///// </summary>
        //public bool open
        //{
        //    get { return state != "closed"; }
        //}

        ///// <summary>
        ///// ztree专用,节点名称
        ///// </summary>
        //public string name
        //{
        //    get { return text; }
        //}

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 关联对象
        /// </summary>
        public string attachobj { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public string devtype { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public int cartype { get; set; }


        /// <summary>
        /// vin
        /// </summary>
        public string vin { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }



        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeJson> children { get; set; }

        private string _state = null;
        /// <summary>
        /// 节点状态。默认是展开
        /// </summary>
        public string state
        {
            get { return _state; }
            set { _state = value; }
        }


        /// <summary>
        /// 设备上线时间[如果是设备]
        /// </summary>
        public DateTime? linetime { get; set; }


        /// <summary>
        /// 设备离线时间[如果是设备]
        /// </summary>
        public DateTime? offlinetime { get; set; }



        private bool _easyuihecked = false;

        /// <summary>
        /// 是否选中
        /// </summary>
        [JsonProperty(PropertyName = "checked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Checked
        {
            get { return _easyuihecked; }
            set { _easyuihecked = value; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int? DisplayIndex { get; set; }
    }
}
