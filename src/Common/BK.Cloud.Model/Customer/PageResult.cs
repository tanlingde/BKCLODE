using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.Data;

namespace BK.Cloud.Model.Customer
{
    [Serializable]
    public class PageResult
    {

        /// <summary>
        /// 数据总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 数据集
        /// </summary>
        public DataTable rows { get; set; }
    }

}
