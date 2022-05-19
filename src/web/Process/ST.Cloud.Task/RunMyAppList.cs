using Fireasy.Common.Logging;
using BK.Cloud.Model.Data.Model;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Reflection;
using BK.Cloud.Task.SubTasks;
namespace BK.Cloud.Task
{
    public class RunMyAppList
    {
        public List<MethodApp> Apps = new List<MethodApp>();

        [ThreadStatic]
        public static readonly RunMyAppList Instance = new RunMyAppList();

        Dictionary<string, Action<MethodApp>> Methods = new Dictionary<string, Action<MethodApp>>();

        public RunMyAppList()
        {
            var query = from t in Assembly.Load("BK.Cloud.Task").GetTypes()
                        where t.IsSubclassOf(typeof(BaseTask)) && t.Namespace.Equals("BK.Cloud.Task.SubTasks.Tasks", StringComparison.InvariantCultureIgnoreCase)
                        select Activator.CreateInstance(t) as BaseTask;
            query.ToList().ForEach(o =>
            {
                o.Init(Methods);
            });
            InitMethods();
        }

        private void InitMethods()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sttasks.xml"));
            XmlNode node = xmldoc.DocumentElement;
            string currenttask = node.Attributes["currenttask"].Value;
            List<string> currenttasks = new List<string>(currenttask.Split(','));

            foreach (XmlNode tasknode in node.ChildNodes)
            {
                if (tasknode is XmlComment)
                {
                    continue;
                }
                if (tasknode.Name.ToLower() == "commontask")
                {
                    AddNodeToJobs(tasknode,true);
                    continue;
                }
                if (tasknode.Attributes != null)
                {
                    string taskname = tasknode.Attributes["name"].Value;
                    if (currenttasks.Contains(taskname))
                    {
                        AddNodeToJobs(tasknode);
                    }
                }
            }
        }

        private void AddNodeToJobs(XmlNode tasknode, bool istongyong = false)
        {
            if (tasknode.Attributes != null)
            {
                string orgcode = tasknode.Attributes["orgcode"] == null ? "" : (tasknode.Attributes["orgcode"].Value ?? "");
                string cartype = tasknode.Attributes["cartype"] == null ? "" : (tasknode.Attributes["cartype"].Value ?? "");

                foreach (XmlNode ctasknode in tasknode.ChildNodes)
                {
                    if (ctasknode is XmlComment)
                    {
                        continue;
                    }
                    if (ctasknode.Attributes == null)
                    {
                        continue;
                    }
                    string appcode = ctasknode.Attributes["appcode"].Value;
                    if (!Methods.ContainsKey(appcode))
                        continue;
                    var findapp = new MethodApp()
                    {
                        AppCode = appcode,
                        AppName = (istongyong ? "[通用任务]" : "") + ctasknode.Attributes["appname"].Value,
                        TimeInterval = double.Parse(ctasknode.Attributes["interval"].Value),
                        Unit = "小时",
                        IsShow = ctasknode.Attributes["isshow"].Value == "1",
                        RunType = int.Parse(ctasknode.Attributes["runtype"].Value ?? "1"),
                        RealRunMethod = Methods[appcode]
                    };

                    if (!findapp.IsShow)
                        continue;

                    if (!string.IsNullOrEmpty(orgcode))
                    {
                        findapp.Params["orgcode"] = orgcode;
                        string orgids;
                        List<string> orgcodes = orgcode.Split(',').ToList();
                        using (DbContext db = new DbContext())
                        {
                            orgids = "";
                                //string.Join(",", db.TB_Orgs.Where(o => orgcodes.Contains(o.OrgCode)).Select(o => o.OrgID).ToList());
                        }
                        findapp.Params["orgid"] = orgids;
                    }
                    if (!string.IsNullOrEmpty(cartype))
                    {
                        findapp.Params["cartype"] = cartype;
                        string cartypeids;
                        var cartypelist = cartype.Split(',').ToList();
                        using (DbContext db = new DbContext())
                        {
                            var caryypelist =
                                db.TB_Dictionaries.Where(o => cartypelist.Contains(o.ExtendField1))
                                    .Select(o => o.DictionaryId)
                                    .ToList();
                            cartypeids = string.Join(",", caryypelist);
                        }
                        findapp.Params["cartypeid"] = cartypeids;
                    }

                    foreach (XmlAttribute attribute in ctasknode.Attributes)
                    {
                        //过滤关键字
                        if (attribute.Name != "orgcode" && attribute.Name != "orgid" && attribute.Name != "cartype" && attribute.Name != "cartypeid")
                            findapp.Params[attribute.Name] = attribute.Value;
                    }

                    Apps.Add(findapp);
                }
            }
        }

        public void StartAll()
        {
            Apps.ForEach(o => o.Start());
        }

        public void StopAll()
        {
            Apps.ForEach(o => o.Stop());
        }
  

        public void AttachAction(Action<string> msgs)
        {
            Apps.ForEach(o => o.ShowMsg = msgs);
        }
    }
}
