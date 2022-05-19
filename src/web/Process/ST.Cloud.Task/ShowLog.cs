using System;
using System.Windows.Forms;

namespace BK.Cloud.Task
{
    public partial class ShowLog : Form
    {
        public static readonly ShowLog Instance = new ShowLog();

        public ShowLog()
        {
            InitializeComponent();
            WriteLog("报表服务状态为已停止");
        }

        public void WriteLog(string msg)
        {
            try
            {
                if (textBox1.InvokeRequired)
                {
                    textBox1.BeginInvoke(new Action(() => textBox1.AppendText(msg + Environment.NewLine)));
                }
                else
                {
                    textBox1.AppendText(msg + Environment.NewLine);
                }
            }
            catch(Exception ex1)
            {
                throw ex1;
            }
        }

        private void ShowLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
