using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fireasy.Common.Extensions;

namespace BK.Cloud.Model.Customer
{
    public class Condition : Dictionary<string, string>
    {
        Dictionary<string, string> _where;
        public Condition()
            : base(StringComparer.OrdinalIgnoreCase)
        {
            showcols = "*";
            page = 1;
            rows = int.MaxValue;
            isaddblank = false;
            cmd = "";
            _where = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        const string syskeys = "|cmd|op|datatype|checkBool|showcols|page|rows|order|sort|groupbycols|isaddblank|where|";

        public static string GetConditionKeys()
        {
            return syskeys;
        }

        public new string this[string index]
        {
            get
            {
                if (ContainsKey(index))
                    return base[index];
                if (_where.ContainsKey(index))
                    return _where[index];
                return null;
            }
            set
            {
                if (syskeys.IndexOf("|" + index + "|", StringComparison.Ordinal) == -1)
                {
                    _where[index] = value;
                    return;
                }
                base[index] = value;
            }
        }
        /// <summary>
        /// 查询命令
        /// </summary>
        public string cmd
        {
            get
            {
                if (!this.ContainsKey("cmd"))
                    return null;
                return this["cmd"];
            }
            set
            {
                this["cmd"] = value;
            }
        }
        /// <summary>
        /// 查询方式,通用全局。query:like %条件% 模糊查询，queryext:like 条件 自定义模糊查询，其他：=查询
        /// </summary>
        public string op
        {
            get
            {
                if (!this.ContainsKey("op"))
                    return null;
                return this["op"];
            }
            set
            {
                this["op"] = value;
            }
        }
        /// <summary>
        /// 查找数据表或视图
        /// </summary>
        public string datatype
        {
            get
            {
                if (!this.ContainsKey("datatype"))
                    return null;
                return this["datatype"];
            }
            set
            {
                this["datatype"] = value;
            }
        }

        /// <summary>
        /// 返回结果是否只包含true和false
        /// </summary>
        public bool checkBool
        {
            get
            {
                if (!this.ContainsKey("checkBool"))
                    return false;
                return this["checkBool"] == "true" ? true : false;
            }
            set
            {
                this["checkBool"] = value ? "true" : "false";
            }
        }

        /// <summary>
        /// 查找的列
        /// </summary>
        public string showcols
        {
            get
            {
                if (!this.ContainsKey("showcols"))
                    return null;
                return this["showcols"];
            }
            set
            {
                this["showcols"] = value;
            }
        }

        /// <summary>
        /// 查找的页,以1开始
        /// </summary>
        public int page
        {
            get
            {
                if (!this.ContainsKey("page"))
                    return 1;
                int mpage;
                if (int.TryParse(this["page"], out mpage))
                {
                    if (mpage < 1)
                    {
                        return 1;
                    }
                    return mpage;
                }
                return 1;
            }
            set
            {
                this["page"] = "" + value;
            }
        }

        /// <summary>
        /// 每页记录条数
        /// </summary>
        public int rows
        {
            get
            {
                if (!this.ContainsKey("rows"))
                    return 1;
                int mrows;
                if (int.TryParse(this["rows"], out mrows))
                {
                    return mrows;
                }
                return 1;
            }
            set
            {
                this["rows"] = "" + value;
            }
        }

        /// <summary>
        /// 升序还是降序
        /// </summary>
        public string order
        {
            get
            {
                if (!this.ContainsKey("order"))
                    return null;
                return this["order"];
            }
            set
            {
                this["order"] = value;
            }
        }

        /// <summary>
        /// 排序的字段
        /// </summary>
        public string sort
        {
            get
            {
                if (!this.ContainsKey("sort"))
                    return null;
                return this["sort"];
            }
            set
            {
                this["sort"] = value;
            }
        }

        /// <summary>
        /// 分组的列
        /// </summary>
        public string groupbycols
        {
            get
            {
                if (!this.ContainsKey("groupbycols"))
                    return null;
                return this["groupbycols"];
            }
            set
            {
                this["groupbycols"] = value;
            }
        }

        /// <summary>
        /// 是否添加一条空记录放结果集表头
        /// </summary>
        public bool isaddblank
        {
            get
            {
                if (!this.ContainsKey("isaddblank"))
                    return false;
                return this["isaddblank"] == "true" ? true : false;
            }
            set
            {
                this["isaddblank"] = value ? "true" : "false";
            }
        }
        /// <summary>
        /// 查询条件集合
        /// </summary>
        public Dictionary<string, string> where
        {
            get
            {
                return _where;
            }
        }

        public T WhereConvertToObj<T>() where T : new()
        {
            T t = new T();
            var pars = t.GetType().GetProperties();
            foreach (var v in where.Keys)
            {
                var pv = pars.FirstOrDefault(o => o.Name.ToLower().Equals(v.ToLower()));
                if (pv != null)
                {
                    if (where[v] == "[null]" || where[v] == "")
                    {
                        pv.SetValue(t, LightObjExend.ChangeType(pv.PropertyType, pv.PropertyType.GetDefaultValue()));
                    }
                    else
                    {
                        pv.SetValue(t, LightObjExend.ChangeType(pv.PropertyType, where[v]));
                    }
                }
            }
            return t;
        }


        //public string this[string index]    // Indexer declaration
        //{
        //    get
        //    {
        //        if (where == null)
        //            return null;
        //        if (where.ContainsKey(index))
        //        {
        //            return where[index];
        //        }
        //        return null;
        //    }
        //    set {
        //        where[index] = value;
        //    }
        //}
    }
}
