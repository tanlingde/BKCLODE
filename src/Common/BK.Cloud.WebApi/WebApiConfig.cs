using BK.Cloud.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using Zephyr.Core;
using Zephyr.Web;
using System.Web.Http.Routing;
using System.IO;
using System.Web.Hosting;
using System.Threading;
using System.Web.Http.Cors;
using BK.Cloud.WebApi.webstack;

namespace BK.Cloud.WebApi
{
    public class MyWorkerRequest : SimpleWorkerRequest
    {
        private string localAdd = string.Empty;

        public MyWorkerRequest(string page, string query, TextWriter output, string address)
            : base(page, query, output)
        {
            this.localAdd = address;
        }

        public override string GetLocalAddress()
        {
            return this.localAdd;
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            GlobalConfiguration.Configuration.Filters.Add(new LoginCheckFiter());
            //config.Filters.Add(new AuthorizeAttribute());
            config.Filters.Add(new WebApiExceptionFilter());
            config.Filters.Add(new WebApiDisposeFilter());
            // Web API configuration and services
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API routes
            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute
            RouteTable.Routes.MapHttpRoute
            (
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional},
                constraints: null
            // handler: new DecompressionHandler(config) //添加压缩支持 ,
            );
            //.RouteHandler = new SessionControllerRouteHandler();

           // config.Routes.MapHttpRoute(
           //    name: "DefaultApi",
           //    routeTemplate: "api/{controller}/{action}/{id}",
           //    defaults: new { action = "{action}", id = RouteParameter.Optional },
           //   constraints: null,
           //    handler: new DecompressionHandler(config)
           //     //handler: new DecompressionHandler(config)//重点在这里
           //);

            config.Formatters.Remove(config.Formatters.XmlFormatter);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
            else
            {
                Thread.GetDomain().SetData(".appPath", "c:\\inetpub\\wwwroot\\webapp\\");
                Thread.GetDomain().SetData(".appVPath", "/");
                TextWriter tw = new StringWriter();
                String address = "localhost";
                HttpWorkerRequest wr = new MyWorkerRequest
                ("default.aspx", "friendId=1300000000", tw, address);
                HttpContext.Current = new HttpContext(wr);
            }

     


            //支持命名空间
            //config.Services.Replace(typeof(IHttpControllerSelector),
            //    new NamespaceHttpControllerSelector(config));

            //config.ParameterBindingRules.Insert(0, param =>
            //{
            //    if (param.ParameterType == typeof(RequestWrapper))
            //        return new RequestWrapperParameterBinding(param);

            //    return null;
            //});

            //config.Filters.Add(new AuthorizeAttribute());
            //config.Filters.Add(new WebApiExceptionFilter());
            //config.Filters.Add(new WebApiDisposeFilter());
        }
    }

    public class SessionRouteHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionRouteHandler(RouteData routeData)
            : base(routeData)
        {

        }
    }
    public class SessionControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionRouteHandler(requestContext.RouteData);
        }
    }
}





