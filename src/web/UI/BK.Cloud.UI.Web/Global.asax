<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="BK.Cloud.WebApi" %>

<script runat="server">

   
    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Cache.SetNoStore();
        //JobPlans.Instance.CheckJobs();
        // HttpContext.Current.Response.Write("");
        //BundleHelper.Config();
        // HttpConfiguration.Default.ServiceFactory = new AspPageServiceFactory(); 
    }

    protected void Application_EndRequest(object sender, EventArgs e)
    {

    }

    void Application_Start(object sender, EventArgs e)
    {
        // 在应用程序启动时运行的代码
        GlobalConfiguration.Configure(WebApiConfig.Register);
    }
    public override void Init()
    {
        PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
        base.Init();
    }

    //添加session支持
    void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
    {
        HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
    }

    void Application_End(object sender, EventArgs e)
    {
        //  在应用程序关闭时运行的代码

    }

    void Application_Error(object sender, EventArgs e)
    {
        // 在出现未处理的错误时运行的代码

    }

    void Session_Start(object sender, EventArgs e)
    {
        // 在新会话启动时运行的代码

    }



    void Session_End(object sender, EventArgs e)
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
        // 或 SQLServer，则不会引发该事件。
        //var userList = HttpContext.Current.Application["UserInfo"] as List<LoginInfo>;
        //var logininfo = HttpContext.Current.Session["LoginInfo"] as LoginInfo;
        //if (logininfo != null && userList != null)
        //{
        //    var findindex = userList.FindIndex(o => o.GUID == logininfo.GUID);
        //    if (findindex != -1)
        //    {
        //        HttpContext.Current.Application.Lock();
        //        userList.RemoveAt(findindex);
        //        HttpContext.Current.Application.UnLock();
        //    }
        //}
    }
       
</script>
