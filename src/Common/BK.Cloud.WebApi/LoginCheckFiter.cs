
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Threading;
using System.Security.Principal;
using System.Text;
using BK.Cloud.Facade;
namespace BK.Cloud.Web.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class LoginCheckFiter : AuthorizationFilterAttribute
    {
        public static bool CheckUser(string uid, string pwd)
        {
            //  var message = FacadeProvide.FacadeAuthor.Login(uid, pwd);
            //  return message.success;
            return true;
        }
        bool Active = true;

        public LoginCheckFiter()
        { }

        /// <summary>  
        /// Overriden constructor to allow explicit disabling of this  
        /// filter's behavior. Pass false to disable (same as no filter  
        /// but declarative)  
        /// </summary>  
        /// <param name="active"></param>  
        public LoginCheckFiter(bool active)
        {
            Active = active;
        }

        /// <summary>  
        /// Override to Web API filter method to handle Basic Auth check  
        /// </summary>  
        /// <param name="actionContext"></param>  
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Active)
            {
                //判断是否浏览器自动录入账号名和密码判断.
                var identity = ParseAuthorizationHeader(actionContext);
                if (identity != null)
                {
                    //用户登录不成功,再次校验，直到成功为止.
                    if (!CheckUser(identity.Name, identity.Password))
                    {
                        Challenge(actionContext);
                        return;
                    }
                  //  HttpContext.Current.Session["uid"] = identity.Name;
                }

                //判断session当前用户是否登录.
                if (HttpContext.Current.Session["LoginInfo"] == null)
                {
                    Challenge(actionContext);
                    return;
                }

                base.OnAuthorization(actionContext);
            }
        }

        /// <summary>  
        /// Send the Authentication Challenge request  
        /// </summary>  
        /// <param name="message"></param>  
        /// <param name="actionContext"></param>  
        void Challenge(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }

        /// <summary>  
        /// Parses the Authorization header and creates user credentials  
        /// </summary>  
        /// <param name="actionContext"></param>  
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);

        }
    }

    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.Password = password;
        }

        /// <summary>  
        /// Basic Auth Password for custom authentication  
        /// </summary>  
        public string Password { get; set; }
    }
}