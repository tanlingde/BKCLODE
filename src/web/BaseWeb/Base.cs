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
    public class Base : Page
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
			<link href='" + baseUrl + @"/Content/Themes/bootstrap/css/font-awesome.min.css' rel='stylesheet' type='text/css' />
            ", "default"));

            /*加载用户信息*/
            //            Response.Write(@"
            //			 <script type='text/javascript'>
            //                $.cloud.user=" + Newtonsoft.Json.JsonConvert.SerializeObject(FacadeProvide.FacadeAuthor.LoginInfo) + @";
            //            </script>
            //            ");
            Response.Write(@"
			 <script type='text/javascript'>
                $.cloud.loginurl='" + loginurl.ToLower() + @"';
                $.cloud.projectname='" + apppath + @"';
                $.cloud.baseUrl='" + baseUrl + @"';
                $.ajax({
                    url: '" + baseUrl + @"/User/GetCurrentUserInf" + @"',
                    async: false,
                    success: function(data) {
                        data = $.cloud.strtojson(data);
                        if (data.success) {
                            $.cloud.user=data.data;
                        }
                        else{
                            if(location.href.toLowerCase().indexOf($.cloud.loginurl)<0){
                                  location.href = $.cloud.loginurl;
                            }
                        }
                    }
                });
            </script>
            ");
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
            get
            {
                return (HttpContext.Current.Session["_currentThemes"] as string) ?? _currentThemes;
            }
            set
            {
                HttpContext.Current.Session["_currentThemes"] = _currentThemes = value;
                //_currentThemes = value;
            }
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

        //在C#后台实现JavaScript的函数escape()的字符串转换  
        //些方法支持汉字  
        public string escape(string s)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byteArr = System.Text.Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < byteArr.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(byteArr[i + 1].ToString("X2"));//把字节转换为十六进制的字符串表现形式  

                sb.Append(byteArr[i].ToString("X2"));
            }
            return sb.ToString();
        }
        //把JavaScript的escape()转换过去的字符串解释回来  
        //些方法支持汉字  
        public string unescape(string s)
        {

            string str = s.Remove(0, 2);//删除最前面两个＂%u＂  
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔  
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节  
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr);　//把字节转为unicode编码  
            return str;
        }
    }
}
