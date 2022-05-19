using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace BK.Cloud.Logic.InnerObj
{
    public class ProvideHelper
    {
        public static IContainer BuildProvide(ContainerBuilder build, IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> builder)
        {
            builder
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
            //.InstancePerDependency();
            var buContainer1 = build.Build();
            return buContainer1;
        }
    }
}
