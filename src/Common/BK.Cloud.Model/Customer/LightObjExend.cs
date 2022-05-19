using Fireasy.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fireasy.Common.Extensions;
using Fireasy.Data.Entity;
using System.Xml;
using Fireasy.Common.ComponentModel;
using Fireasy.Common.Caching;
using Fireasy.Common.Emit;
using System.Reflection;
using System.Dynamic;
using BK.Cloud.Tools;

namespace BK.Cloud.Model.Customer
{
    public static class LightObjExend
    {


        public static IDictionary<string, object> ObjToDictionary(this object source)
        {

            var target = new Dictionary<string, object>();
#if !N35 && DYNAMIC
            TypeDescriptorUtility.AddDefaultDynamicProvider();
#endif
            var otherProperties = System.ComponentModel.TypeDescriptor.GetProperties(source);
            foreach (System.ComponentModel.PropertyDescriptor p in otherProperties)
            {
                if (p.IsDefined<PropertyMappingAttribute>())
                {
                    var targetProperty = otherProperties[p.Name];
                    var value = p.GetValue(source);
                    if (value != null && value.GetType().IsPrimitive)
                    {
                        target.Add(p.Name, value.ToStringSafely());
                    }
                    else
                    {
                        target.Add(p.Name, value);
                    }
                }
            }
            return target;
        }

        public static IDictionary<string, object> ObjToDictionaryExtend(this object source, object newobj)
        {
#if !N35 && DYNAMIC
            TypeDescriptorUtility.AddDefaultDynamicProvider();
#endif
            var target = (source is IDictionary<string, object>) ? (source as IDictionary<string, object>) : source.ObjToDictionary();
            var newobjProperties = System.ComponentModel.TypeDescriptor.GetProperties(newobj);
            foreach (System.ComponentModel.PropertyDescriptor p in newobjProperties)
            {
                //var targetProperty = newobjProperties[p.Name];
                var value = p.GetValue(newobj);
                if (value != null && value.GetType().IsPrimitive)
                {
                    target.Add(p.Name, value.ToStringSafely());
                }
                else
                {
                    target.Add(p.Name, value);
                }
            }
            return target;
        }

        public static T Clone<T>(this object source) where T : class,new()
        {
            var propertys = typeof(T).GetProperties();
            var newobjProperties = System.ComponentModel.TypeDescriptor.GetProperties(source);
            T t = new T();
            foreach (System.ComponentModel.PropertyDescriptor property in newobjProperties)
            {
                var findproperty = propertys.FirstOrDefault(o => o.Name == property.Name);
                if (findproperty != null && findproperty.CanRead && findproperty.CanWrite)
                {
                    var val = property.GetValue(source);
                    var type = findproperty.PropertyType;
                    if (val.ToStringSafely() == "[null]")
                    {
                        val = type.GetDefaultValue();
                    }
                    var cv = CommonHelper.ChangeType(findproperty.PropertyType, val);
                    findproperty.SetValue(t, cv);
                }
            }
            return t;
        }

        public static void Copy<T>(object source, T dest) where T : class,new()
        {
            var propertys = typeof(T).GetProperties();
            var newobjProperties = System.ComponentModel.TypeDescriptor.GetProperties(source);

            foreach (System.ComponentModel.PropertyDescriptor property in newobjProperties)
            {
                var findproperty = propertys.FirstOrDefault(o => o.Name == property.Name);
                if (findproperty != null)
                {
                    var val = property.GetValue(source);
                    var type = findproperty.PropertyType;
                    if (val.ToStringSafely() == "[null]")
                    {
                        val = type.GetDefaultValue();
                    }
                    var cv = CommonHelper.ChangeType(findproperty.PropertyType, val);
                    findproperty.SetValue(dest, cv);
                }
            }
        }


        public static Condition ConvertoObjToCondition(this object source, string objname)
        {
            Condition condition = new Condition();
            condition.datatype = objname;
            var newobjProperties = System.ComponentModel.TypeDescriptor.GetProperties(source);
            foreach (System.ComponentModel.PropertyDescriptor property in newobjProperties)
            {
                var val = property.GetValue(source);
                if (val != null && val != DBNull.Value)
                {
                    condition[property.Name] = val.ToString();
                }
            }
            return condition;
        }

