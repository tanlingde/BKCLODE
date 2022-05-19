using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using BK.Cloud.Logic;


namespace BK.Cloud.Task
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

           // new BaseTask().TestCompMaxSubMin();

            NameValueCollection collection = ParseArguments(args);
            if (collection["debug"] != null)
            {
               
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                STTaskService service = new STTaskService();
                System.ServiceProcess.ServiceBase[] servicesToRunTemp = { service };
                System.ServiceProcess.ServiceBase.Run(servicesToRunTemp);
            }
        }

        public static NameValueCollection ParseArguments(string[] args)
        {
            var arguments = new NameValueCollection();
            foreach (string a in args)
            {
                int index = a.IndexOf(":", StringComparison.Ordinal);
                if (index == -1)
                {
                    arguments.Add("configfile".ToLowerInvariant(), a);
                    continue;
                }

                string argName = a.Substring(1, index - 1);
                string argValue = a.Substring(index + 1);

                arguments.Add(argName.ToLowerInvariant(), argValue);
            }

            return arguments;
        }
    }
}



