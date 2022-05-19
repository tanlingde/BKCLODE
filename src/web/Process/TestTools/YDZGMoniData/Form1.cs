using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using Newtonsoft.Json;
using License = System.ComponentModel.License;
using System.Threading;

namespace YDZGMoniData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.DataError += dataGridView1_DataError;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            txtPath.Text = AppDomain.CurrentDomain.BaseDirectory + "PLC统计表-远大住工.xls";
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                txtPath.Text = openFileDialog1.FileName;
            }
        }

        public string baseUrl = "";

        public double interval = 10;

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (btnRun.Text == "停止")
            {
                IsRun = false;
                btnRun.Text = "开始";
                return;
            }

            if (IsRun == false)
            {
                MessageBox.Show("正在停止中,请稍候..");
                return;
            }

            if (string.IsNullOrEmpty(txtUrl.Text) || !txtUrl.Text.ToLower().StartsWith("http://"))
            {
                txtUrl.Focus();
                MessageBox.Show("请先设置同步服务Url.必须以http://开头");
                return;
            }

            baseUrl = txtUrl.Text;
            if (!baseUrl.Trim().EndsWith("/"))
            {
                baseUrl += "/";
            }
            double res;
            if (!double.TryParse(txtInterval.Text, out res))
            {
                txtInterval.Focus();
                MessageBox.Show("时间间隔格式设置错误");
                return;

            }
            btnRun.Text = "停止";

            interval = res;

            BuildData();
        }

        private DataTable senDataTable;
        /// <summary>
        /// 主要实现Aspose.cells中的Excel格式化输出定制
        /// </summary>
        /// <returns></returns>
        protected void BuildData()
        {
            senDataTable = new DataTable();
            senDataTable.Columns.Add("设备代码");
            senDataTable.Columns.Add("设备名");
            senDataTable.Columns.Add("日期");
            senDataTable.Columns.Add("发送数据");
            senDataTable.PrimaryKey = new[] { senDataTable.Columns[0] };


            //#region Aspose.Cell引用

            //Aspose.Cells.License licExcel = new Aspose.Cells.License();  //Aspose.Cells申明


            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/cellLic.lic"))

            //    licExcel.SetLicense(AppDomain.CurrentDomain.BaseDirectory + "/cellLic.lic");

            //#endregion

            Workbook workbook = new Workbook(txtPath.Text);
            //workbook.Shared = true;
            //workbook.Open(txtPath.Text);

            Dictionary<string, Dictionary<string, string>> bldic = new Dictionary<string, Dictionary<string, string>>();
            int index = 0;
            for (var i = 1; i <= 5; i++)
            {
                Worksheet buliaoji = workbook.Worksheets[index++];
                SetTableToDic(buliaoji, bldic, string.Format("YDBLJ_BLJ{0}|{0}#布料机", i));
            }
            for (var i = 1; i <= 5; i++)
            {
                Worksheet buliaoji = workbook.Worksheets[index++];
                SetTableToDic(buliaoji, bldic, string.Format("YDYHY_YHY{0}|{0}#养护窑", i));
            }
            for (var i = 1; i <= 5; i++)
            {
                Worksheet buliaoji = workbook.Worksheets[index++];
                SetTableToDic(buliaoji, bldic, string.Format("YDSSX_SSX{0}|{0}#钢轨轮输送线", i));
            }

            for (var i = 1; i <= 5; i++)
            {
                Worksheet buliaoji = workbook.Worksheets[index++];
                SetTableToDic(buliaoji, bldic, string.Format("YDYSC_YSC{0}|{0}#综合运输车", i));
            }

            Worksheet buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDSLXT_SLXT|送料系统");


            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDQDJ_QDJ1|1#数控钢筋调直切断机");


            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDQDJ_QDJ2|调直切断机2");


            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDWHJ_WHJ1|网焊机");

            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDWGJ_WGJ1|弯箍机");

            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDJQJ_JQJ1|剪切机");

            buliaoji1 = workbook.Worksheets[index++];

            SetTableToDic(buliaoji1, bldic, "YDXMW_XMW1|斜面弯(弯曲机)");


            RunTask(bldic);
        }


        public bool IsRun = true;
        public void RunTask(Dictionary<string, Dictionary<string, string>> bldic)
        {
            Task.Run(() =>
            {
                while (IsRun)
                {
                    try
                    {
                        RunBuildData(bldic);
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            this.Text = ex.Message;
                        }));
                        // this.Text = ex.Message;
                    }
                    finally
                    {
                        dataGridView1.BeginInvoke(new Action(() =>
                        {
                            dataGridView1.DataSource = senDataTable;
                            dataGridView1.Columns[0].Width = 100;
                            dataGridView1.Columns[1].Width = 120;
                            dataGridView1.Columns[2].Width = 120;
                            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                        }));

                    }
                    Thread.Sleep(TimeSpan.FromSeconds(interval));
                }
                IsRun = true;
            });
        }

        public static int randbit(int i, int j, int p)
        {
            int a;
            Random ra = new Random(Guid.NewGuid().GetHashCode() + p);
            a = ra.Next(i, j);
            return a;
        }

        public static int randbit(int i, int j)
        {
            int a;
            Random ra = new Random(Guid.NewGuid().GetHashCode());
            a = ra.Next(i, j);
            return a;
        }

        public void RunBuildData(Dictionary<string, Dictionary<string, string>> bldic)
        {
            var httpClient = new HttpClient();
            string data1 = "";
            foreach (var dickey in bldic.Keys)
            {
                var dks = dickey.Split('|');
                string devcode = dks[0];
                string devname = dks[1];
                string data2 = "";
                foreach (var onedata in bldic[dickey].Keys)
                {

                    // data+=onedata+"|"+
                    // var vals = onedata.Split('|');
                    string varname = onedata;
                    string value = bldic[dickey][onedata];

                    if (varname.EndsWith("CZS") || varname.EndsWith("ZS") || varname.EndsWith("CK") || varname.EndsWith("JK") || varname.EndsWith("DCZXZ") || varname.EndsWith("DCYXZ"))
                    {
                        continue;
                    }

                    Random ran = new Random(Guid.NewGuid().GetHashCode());

                    if (value.Contains("VD") || value.Contains("VW") || value.StartsWith("D") || value.StartsWith("C") || value.StartsWith("ML")
                        || (value.StartsWith("M") && value.Contains("至")))
                    {
                        data1 += varname + "|" + Math.Round(ran.NextDouble() * 1000, 2) + ",";
                        data2 += varname + "|" + Math.Round(ran.NextDouble() * 1000, 2) + ",";
                    }
                    else
                    {
                        data1 += varname + "|" + ran.Next(0, 2) + ",";
                        data2 += varname + "|" + ran.Next(0, 2) + ",";
                        //bldic[devcode] += row[1].ToString() + "|" + (new Random()).Next(0, 1).ToString() + ",";
                    }
                }
                senDataTable.LoadDataRow(new object[]
                {
                    devcode,devname,DateTime.Now,data2
                }, true);
            }
            int dongzuo =new Random(Guid.NewGuid().GetHashCode()).Next(0, 2);

            data1 += string.Format("YHY{5}#_{0}CZS|true,YHY{5}#_DCWZ{1}ZS|true,YHY{5}#_DC{2}|true,YHY{5}#_JK|{3},YHY{5}#_CK|{4}",
                new Random(Guid.NewGuid().GetHashCode()).Next(1, 11), new Random(Guid.NewGuid().GetHashCode()).Next(1, 8), new Random(Guid.NewGuid().GetHashCode()).Next(1, 3) == 1 ? "ZXZ" : "YXZ", dongzuo == 1 ? "true" : "false", dongzuo == 1 ? "false" : "true", new Random(Guid.NewGuid().GetHashCode()).Next(1, 6));

            var requestJson = JsonConvert.SerializeObject(new
            {
                //equipmentCode = devcode,
                //equipmentName = devname,
                data = data1
            });
            HttpContent httpContent = new StringContent(requestJson);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpClient.PostAsync(baseUrl + "API/YDZGSync/WriteData", httpContent);

        }


        private static void SetTableToDic(Worksheet buliaoji, Dictionary<string, Dictionary<string, string>> bldic, string devcode)
        {

            var buliaojitb = buliaoji.Cells.ExportDataTableAsString(1, 0, buliaoji.Cells.MaxDataRow + 1,
                buliaoji.Cells.MaxDataColumn + 1, true);

            bldic[devcode] = new Dictionary<string, string>();
            foreach (DataRow row in buliaojitb.Rows)
            {
                if (row[9].ToString() == "")
                    continue;
                string value = row[6].ToString().Trim();

                bldic[devcode][row[9].ToString()] = value;

                //if (value.Contains("VD") || value.Contains("VW"))
                //{
                //    bldic[devcode][row[1].ToString()] = ((new Random()).NextDouble()*1000).ToString();
                //    // bldic[devcode] += row[1].ToString() + "|" + ((new Random()).NextDouble()*1000).ToString() + ",";
                //}
                //else
                //{
                //    bldic[devcode][row[1].ToString()] =(new Random()).Next(0, 1).ToString();
                //    //bldic[devcode] += row[1].ToString() + "|" + (new Random()).Next(0, 1).ToString() + ",";
                //}
            }
        }

        public static System.Data.DataTable ReadExcel(Worksheet sheet)
        {
            Cells cells = sheet.Cells;
            return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
        }

        private object celltext = "";
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.ColumnIndex > -1 && e.RowIndex > -1)  //点击的是鼠标右键，并且不是表头
            {
                //右键选中单元格
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                celltext = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                this.contextMenuStrip1.Show(MousePosition.X, MousePosition.Y); //MousePosition.X, MousePosition.Y 是为了让菜单在所选行的位置显示

            }

        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, celltext);
        }

    }
}