        public static T ConverConditionToObj<T>(Condition condition) where T : new()
        {

            T t = new T();
            var newobjProperties = System.ComponentModel.TypeDescriptor.GetProperties(t);

            foreach (PropertyDescriptor property in newobjProperties)
            {
                var cv = CommonHelper.ChangeType(property.PropertyType, condition[property.Name]);
                property.SetValue(t, cv);
            }

            return t;
        }

        /// <summary>
        /// 使用另一个对象对源对象进行扩展，这类似于 jQuery 中的 extend 方法。
        /// </summary>
        /// <param name="source">源对象。</param>
        /// <param name="other">用于扩展的另一个对象。</param>
        /// <returns></returns>
        //public static IDictionary<string, object> ObjToDictionaryU(this object source, object other)
        //{
        //    if (source == null)
        //    {
        //        return new Dictionary<string, object>();
        //    }

        //    if (other == null)
        //    {
        //        return new Dictionary<string, object>();
        //    }

        //    TypeDescriptorUtility.AddDefaultDynamicProvider();
        //    var sourceProperties = TypeDescriptor.GetProperties(source);
        //    var otherProperties = TypeDescriptor.GetProperties(other);
        //    var expando = new ExpandoObject();
        //    var dictionary = (IDictionary<string, object>)expando;

        //    var sourceLazy = source as ILazyManager;
        //    var otherLazy = other as ILazyManager;
        //    foreach (PropertyDescriptor p in sourceProperties)
        //    {
        //        if (sourceLazy != null && !sourceLazy.IsValueCreated(p.Name))
        //        {
        //            continue;
        //        }

        //        dictionary.Add(p.Name, p.GetValue(source));
        //    }

        //    foreach (PropertyDescriptor p in otherProperties)
        //    {
        //        if (otherLazy != null && !otherLazy.IsValueCreated(p.Name))
        //        {
        //            continue;
        //        }

        //        if (!dictionary.ContainsKey(p.Name))
        //        {
        //            dictionary.Add(p.Name, p.GetValue(other));
        //        }
        //    }

        //    return (IDictionary<string, object>)expando;
        //}



        /// <summary>
        /// 将数组或 <see cref="IEnumerable"/> 中的成员转换到 <see cref="DataTable"/> 对象。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ObjToTable<T>(this T data)
        {
            if (data == null)
            {
                return null;
            }

            var table = data as DataTable;
            if (table == null)
            {
                if (data is IEnumerable)
                {
                    table = ParseFromEnumerable(data as IEnumerable);
                }
                else
                {
                    table = ParseFromEnumerable(new List<object> { data });
                }
            }

            if (table != null)
            {
                table.TableName = typeof(T).Name;
            }

            return table;
        }


