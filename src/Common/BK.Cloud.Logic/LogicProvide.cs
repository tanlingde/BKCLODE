using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using Fireasy.Common.Extensions;
using BK.Cloud.Logic.InnerObj;
using Module = Autofac.Module;
using BK.Cloud.Tools;
using BK.Cloud.Logic.Interface;

namespace BK.Cloud.Logic
{
    public class LogicProvide
    {
        #region 初始化

        private static IContainer buContainer;
        static readonly ContainerBuilder builder = new ContainerBuilder();

        static LogicProvide()
        {
            /*
             public class TestInterceptor : IInterceptor
            {
             
            }
             */
            //1：将[Intercept(typeof(TestInterceptor))]特性加在public interface ILogger定义中。
            //builder
            //    .RegisterType<DefaultLogger>()
            //    .EnableInterfaceInterceptors()
            //    .InterceptedBy(typeof(TestInterceptor))
            //    .As<ILogger>();
            //builder.RegisterType<TestInterceptor>();

            //builder.RegisterModule(new AutoPropertyModule());

            // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //     .AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemBuilder = builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            // builder.RegisterType<LogicCommond>().Named<ILogicCommond>("LogicCommond");
            //builder.RegisterType<LogicCommandCan>().Named<ILogicCommond>("LogicCommandCan");
            //builder.RegisterType<LogicCmd>().Named<ILogicCommond>("LogicCommond");
            buContainer = ProvideHelper.BuildProvide(builder, assemBuilder);

        }



        #endregion


        #region 通用模块
        /// <summary>
        /// 通用模块
        /// </summary>
        public static ILogicGeneric LogicGeneric
        {
            get
            {
                return buContainer.Resolve<ILogicGeneric>();
            }
        }
        #endregion

        /// <summary>
        /// 日志模块
        /// </summary>
        public static ILogicLog LogicLog
        {
            get
            {
                return buContainer.Resolve<ILogicLog>();
            }
        }
        /// <summary>
        /// 调试日志
        /// </summary>
        public static ILogicDebug LogicDebug
        {
            get
            {
                return buContainer.Resolve<ILogicDebug>();
            }
        }
        /// <summary>
        /// 长坯库库存与来料生产计划
        /// </summary>
        public static ILogicLongInventory LogicLongInventory
        {
            get
            {
                return buContainer.Resolve<ILogicLongInventory>();
            }
        }
        /// <summary>
        /// 短坯库库存
        /// </summary>
        public static ILogicShortInventory LogicShortInventory
        {
            get
            {
                return buContainer.Resolve<ILogicShortInventory>();
            }
        }
        /// <summary>
        /// 火切记录
        /// </summary>
        public static ILogicFireCut LogicFireCut
        {
            get
            {
                return buContainer.Resolve<ILogicFireCut>();
            }
        }
        /// <summary>
        /// 锯切记录
        /// </summary>
        public static ILogicSawCutting LogicSawCutting
        {
            get
            {
                return buContainer.Resolve<ILogicSawCutting>();
            }
        }
        /// <summary>
        /// 跟踪队列redis
        /// </summary>
        public static IRedisManage RedisManage
        {
            get
            {
                return buContainer.Resolve<IRedisManage>();
            }
        }
    }

    #region 附加特性
    public class AutoPropertyModule : Module
    {
        private readonly ConcurrentDictionary<string, object> _loggerCache;

        public AutoPropertyModule()
        {
            _loggerCache = new ConcurrentDictionary<string, object>();
        }

        protected override void Load(ContainerBuilder moduleBuilder)
        {
            // by default, use Coevery's logger that delegates to Castle's logger factory
            //moduleBuilder.RegisterType<Test>().As<Test>().InstancePerLifetimeScope();

            moduleBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            // call CreateLogger in response to the request for an ILogger implementation
            // moduleBuilder.Register(CreateLogger).As<ILogging.ILogger>().InstancePerDependency();

        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            var implementationType = registration.Activator.LimitType;

            // build an array of actions on this type to assign loggers to member properties
            var injectors = BuildLoggerInjectors(implementationType).ToArray();

            // if there are no logger properties, there's no reason to hook the activated event
            if (!injectors.Any())
                return;

            // otherwise, whan an instance of this component is activated, inject the loggers on the instance
            registration.Activated += (s, e) =>
            {
                foreach (var injector in injectors)
                    injector(e.Context, e.Instance);
            };
        }

        private IEnumerable<Action<IComponentContext, object>> BuildLoggerInjectors(Type componentType)
        {
            // Look for settable properties of type "ILogger" 
            var loggerProperties = componentType
                .GetProperties(BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    PropertyInfo = p,
                    p.PropertyType,
                    IndexParameters = p.GetIndexParameters(),
                    Accessors = p.GetAccessors(false)
                })
                // .Where(x => x.PropertyType == typeof (Test)) // must be a logger
                .Where(x => x.IndexParameters.Count() == 0) // must not be an indexer
                .Where(x => x.Accessors.Length != 1 || x.Accessors[0].ReturnType == typeof(void));
            //must have get/set, or only set

            // Return an array of actions that resolve a logger and assign the property
            foreach (var entry in loggerProperties)
            {
                var propertyInfo = entry.PropertyInfo;

                yield return (ctx, instance) =>
                {
                    string component = componentType.ToString();
                    //var logger = _loggerCache.GetOrAdd(component,
                    //    key => ctx.Resolve<Test>(new TypedParameter(typeof (Type), componentType)));
                    var resolvtype = ctx.Resolve(propertyInfo.PropertyType);
                    var logger = _loggerCache.GetOrAdd(component, key => resolvtype);
                    propertyInfo.SetValue(instance, logger, null);
                };
            }

        }
    }
    #endregion
}
