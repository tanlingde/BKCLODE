using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Newtonsoft.Json;
using BK.Cloud.Model.Customer;

namespace Zephyr.Web
{
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //ILog log = LogManager.GetLogger(HttpContext.Current.Request.Url.LocalPath);
            //log.Error(context.Exception);

            var message = context.Exception.Message;
            if (context.Exception.InnerException != null)
                message = context.Exception.InnerException.Message;

            Fireasy.Common.Logging.DefaultLogger.Instance.Error("web异常", context.Exception);

            context.Response = new HttpResponseMessage()
            {
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new UpMsg(message, false)),
                Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.OK
            };
           
            //context.Response.Content =new JsonResult<UpMsg>(new UpMsg(message,false),new JsonSerializerSettings(){},Encoding.UTF8,context.Request);

            base.OnException(context);
        }
    }
}