        private static DataTable ParseFromEnumerable(IEnumerable enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            var table = new DataTable();
            var flag = new AssertFlag();
            PropertyDescriptorCollection properties = null;
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (flag.AssertTrue())
                {
                    properties = TypeDescriptor.GetProperties(current);

                    foreach (PropertyDescriptor pro in properties)
                    {
                        if (pro.IsDefined<PropertyMappingAttribute>())
                        {
                            table.Columns.Add(pro.Name, pro.PropertyType.GetNonNullableType());
                        }
                    }
                }

                DataRow row = table.NewRow();

                for (var i = 0; i < properties.Count; i++)
                {
                    if (properties[i].IsDefined<PropertyMappingAttribute>())
                    {
                        row[properties[i].Name] = properties[i].GetValue(current) ?? DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static void ObjToXmlNode(this XmlDocument doc, EntityObject obj, XmlNode node)
        {

#if !N35 && DYNAMIC
            TypeDescriptorUtility.AddDefaultDynamicProvider();
#endif
            var otherProperties = System.ComponentModel.TypeDescriptor.GetProperties(obj);
            foreach (System.ComponentModel.PropertyDescriptor p in otherProperties)
            {
                if (p.IsDefined<PropertyMappingAttribute>())
                {
                    XmlAttribute attr = doc.CreateAttribute(p.Name);
                    var targetProperty = otherProperties[p.Name];
                    var value = p.GetValue(obj);
                    attr.Value = value.ToStringSafely();
                    node.Attributes.Append(attr);
                }
            }
        }

        public static void XmlNodeToObj(this Object obj, XmlNode node)
        {

#if !N35 && DYNAMIC
            TypeDescriptorUtility.AddDefaultDynamicProvider();
#endif
            var otherProperties = System.ComponentModel.TypeDescriptor.GetProperties(obj);
            foreach (System.ComponentModel.PropertyDescriptor p in otherProperties)
            {
                if (p.IsDefined<PropertyMappingAttribute>())
                {
                    if (node.Attributes != null && node.Attributes[p.Name] != null)
                    {

                        p.SetValue(obj, ChangeType(p.PropertyType, node.Attributes[p.Name].Value));
                    }

                    //XmlAttribute attr = doc.CreateAttribute(p.Name);
                    //var targetProperty = otherProperties[p.Name];
                    //var value = p.GetValue(obj);
                    //attr.Value = value.ToStringSafely();
                    //node.Attributes.Append(attr);
                }
            }
        }


        public static EntityObject ConvertRowToObj(this DataTable tb, DataRow row)
        {
            //var dataobj=
            string classpath = "BK.Cloud.Model.Data.Model." + tb.TableName;
            var assemblyBuilder = Assembly.Load("BK.Cloud.Model");
            var vtype = assemblyBuilder.GetType(classpath);
            string key = Newtonsoft.Json.JsonConvert.SerializeObject(row);
            var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            var res = serializer.Deserialize(key, vtype).As<Fireasy.Data.Entity.EntityObject>();
            return res;
        }

        public static DataTable DicToDataTable(this Dictionary<object, object> objs)
        {
            string key = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
            key = "[" + key + "]";
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(key);
            //var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            // return serializer.Deserialize<DataTable>(key);
        }

        public static DataTable DicToDataTable(this List<Dictionary<string, object>> objs)
        {
            string key = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(key);
            //key = "[" + key + "]";
            //var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            //return serializer.Deserialize<DataTable>(key);
        }

        public static DataTable DicToDataTable(this List<Dictionary<object, object>> objs)
        {
            string key = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(key);
            //key = "[" + key + "]";
            //var serializer = new Fireasy.Common.Serialization.JsonSerializer();
            //return serializer.Deserialize<DataTable>(key);
        }
        /// <summary>
        /// 将objectd对象转换为指定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object ChangeType(Type type, object value)
        {

            //var val = value as string;
            //if (val != null && !type.IsPrimitive)
            //{
            //    if (val.ToLower() == "null")
            //    {
            //        return null;
            //    }
            //}

            if ((value == null) && type.IsGenericType)
            {

                return Activator.CreateInstance(type);

            }
            if ((value as string) == string.Empty && type.IsGenericType)
                return null;
            if (value == null)
            {

                return null;

            }

            if (type == value.GetType())
            {

                return value;

            }

            if (type.IsEnum)
            {

                if (value is string)
                {

                    return Enum.Parse(type, value as string);

                }

                return Enum.ToObject(type, value);

            }

            if (!type.IsInterface && type.IsGenericType)
            {

                Type type1 = type.GetGenericArguments()[0];

                object obj1 = ChangeType(type1, value);
                if (obj1 == null)
                    return null;
                return Activator.CreateInstance(type, new object[] { obj1 });
            }

            if ((value is string) && (type == typeof(Guid)))
            {

                return new Guid(value as string);

            }

            if ((value is string) && (type == typeof(Version)))
            {

                return new Version(value as string);

            }

            if (!(value is IConvertible))
            {

                return value;

            }
            if (type.IsPrimitive && value is string && value.ToString() == "")
            {
                return null;
            }
            if (type == typeof(bool))
            {
                if ((value as string) == "1")
                {
                    return Convert.ChangeType("true", type);
                }
                else if ((value as string) == "0")
                {
                    return Convert.ChangeType("false", type);
                }
            }
            return Convert.ChangeType(value, type);

        }

    }
}
