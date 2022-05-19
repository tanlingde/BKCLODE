using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Aspose.Cells;
using System.IO;
using BK.Cloud.Model.Customer;
using BK.Cloud.Tools;
using BK.Cloud.Facade.Interface;
using BK.Cloud.Logic;
using BK.Cloud.Logic.InnerObj;
using BK.Cloud.Model.Data.Model;
using Microsoft.JScript.Vsa;

namespace BK.Cloud.Facade
{
    public class BaseWeb
    {

        /// <summary>
        /// 页面序列json包装方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string SerialObj(object obj)
        {
            //Newtonsoft.Json.Converters.IsoDateTimeConverter iso = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //iso.DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            var serialset = new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddTHH:mm:ss" };
            serialset.DefaultValueHandling = DefaultValueHandling.Include;
            serialset.Formatting = Formatting.None;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, serialset);
            //return WebUtil.Wrap(Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented, iso));
        }
        public string SerialStr(string jsonstr)
        {
            return jsonstr;
            //return WebUtil.Wrap(jsonstr);
        }

        public void DeSerialObj<T>(string info, T t)
        {
            JsonConvert.PopulateObject(info, t);
        }

        /// <summary>
        /// 主要实现Aspose.cells中的Excel格式化输出定制
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static Workbook Object2Workbook(dynamic jsonObject, HttpContext context, dynamic retData)
        {

            //#region Aspose.Cell引用

            //Aspose.Cells.License licExcel = new License();  //Aspose.Cells申明

            //if (File.Exists(context.Server.MapPath("~/Bin/cellLic.lic")))

            //    licExcel.SetLicense(context.Server.MapPath("~/Bin/cellLic.lic"));

            //#endregion



            Workbook workbook = new Workbook();
            Worksheet sheet = workbook.Worksheets[0];


            //Styles styles = workbook.Styles;

            //int styleIndex = //styles.Add();

            Aspose.Cells.Style borderStyle = workbook.Styles[workbook.Styles.Add()];

            borderStyle.Borders.DiagonalStyle = CellBorderType.None;

            borderStyle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 

            Cells cells = sheet.Cells;

            //sheet.FreezePanes(1, 1, 1, 0);//冻结第一行

            sheet.Name = jsonObject.sheetName;//接受前台的Excel工作表名


            //为标题设置样式     

            Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式 

            styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中 

            styleTitle.Font.Name = "宋体";//文字字体 

            styleTitle.Font.Size = 18;//文字大小 

            styleTitle.Font.IsBold = true;//粗体 



            //题头样式 

            Style styleHeader = workbook.Styles[workbook.Styles.Add()];//新增样式 

            styleHeader.HorizontalAlignment = TextAlignmentType.Center;//文字居中 

            styleHeader.Font.Name = "宋体";//文字字体 

            styleHeader.Font.Size = 14;//文字大小 

            styleHeader.Font.IsBold = true;//粗体 

            styleHeader.IsTextWrapped = true;//单元格内容自动换行 

            styleHeader.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;

            styleHeader.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            styleHeader.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;

            styleHeader.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;



            //内容样式

            Style styleContent = workbook.Styles[workbook.Styles.Add()];//新增样式 

            styleContent.HorizontalAlignment = TextAlignmentType.Center;//文字居中 

            styleContent.Font.Name = "宋体";//文字字体 

            styleContent.Font.Size = 12;//文字大小 

            styleContent.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;

            styleContent.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            styleContent.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;

            styleContent.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            styleContent.Number = 49;


            //var rowCount = jsonObject.rows.Count;//表格行数 
            var columnCount = jsonObject.columns.Count;//表格列数 


            //生成行1 标题行    

            cells.Merge(0, 0, 1, columnCount);//合并单元格 

            cells[0, 0].PutValue(jsonObject.sheetName);//填写内容 
            cells[0, 0].SetStyle(styleTitle);
            cells.SetRowHeight(0, 25);

            //生成行2 备注    
            cells.Merge(1, 0, 1, columnCount);//合并单元格 

            cells[1, 0].PutValue(jsonObject.remark);//填写内容 

            //cells[0, 0].Style = styleTitle;

            cells.SetRowHeight(1, 15);

            //生成题头列行 
            for (int i = 0; i < columnCount; i++)
            {

                cells[2, i].PutValue(jsonObject.columns[i]["title"]);
                cells[2, i].SetStyle(styleHeader);
                cells.SetRowHeight(2, 23);

            }
            int rowCount = 0;
            //生成内容行,第三行起始
            //生成数据行 
            if (retData != null)
            {
                //返回UpMsg
                if (retData.rows != null)
                {
                    rowCount = retData.rows.Count;
                    for (int i = 0; i < rowCount; i++)
                    {

                        for (int k = 0; k < columnCount; k++)
                        {
                            var currentColumnName = jsonObject.columns[k]["field"];
                            var newValue = retData.rows[i][currentColumnName.Value];
                            string functions = (jsonObject.columns[k]["formatter"]).Value;
                            string funname = "";
                            if (!string.IsNullOrEmpty(functions) && functions.IndexOf('(') != -1)
                            {
                                functions = "function testmethod" + functions.Substring(functions.IndexOf('(')) + "";
                                funname = "testmethod";
                                newValue = EvalJScript(functions + funname + "('" + newValue + "'," + retData.rows[i] + ",'" + i + "');", newValue);
                            }
                            cells[3 + i, k].PutValue(newValue);
                            cells[3 + i, k].SetStyle(styleContent);
                        }
                        cells.SetRowHeight(3 + i, 22);
                    }
                }
                //返回DataTable
                else
                {
                    rowCount = retData.Count;
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int k = 0; k < columnCount; k++)
                        {
                            var currentColumnName = jsonObject.columns[k]["field"];
                            var newValue = retData[i][currentColumnName.Value];
                            string functions = (jsonObject.columns[k]["formatter"]).Value;
                            string funname = "";
                            int mindex = functions.IndexOf('(');
                            if (!string.IsNullOrEmpty(functions) && mindex != -1)
                            {
                                functions = "function testmethod" + functions.Substring(mindex) + "";
                                funname = "testmethod";
                                newValue = EvalJScript(functions + funname + "('" + newValue + "'," + retData.rows[i] + ",'" + i + "');", newValue);
                            }
                            cells[3 + i, k].PutValue(newValue);
                            cells[3 + i, k].SetStyle(styleContent);
                        }
                        cells.SetRowHeight(3 + i, 22);
                    }
                }
            }
            //传入行数据信息,只导出当前页
            else if (jsonObject.rows != null)
            {
                rowCount = jsonObject.rows.Count;//或者用这个方法取行数 retData.total;//表格行数 
                for (int i = 0; i < rowCount; i++)
                {

                    for (int k = 0; k < columnCount; k++)
                    {
                        var currentColumnName = jsonObject.columns[k]["field"];
                        cells[3 + i, k].PutValue(jsonObject.rows[i][currentColumnName.Value]);
                        cells[3 + i, k].SetStyle(styleContent);
                    }
                    cells.SetRowHeight(3 + i, 22);
                }
            }

            //添加制表日期
            cells[3 + rowCount, columnCount - 1].PutValue("制表日期:" + DateTime.Now.ToShortDateString());

            sheet.AutoFitColumns();//让各列自适应宽度

            sheet.AutoFitRows();//让各行自适应宽度

            return workbook;

        }

        //public IFacadeAuthor FacadeAuthor { get; set; }

        //执行JS方法
        public static object EvalJScript(string JScript, dynamic defaultVal)
        {

            object Result = null;

            try
            {

                Result = Microsoft.JScript.Eval.JScriptEvaluate(JScript, VsaEngine.CreateEngine());

            }

            catch (Exception ex)
            {
                //Fireasy.Common.Logging.DefaultLogger.Instance.Error("");
                return defaultVal;
                // return ex.Message;

            }

            return Result;



        }
    }
}