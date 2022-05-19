using BK.Cloud.Facade;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace BaseWeb
{
    public class BaseDuoBi : Page
    {
        public static string baseUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                {
                    _baseUrl = System.Configuration.ConfigurationManager.AppSettings["webapi"];
                }
                return _baseUrl;
            }
        }
        private static string _baseUrl = null;
        //"http://localhost:10000";

        public string loginurl
        {
            get
            {
                if (string.IsNullOrEmpty(_loginurl))
                {
                    _loginurl = AppPath + System.Configuration.ConfigurationManager.AppSettings["loginurl"];
                }
                return _loginurl;
            }
        }
        private string _loginurl = null;

        public static string LoadMyScript(string url)
        {
            return "<script src=\"" + baseUrl + url + "\" type=\"text/javascript\"></script>";
        }

        public static string LoadMyCSS(string url)
        {
            return "<link href=\"" + baseUrl + url + "\" rel=\"stylesheet\" />";
        }

        private string _currentThemes = "default";
        public string StatusEnums
        {
            get
            {
                //var assemblyBuilder = new Fireasy.Common.Emit.DynamicAssemblyBuilder("BK.Cloud.Model");

                //assemblyBuilder.AssemblyBuilder.GetType()
                string script = "<script type='text/javascript'>$.cloud.enums={};";
                var type = typeof(BK.Cloud.Model.Customer.Enums);
                var subtypes = type.GetNestedTypes();
                foreach (var subtype in subtypes)
                {
                    string propertynames = "";
                    foreach (var property in subtype.GetFields())
                    {
                        propertynames += string.Format("\"{0}\":{1},", property.Name, property.GetValue(subtype));
                    }
                    propertynames = propertynames.Remove(propertynames.Length - 1);
                    script += string.Format("$.cloud.enums.{0}={{{1}}};", subtype.Name, propertynames);
                }
                script += "</script>";
                return script;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            //if (FacadeProvide.FacadeAuthor.LoginInfo == null &&
            //     Request.Url.ToString().ToLower() != ("http://" + Request.Url.Host + ":" + Request.Url.Port + "/Web/Default/login.aspx").ToLower()
            //      &&
            //     Request.Url.ToString().ToLower() != ("http://" + Request.Url.Host + ":" + Request.Url.Port + "/Web/Default/AdminLogin.aspx").ToLower())
            //{
            //    string url = Session["loginurl"] as string;
            //    Response.Redirect(url ?? "/Web/Default/login.aspx");
            //}
            LoadScriptDebug();
        }

        public string AppPath
        {
            get
            {
                string path = Request.Path;
                int stindex = path.IndexOf('/') + 1;
                path = "/" + path.Substring(stindex, path.IndexOf('/', stindex) - stindex);
                return path;
            }
        }

        //[Conditional( "DEBUG")] 
        private void LoadScriptDebug()
        {
            string apppath = AppPath;
            Response.Write(string.Format(@"              
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,Chrome=1"">
                <!--[if lt IE 9]>
                   <script src=""http://apps.bdimg.com/libs/html5shiv/3.7/html5shiv.min.js""></script>
                   <script src=""http://apps.bdimg.com/libs/respond.js/1.4.2/respond.min.js""></script>
                 <![endif]-->
                <link href='" + baseUrl + @"/Content/Themes/{0}/easyui.css' rel='stylesheet' type='text/css' />
                <script src='" + baseUrl + @"/Scripts/ReferencedScripts/jquery-1.11.3.min.js' type='text/javascript'></script>
                <script src='" + baseUrl + @"/Scripts/ReferencedScripts/jquery.easyui-1.4.3.js' type='text/javascript'></script>
                <script src='" + baseUrl + @"/Scripts/ReferencedScripts/locale/easyui-lang-zh_CN.js' type='text/javascript'></script>   
                <script src='" + baseUrl + @"/Scripts/WebScripts/javascript_develop.js' type='text/javascript'></script>      
                <script src='" + baseUrl + @"/Scripts/WebScripts/jquery_develop.js' type='text/javascript'></script> 
                <script src='" + baseUrl + @"/Scripts/WebScripts/easyui_develop.js' type='text/javascript'></script> 
                <script src='" + baseUrl + @"/Scripts/WebScripts/st_common.js' type='text/javascript'></script> 
                <link href='" + baseUrl + @"/Content/Themes/icon.css' rel='stylesheet' type='text/css' />
                <link href='" + baseUrl + @"/Content/Themes/color.css' rel='stylesheet' type='text/css' />     
            ", CurrentThemes));

            /*加载组件*/
            Response.Write(string.Format(@"
			<link href='" + apppath + @"/Content/global.css' rel='stylesheet' type='text/css' />
            <link href='" + baseUrl + @"/Content/themesextend/{0}/st_formgrid/st_formgrid.css' rel='stylesheet' type='text/css' />
            <script src='" + baseUrl + @"/ST_Component/st_formgrid/st_formgrid.js' type='text/javascript'></script>
            <link href='" + baseUrl + @"/Content/themesextend/{0}/st_devicetree/st_devicetree.css' rel='stylesheet' type='text/css' />
            <script src='" + baseUrl + @"/ST_Component/st_devicetree/st_devicetree.js' type='text/javascript'></script>
            <link href='" + baseUrl + @"/Content/themesextend/{0}/st_treegrid/st_treegrid.css' rel='stylesheet' type='text/css' />
            <script src='" + baseUrl + @"/ST_Component/st_treegrid/st_treegrid.js' type='text/javascript'></script>
            <link href='" + baseUrl + @"/Content/themesextend/{0}/st_orgtree/st_orgtree.css' rel='stylesheet' type='text/css' />
            <script src='" + baseUrl + @"/ST_Component/st_orgtree/st_orgtree.js' type='text/javascript'></script>
			<link href='" + baseUrl + @"/Content/Themes/font-awesome/css/font-awesome.min.css' rel='stylesheet' type='text/css' />
            ", CurrentThemes));

            /*加载用户信息*/
            //            Response.Write(@"
            //			 <script type='text/javascript'>
            //                $.cloud.user=" + Newtonsoft.Json.JsonConvert.SerializeObject(FacadeProvide.FacadeAuthor.LoginInfo) + @";
            //            </script>
            //            ");
            Response.Write(@"
			 <script type='text/javascript'>
                $.cloud.loginurl='" + loginurl + @"';
                $.cloud.projectname='" + apppath + @"';
                $.cloud.baseUrl='" + baseUrl + @"';
</script>
            ");
                //$.ajax({
                //    url: '" + baseUrl + @"/User/GetCurrentUserInf" + @"',
                //    async: false,
                //    success: function(data) {
                //        data = $.cloud.strtojson(data);
                //        if (data.success) {
                //            $.cloud.user=data.data;
                //        }
                //        else{
                //            if(location.href.indexOf($.cloud.loginurl)<0){
                //                  location.href = $.cloud.loginurl;
                //            }
                //        }
                //    }
                //});
            
            Response.Write(StatusEnums);
        }

        [Conditional("RELEASE")]
        private void LoadScript()
        {
            Response.Write(string.Format(@"
                <link href='" + baseUrl + @"/Content/themes/{0}/easyui.css' rel='stylesheet' type='text/css' />
                <link href='" + baseUrl + @"/Content/themesextend/{0}/cusall.css' rel='stylesheet' type='text/css' />
                <link href='" + baseUrl + @"/Content/themes/icon.css' rel='stylesheet' type='text/css' />
                <link href='" + baseUrl + @"/Content/themes/color.css' rel='stylesheet' type='text/css' />
                <script src='" + baseUrl + @"/Scripts/jquery-1.11.3.min.js' type='text/javascript'></script>
                <script src='" + baseUrl + @"/Scripts/jquery.easyui-1.4.3.min.js' type='text/javascript'></script>
                <script src='" + baseUrl + @"/Scripts/locale/easyui-lang-zh_CN.js' type='text/javascript'></script>   
                <script src='" + baseUrl + @"/Scripts/WebScripts/javascript_develop.js' type='text/javascript'></script>      
                <script src='" + baseUrl + @"/Scripts/WebScripts/jquery_develop.js' type='text/javascript'></script> 
                <script src='" + baseUrl + @"/Scripts/WebScripts/easyui_develop.js' type='text/javascript'></script> 
                <script src='" + baseUrl + @"/Scripts/WebScripts/st_common.js' type='text/javascript'></script> 
                <link href='" + baseUrl + @"/Content/themesextend/{0}/cusall.css' type='text/css' />  
            ", CurrentThemes));
        }


        /// <summary>
        /// 当前主题名称
        /// </summary>
        public string CurrentThemes
        {
            get { return _currentThemes; }
            set { _currentThemes = value; }
        }

        public static string CurrentPageName
        {
            get
            {
                string currentFilePath = HttpContext.Current.Request.FilePath;
                currentFilePath = currentFilePath.Substring(currentFilePath.LastIndexOf("/", System.StringComparison.Ordinal) + 1);
                currentFilePath = currentFilePath.Replace(".aspx", "");
                return currentFilePath;
            }
        }

        public static string UserScript
        {
            get
            {
                if (true)
                {
                    return "<script type='text/javascript'>if(!$.cloud.userinfo){$.cloud.userinfo =" + "" +
                           ";}</script>";
                }
            }
        }
    }
}
