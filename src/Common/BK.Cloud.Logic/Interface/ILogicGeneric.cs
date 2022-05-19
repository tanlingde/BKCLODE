using Fireasy.Data;
using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Logic.Interface
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public interface ILogicGeneric : IDisposable
    {
        /// <summary>
        /// 增加记录时，前端调用获取唯一键值
        /// </summary>
        /// <returns></returns>
        long GetNewID();

        /// <summary>
        /// 导入表格
        /// </summary>
        /// <param name="datatable"></param>
        /// <returns></returns>
        int ImportTable(DataTable datatable);

        /// <summary>
        /// 查找表的结构,导出模板用
        /// </summary>
        /// <returns></returns>
        DataTable QueryTableStructs(string tablename);


        UpMsg ConditionInsertObj(Condition condition, DbContext db = null);

        UpMsg ConditionUpDateObj(Condition condition, DbContext db = null);

        UpMsg ConditionDeleteObj(Condition condition, DbContext db = null);

        PageResult ConditionQueryData(Condition condition, DbContext db = null);

        DataTable ConditionQueryAll(Condition condition, DbContext db = null);



        ///// <summary>
        ///// 删除对象
        ///// </summary>
        ///// <param name="efEntities"></param>
        ///// <returns></returns>
        //int InsertObj(params Fireasy.Data.Entity.IEntity[] efEntities);
        ///// <summary>
        ///// 删除对象
        ///// </summary>
        ///// <param name="efEntities"></param>
        ///// <returns></returns>
        //int DelObj(params Fireasy.Data.Entity.IEntity[] efEntities);

        ///// <summary>
        ///// 更新对象
        ///// </summary>
        ///// <param name="efEntities"></param>
        ///// <returns></returns>
        //int UpDateObj(params Fireasy.Data.Entity.IEntity[] efEntities);

    }
}
