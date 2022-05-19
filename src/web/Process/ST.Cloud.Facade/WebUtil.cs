using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Reflection;
using Aspose.Cells;
using System.IO;
using BK.Cloud.Model.Customer;
using System.Collections;

namespace BK.Cloud.Facade
{
    public static class WebUtil
    {

        /// <summary>
        /// 生成指定位数随机数字和字符串
        /// </summary>
        /// <param name="count">位数</param>
        /// <returns>随机数字和字符串</returns>
        public static string getString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串   

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生count位校验码   
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }

        public static string ArrayObjToStr(object obj)
        {

            if (!(obj is IEnumerable))
            {
                return "[" + Newtonsoft.Json.JsonConvert.SerializeObject(obj) + "]";
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string QueryObjToStr(object obj)
        {
            //if (obj is IEnumerable)
            //{
            //    throw new Exception("查询集不支持迭代");
            //}
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static string Wrap(string jsonstr)
        {

            if (HttpContext.Current.Request["jsonp"] == "mobile")
            {
                string callback = HttpContext.Current.Request.Params["callback"];
                return callback + "(" + jsonstr + ")";
            }
            return jsonstr;
        }

        public static void SetWebContext()
        {
            var context = HttpContext.Current;
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetNoStore();
            //不让浏览器缓存
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
        }

        /// <summary>
        /// 通过cmd命令执行对象里面的方法。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void ContextInvoke<T>(T t, UpMsg errmsg)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request["cmd"]))
            {
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.Write(WebUtil.Wrap(JsonConvert.SerializeObject(new UpMsg { success = false, message = "命令参数不能为空" })));
                return;
            }
            var context = HttpContext.Current;
            MethodInfo mf = t.GetType().GetMethod(context.Request["cmd"], BindingFlags.Public | BindingFlags.Instance, null, new Type[] { }, null);
            if (mf != null)
            {
                try
                {
                    mf.Invoke(t, null);
                    //HttpContext.Current.Response.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.StatusCode = 500;
                    context.Response.Write(WebUtil.Wrap(JsonConvert.SerializeObject(new UpMsg { success = false, message = ex.InnerException.Message })));

                }

            }
            else
            {
                HttpContext.Current.Response.StatusCode = 500;
                context.Response.Write(WebUtil.Wrap(JsonConvert.SerializeObject(errmsg)));
            }
        }

        public static string GetTreeJson(List<TreeJson> jsons)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            var tdata = JsonConvert.SerializeObject(jsons, setting);
            return Wrap(tdata);
        }

        public static string GetResGridTreeJson(List<ResTreeGridJson> jsons)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            var tdata = JsonConvert.SerializeObject(jsons, setting);
            return Wrap(tdata);
        }

        public static void UploadFile(Action<string> callback = null)
        {
            HttpContext context = HttpContext.Current;
            context.Response.CacheControl = "no-cache";

            string s_rpath = FileHelper.GetUploadPath();

            string Datedir = DateTime.Now.ToString("yy-MM-dd");

            string updir = Path.Combine(s_rpath, Datedir);
            if (!Directory.Exists(updir))
            {
                Directory.CreateDirectory(updir);
            }
            if (context.Request.Files.Count > 0)
            {
                for (int j = 0; j < context.Request.Files.Count; j++)
                {
                    HttpPostedFile uploadFile = context.Request.Files[j];

                    if (uploadFile.ContentLength > 0)
                    {
                        if (!Directory.Exists(updir))
                        {
                            Directory.CreateDirectory(updir);
                        }
                        string extname = Path.GetExtension(uploadFile.FileName);
                        // string fullname = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                        string filename = Path.GetFileName(uploadFile.FileName);
                        uploadFile.SaveAs(string.Format("{0}\\{1}", updir, filename));
                        if (callback != null)
                        {
                            callback(string.Format("{0}\\{1}", updir, filename));
                        }
                    }
                }


            }

        }

        /// <summary>
        /// 页面下载模板
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool ExportExcelWithAspose(System.Data.DataTable dt)
        {
            bool succeed = false;
            if (dt != null)
            {
                try
                {
                    //Aspose.Cells.License li = new Aspose.Cells.License();
                    //Aspose.Cells.License licExcel = new License();  //Aspose.Cells申明
                    //if (File.Exists(HttpContext.Current.Server.MapPath("~/Bin/cellLic.lic")))
                    //{
                    //    licExcel.SetLicense(HttpContext.Current.Server.MapPath("~/Bin/cellLic.lic"));
                    //}
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];

                    cellSheet.Name = dt.TableName;

                    int rowIndex = 0;
                    int colIndex = 0;
                    int colCount = dt.Columns.Count;
                    int rowCount = dt.Rows.Count;

                    //列名的处理
                    for (int i = 0; i < colCount; i++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                        Style style1 = new Style();
                        cellSheet.Cells[rowIndex, colIndex].SetStyle(style1);
                        style1.Font.IsBold = true;
                        style1.Font.Name = "宋体";
                        colIndex++;
                    }

                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    style.Font.Name = "Arial";
                    style.Font.Size = 10;
                    //Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                    //cellSheet.Cells.AsParallel(style, styleFlag);

                    rowIndex++;

                    for (int i = 0; i < rowCount; i++)
                    {
                        colIndex = 0;
                        for (int j = 0; j < colCount; j++)
                        {
                            cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
                            colIndex++;
                        }
                        rowIndex++;
                    }

                    cellSheet.AutoFitColumns();
                    var response = HttpContext.Current.Response;
                    response.Clear();
                    response.Buffer = true;
                    response.Charset = "utf-8";
                    response.AppendHeader("Content-Disposition", "attachment;filename=" + dt.TableName + ".xls");
                    response.ContentEncoding = System.Text.Encoding.UTF8;
                    response.ContentType = "application/ms-excel";
                    response.BinaryWrite(workbook.SaveToStream().ToArray());
                    response.End();

                    //workbook.Save(HttpContext.Current.Response,FileFormatType.Excel97To2003);
                    //path = Path.GetFullPath(path);
                    //workbook.Save(path);
                    succeed = true;
                }
                catch (Exception ex)
                {
                    succeed = false;
                }
            }

            return succeed;
        }

    }

    /// <summary>

    ///FileHelper 的摘要说明

    /// </summary>

    public class FileHelper
    {

        public FileHelper()
        {

            //

            //TODO: 在此处添加构造函数逻辑

            //

        }

        public static string GetUploadPath()
        {

            string path = HttpContext.Current.Server.MapPath("~/");

            string dirname = GetDirName();

            string uploadDir = Path.Combine(path, dirname);

            CreateDir(uploadDir);

            return uploadDir;

        }





        private static string GetDirName()
        {
            //return string.Empty;
            return System.Configuration.ConfigurationManager.AppSettings["uploaddir"];

        }

        public static void CreateDir(string path)
        {

            if (!System.IO.Directory.Exists(path))
            {

                System.IO.Directory.CreateDirectory(path);

            }

        }




    }




}
