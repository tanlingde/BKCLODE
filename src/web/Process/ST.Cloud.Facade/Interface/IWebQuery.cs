using BK.Cloud.Model.Customer;
using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Facade.Interface
{
    /// <summary>
    /// 通用查询对外接口
    /// </summary>
    public interface IWebQuery
    {



        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        DataTable QueryAllData(Condition condition);

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页数据量</param>
        /// <param name="viewName">数据实体名</param>
        /// <param name="jsonCondition">查询条件</param>
        /// <returns></returns>
        PageResult QueryPageData(Condition condition);


        /// <summary>
        /// 插入数据实体
        /// </summary>
        /// <param name="objName">对象名称</param>
        /// <param name="jsonData">要插入的对象实体集合</param>
        /// <returns>1，成功。0.失败</returns>
        UpMsg InsertObj(Condition condition);



        /// <summary>
        /// 更新数据实体
        /// </summary>
        /// <param name="objName">对象名称</param>
        /// <param name="jsonData">要更新的对象实体集合</param>
        /// <returns>1，成功。0.失败</returns>
        UpMsg UpDateObj(Condition condition);


        /// <summary>
        /// 删除数据实体
        /// </summary>
        /// <param name="objName">对象名称</param>
        /// <param name="jsonCondition">删除条件，以不为空数据作为删除条件</param>
        /// <returns>1，成功。0.失败</returns>
        UpMsg DelObj(Condition condition);

        /// <summary>
        /// 增加记录时，前端调用获取唯一键值
        /// </summary>
        /// <returns></returns>
        long GetNewID();


    }
}
