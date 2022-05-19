using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Aspose.Cells;

namespace BK.Cloud.Facade.FacadeTools
{
    public class PrevDocument
    {
        #region 转Html
        public string iOffice2Html(string sourcePath)
        {
            string name = System.IO.Path.GetFileName(sourcePath);

            string UploadUrlLocal = HttpContext.Current.Server.MapPath("~/api/tmpfiles/");
            if (!Directory.Exists(UploadUrlLocal))
            {
                Directory.CreateDirectory(UploadUrlLocal); 
            }
            //var sourcePath = UploadUrlLocal + name;
            var targetPath = UploadUrlLocal + name.Split('.')[0] + ".html";
            ExcelConvertToHtml(sourcePath, targetPath);
            return "/api/tmpfiles/" + name.Split('.')[0] + ".html";
            //if (System.IO.File.Exists(sourcePath))
            //{
            //    System.IO.File.Delete(sourcePath);
            //}
        }

        public MemoryStream iOffice2HtmlToWebStream(string sourcePath)
        {
            return ExcelConvertToHtml(sourcePath);
        }

        void ExcelConvertToHtml(string xlsPath, string htmlPath)
        {
            xlsPath = xlsPath.ToLower();
            if (xlsPath.EndsWith(".doc") || xlsPath.EndsWith(".docx"))
            {
                Aspose.Words.Document d = new Aspose.Words.Document(xlsPath);
                d.Save(htmlPath, Aspose.Words.SaveFormat.Html);
            }
            if (xlsPath.EndsWith(".xls") || xlsPath.EndsWith(".xlsx"))
            {
                Workbook workbook = new Workbook(xlsPath);
                if (workbook.Worksheets.Count <= 0)
                {
                    return;
                }
                workbook.Save(htmlPath, Aspose.Cells.SaveFormat.Html);
            }

            if (xlsPath.EndsWith(".ppt") || xlsPath.EndsWith(".pptx"))
            {
                Aspose.Slides.Presentation d = new Aspose.Slides.Presentation(xlsPath);
                d.Save(htmlPath, Aspose.Slides.Export.SaveFormat.Html);
            }
            if (xlsPath.EndsWith(".pdf"))
            {
                Aspose.Pdf.Document pdf = new Aspose.Pdf.Document(xlsPath);
                Aspose.Pdf.HtmlSaveOptions saveOptions = new Aspose.Pdf.HtmlSaveOptions();
                saveOptions.FixedLayout = true;
                saveOptions.SplitIntoPages = false;
                saveOptions.RasterImagesSavingMode = Aspose.Pdf.HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
                pdf.Save(htmlPath, saveOptions);
            }
        }
        MemoryStream ExcelConvertToHtml(string xlsPath)
        {
            xlsPath = xlsPath.ToLower();
            MemoryStream responseStream = new MemoryStream();
            if (xlsPath.EndsWith(".doc") || xlsPath.EndsWith(".docx"))
            {
                Aspose.Words.Document d = new Aspose.Words.Document(xlsPath);
                d.Save(responseStream, Aspose.Words.SaveFormat.Html);
            }
            if (xlsPath.EndsWith(".xls") || xlsPath.EndsWith(".xlsx"))
            {
                Workbook workbook = new Workbook(xlsPath);
                if (workbook.Worksheets.Count <= 0)
                {
                    return responseStream;
                }
                workbook.Save(responseStream, Aspose.Cells.SaveFormat.Html);
            }

            if (xlsPath.EndsWith(".ppt") || xlsPath.EndsWith(".pptx"))
            {
                Aspose.Slides.Presentation d = new Aspose.Slides.Presentation(xlsPath);
                d.Save(responseStream, Aspose.Slides.Export.SaveFormat.Html);
            }
            if (xlsPath.EndsWith(".pdf"))
            {
                Aspose.Pdf.Document pdf = new Aspose.Pdf.Document(xlsPath);
                Aspose.Pdf.HtmlSaveOptions saveOptions = new Aspose.Pdf.HtmlSaveOptions();
                saveOptions.FixedLayout = true;
                // Split HTML output into pages
                saveOptions.SplitIntoPages = false;
                // Split CSS into pages
                saveOptions.SplitCssIntoPages = false;

                saveOptions.CustomCssSavingStrategy = new Aspose.Pdf.HtmlSaveOptions.CssSavingStrategy(Strategy_4_CSS_MULTIPAGE_SAVING_RIGHT_WAY);
                    //new HtmlSaveOptions.CssSavingStrategy(Strategy_4_CSS_MULTIPAGE_SAVING_RIGHT_WAY);
                saveOptions.CustomStrategyOfCssUrlCreation = new Aspose.Pdf.HtmlSaveOptions.CssUrlMakingStrategy(Strategy_5_CSS_MAKING_CUSTOM_URL_FOR_MULTIPAGING);
                saveOptions.CustomResourceSavingStrategy = new Aspose.Pdf.HtmlSaveOptions.ResourceSavingStrategy(CacheFontsStrategy);
                saveOptions.RasterImagesSavingMode = Aspose.Pdf.HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
                pdf.Save(responseStream, saveOptions);
            }
            return responseStream;
            //var response = HttpContext.Current.Response;
            //response.ContentEncoding = System.Text.Encoding.UTF8;
            //response.Write(responseStream.ToArray());
            //response.Clear();
            //response.Buffer = true;
            //response.Charset = "utf-8";
            //response.AppendHeader("Content-Disposition", "attachment;filename=" + dt.TableName + ".xls");
            //response.ContentEncoding = System.Text.Encoding.UTF8;
            //response.ContentType = "application/ms-excel";
            //response.BinaryWrite(responseStream.ToArray());
            //response.End();
        }
        #endregion
        private static void Strategy_4_CSS_MULTIPAGE_SAVING_RIGHT_WAY(Aspose.Pdf.HtmlSaveOptions.CssSavingInfo partSavingInfo)
        {
            string cssOutFolder = @"c:\pdftest\ResultantFile_files\";
            if (!Directory.Exists(cssOutFolder))
            {
                Directory.CreateDirectory(cssOutFolder);
            }

            string outPath = cssOutFolder + "style_xyz_page" + partSavingInfo.CssNumber+ ".css";
            System.IO.BinaryReader reader = new BinaryReader(partSavingInfo.ContentStream);
            System.IO.File.WriteAllBytes(outPath, reader.ReadBytes((int)partSavingInfo.ContentStream.Length));
        }

