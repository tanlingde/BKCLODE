using Autofac;
using Fireasy.Common.Ioc;
using BK.Cloud.Facade.Interface;
using BK.Cloud.Model.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac.Builder;
using Autofac.Features.Scanning;
using BK.Cloud.Logic.InnerObj;

namespace BK.Cloud.Facade
{
    public class FacadeProvide
    {
        static readonly ContainerBuilder builder = new ContainerBuilder();
        private static IContainer buContainer;
        static FacadeProvide()
        {

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemBuilder = builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            buContainer = ProvideHelper.BuildProvide(builder, assemBuilder);

        }

        /// <summary>
        /// 获取通用查询功能块
        /// </summary>
        public static IWebQuery WebQuery
        {
            get
            {
                return buContainer.Resolve<IWebQuery>();
            }
        }
        /// <summary>
        /// 调试模块
        /// </summary>
        public static IFacadeDebug FacadeDebug
        {
            get
            {
                return buContainer.Resolve<IFacadeDebug>();
            }
        }
        /// <summary>
        /// 长坯库模块
        /// </summary>
        public static IFacadeLongInventory FacadeLongInventory
        {
            get
            {
                return buContainer.Resolve<IFacadeLongInventory>();
            }
        }
        /// <summary>
        /// 短坯库模块
        /// </summary>
        public static IFacadeShortInventory FacadeShortInventory
        {
            get
            {
                return buContainer.Resolve<IFacadeShortInventory>();
            }
        }

    }
}
