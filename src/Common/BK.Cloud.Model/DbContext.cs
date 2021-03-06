// ------------------------------
// 本模块由CodeBuilder工具生成,该类适用于 Fireasy.Data.Entity 1.5 数据框架
// 版权所有 (C) Fireasy 2014

// 项目名称: BK.Cloud.Model
// 模块名称: 数据上下文
// 代码编写: 谭凌德
// 文件路径: $FilePath$
// 创建时间: 2022/5/15 10:33:40
// ------------------------------

#region NAMESPACE
using System;
using Fireasy.Data.Entity;
using BK.Cloud.Model.Data.Model;
#endregion NAMESPACE

namespace BK.Cloud.Model.Data.Model
{
    public class DbContext : EntityContext
    {	
        public DbContext()
            : base(null)
        {

        }
        public DbContext(string dbname)
            : base(dbname)
        {

        }

        /// <summary>
        /// 获取或设置 系统资源字典表 实体仓储。
        /// </summary> 
        public EntityRepository<TB_Dictionary> TB_Dictionaries { get; set; }


        /// <summary>
        /// 获取或设置 调试日志表 实体仓储。
        /// </summary> 
        public EntityRepository<TB_DebugLog> TB_DebugLogs { get; set; }


        /// <summary>
        /// 车辆对照实体仓储。
        /// </summary> 
        public EntityRepository<TB_CarDictionary> TB_CarDictionary { get; set; }

    }
}