        private static string Strategy_5_CSS_MAKING_CUSTOM_URL_FOR_MULTIPAGING(Aspose.Pdf.HtmlSaveOptions.CssUrlRequestInfo requestInfo)
        {
            return "/document-viewer/GetCss?cssId=4544554445_page{0}";
        }

        static object resourceSavingSync = new object();
        /// 
        /// Resource saving callback that saves fonts into output folder and builds css links to the fonts
        /// 
        private string CacheFontsStrategy(Aspose.Pdf.SaveOptions.ResourceSavingInfo resourceSavingInfo)
        {
            string fileNameOnly = "tmpfont";
            // The callback is performed in parallel threads, so synchronization must be implemented
            lock (resourceSavingSync)
            {
                string fontsFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileNameOnly + "_fonts\\"));
                if (!Directory.Exists(fontsFolder))
                    Directory.CreateDirectory(fontsFolder);

                // First path of this method is for saving of font
                if (resourceSavingInfo.ResourceType == Aspose.Pdf.SaveOptions.NodeLevelResourceType.Font)
                {
                    string outFontFile = fontsFolder + Path.GetFileName(resourceSavingInfo.SupposedFileName);
                    System.IO.BinaryReader fontBinaryReader = new BinaryReader(resourceSavingInfo.ContentStream);
                    System.IO.File.WriteAllBytes(outFontFile,
                        fontBinaryReader.ReadBytes((int)resourceSavingInfo.ContentStream.Length));
                    string fontUrl =fileNameOnly + "_fonts/" + resourceSavingInfo.SupposedFileName;
                    return fontUrl;
                }
                resourceSavingInfo.CustomProcessingCancelled = true;
                return null;
            }
        }

        #region 转Pdf
        public string iOffice2Pdf(string sourcePath)
        {
            Office2Pdf office2pdf = new Office2Pdf();

            string savePath = HttpContext.Current.Server.MapPath("~/api/tmpfiles/");
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string name = System.IO.Path.GetFileName(sourcePath);
            var name1 = name.Split('.')[0];
            var a = name.Split('.')[1];
            //string sourcePath = savePath + name;
            string targetPath = savePath + name1 + ".pdf";
            bool flag = true;
            if (a == "doc" || a == "docx")
            {
                if (!office2pdf.DOCConvertToPDF(sourcePath, targetPath))
                {
                    flag = false;
                }
            }
            else if (a == "ppt" || a == "pptx")
            {
                if (!office2pdf.PPTConvertToPDF(sourcePath, targetPath))
                {
                    flag = false;
                }
            }
            else if (a == "xlsx" || a == "xls")
            {
                if (!office2pdf.XLSConvertToPDF(sourcePath, targetPath))
                {
                    flag = false;
                }
            }
            else if (a != "pdf")
            {
                flag = false;
            }
            string mtargetPath = "";
            if (flag)
            {
                if (a != "pdf")
                {
                    if (System.IO.File.Exists(savePath + name))
                    {
                        System.IO.File.Delete(savePath + name);
                    }
                    mtargetPath = iPdf2Swf(targetPath, name1);
                }
                else
                {
                    mtargetPath = iPdf2Swf(sourcePath, name1);
                }
            }
            return mtargetPath;
            //"/api/tmpfiles/" + name1 + ".swf";
        }

        string iPdf2Swf(string sourcePath, string name1)
        {
            string cmdStr = HttpContext.Current.Server.MapPath("~/api/ST_Component/FlexPaper/SWFTools/pdf2swf.exe");

            string savePath = HttpContext.Current.Server.MapPath("~/api/tmpfiles/");
            //string sourcePath = @"""" + savePath + name1 + ".pdf" + @"""";
            string targetPath = @"""" + savePath + name1 + ".swf" + @"""";
            string argsStr = "  -t " + sourcePath + " -s flashversion=9 -o " + targetPath;
            ExcutedCmd(cmdStr, argsStr);
            if (System.IO.File.Exists(savePath + name1 + ".pdf"))
            {
                System.IO.File.Delete(savePath + name1 + ".pdf");
            }
            return "/api/tmpfiles/" + name1 + ".swf";
        }

        void ExcutedCmd(string cmd, string args)
        {
            using (Process p = new Process())
            {
                ProcessStartInfo psi = new ProcessStartInfo(cmd, args);
                p.StartInfo = psi;
                p.Start();
                p.WaitForExit();
            }
        }
        #endregion
    }

    public class Office2Pdf
    {
        public Office2Pdf()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// Word转换成pdf
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public bool DOCConvertToPDF(string sourcePath, string targetPath)
        {
            Aspose.Words.Document d = new Aspose.Words.Document(sourcePath);
            d.Save(targetPath, Aspose.Words.SaveFormat.Pdf);
            return true;
            //bool result = false;
            //Word.WdExportFormat exportFormat = Word.WdExportFormat.wdExportFormatPDF;
            //object paramMissing = Type.Missing;
            //Word.Application wordApplication = new Word.Application();
            //Word.Document wordDocument = null;
            //try
            //{
            //    object paramSourceDocPath = sourcePath;
            //    string paramExportFilePath = targetPath;
            //    Word.WdExportFormat paramExportFormat = exportFormat;
            //    bool paramOpenAfterExport = false;
            //    Word.WdExportOptimizeFor paramExportOptimizeFor = Word.WdExportOptimizeFor.wdExportOptimizeForPrint;
            //    Word.WdExportRange paramExportRange = Word.WdExportRange.wdExportAllDocument;
            //    int paramStartPage = 0;
            //    int paramEndPage = 0;
            //    Word.WdExportItem paramExportItem = Word.WdExportItem.wdExportDocumentContent;
            //    bool paramIncludeDocProps = true;
            //    bool paramKeepIRM = true;
            //    Word.WdExportCreateBookmarks paramCreateBookmarks = Word.WdExportCreateBookmarks.wdExportCreateWordBookmarks;
            //    bool paramDocStructureTags = true;
            //    bool paramBitmapMissingFonts = true;
            //    bool paramUseISO19005_1 = false;
            //    wordDocument = wordApplication.Documents.Open(
            //        ref paramSourceDocPath, ref paramMissing, ref paramMissing,
            //        ref paramMissing, ref paramMissing, ref paramMissing,
            //        ref paramMissing, ref paramMissing, ref paramMissing,
            //        ref paramMissing, ref paramMissing, ref paramMissing,
            //        ref paramMissing, ref paramMissing, ref paramMissing,
            //        ref paramMissing);
            //    if (wordDocument != null)
            //        wordDocument.ExportAsFixedFormat(paramExportFilePath,
            //            paramExportFormat, paramOpenAfterExport,
            //            paramExportOptimizeFor, paramExportRange, paramStartPage,
            //            paramEndPage, paramExportItem, paramIncludeDocProps,
            //            paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
            //            paramBitmapMissingFonts, paramUseISO19005_1,
            //            ref paramMissing);
            //    result = true;
            //}
            //catch
            //{
            //    result = false;
            //}
            //finally
            //{
            //    if (wordDocument != null)
            //    {
            //        wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
            //        wordDocument = null;
            //    }
            //    if (wordApplication != null)
            //    {
            //        wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
            //        wordApplication = null;
            //    }
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
            //return result;
        }

        /// <summary>
        /// 把Excel文件转换成PDF格式文件  
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            Workbook workbook = new Workbook(sourcePath);
            if (workbook.Worksheets.Count <= 0)
            {
                return false;
            }
            workbook.Save(targetPath, Aspose.Cells.SaveFormat.Pdf);
            return true;
            //bool result = false;
            //Excel.XlFixedFormatType targetType = Excel.XlFixedFormatType.xlTypePDF;
            //object missing = Type.Missing;

            ////Object missing = Missing.Value;
            //Excel.Application application = null;
            //Excel.Workbook workBook = null;
            //try
            //{
            //    application = new Excel.Application();
            //    object target = targetPath;
            //    object type = targetType;
            //    workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            //    workBook.ExportAsFixedFormat(targetType, target, Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
            //    result = true;
            //}
            //catch
            //{
            //    result = false;
            //}
            //finally
            //{
            //    if (workBook != null)
            //    {
            //        workBook.Close(true, missing, missing);
            //        workBook = null;
            //    }
            //    if (application != null)
            //    {
            //        application.Quit();
            //        application = null;
            //    }
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
            //return result;
        }
        ///<summary>        
        /// 把PowerPoint文件转换成PDF格式文件       
        ///</summary>        
        ///<param name="sourcePath">源文件路径</param>     
        ///<param name="targetPath">目标文件路径</param> 
        ///<returns>true=转换成功</returns> 
        public bool PPTConvertToPDF(string sourcePath, string targetPath)
        {
            Aspose.Slides.Presentation d = new Aspose.Slides.Presentation(sourcePath);
            d.Save(targetPath, Aspose.Slides.Export.SaveFormat.Pdf);
            return true;
            //bool result;
            //Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType targetFileType = Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF;
            //object missing = Type.Missing;
            //Microsoft.Office.Interop.PowerPoint.Application application = null;
            //Microsoft.Office.Interop.PowerPoint.Presentation persentation = null;
            //try
            //{
            //    application = new Microsoft.Office.Interop.PowerPoint.Application();
            //    persentation = application.Presentations.Open(sourcePath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse); persentation.SaveAs(targetPath, targetFileType, Microsoft.Office.Core.MsoTriState.msoTrue);
            //    result = true;
            //}
            //catch
            //{
            //    result = false;
            //}
            //finally
            //{
            //    if (persentation != null)
            //    {
            //        persentation.Close();
            //        persentation = null;
            //    }
            //    if (application != null)
            //    {
            //        application.Quit();
            //        application = null;
            //    }
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //    GC.Collect();
            //    GC.WaitForPendingFinalizers();
            //}
            //return result;
        }
    }

}




