using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BK.Cloud.Model.Customer
{

    /// <summary>
    /// easyui 渲染tree的对象模型
    /// </summary>
    [Serializable]
    public class ResTreeGridJson
    {
        public ResTreeGridJson()
        {
            children = new List<ResTreeGridJson>();
        }
        public string ResId { get; set; }

        public string Name { get; set; }

        public List<ResTreeGridJson> children { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public string SaveResId { get; set; }

        public string ResType { get; set; }

        public string iconCls { get; set; }
    }




}
