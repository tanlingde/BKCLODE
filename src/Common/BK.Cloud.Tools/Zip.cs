using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Tools
{
    public class Zip
    {

        public static void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            ZipOutputStream s = new ZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            zip(strFile, s, strFile);
            s.Finish();
            s.Close();
        }

        public static void TarFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
                strFile += Path.DirectorySeparatorChar;
            GZipOutputStream s = new GZipOutputStream(File.Create(strZip));
            s.SetLevel(6); // 0 - store only to 9 - means best compression
            TarArchive archive = TarArchive.CreateOutputTarArchive(s, TarBuffer.DefaultBlockFactor);
            targz(strFile, archive, strFile);
            archive.Close();
            s.Finish();
            s.Close();
        }


        /// <summary>
        /// 生成tar文件  （针对于多个文件）
        /// </summary>
        /// <param name="strBasePath">文件夹路径——将被压缩的文件所在的地方</param>        
        /// <param name="listFilesPath">文件的路径：H:\Demo\xxx.txt</param>
        /// <param name="tarFileName">压缩后tar文件名称</param>
        /// <returns></returns>
        public static bool CreatTarArchive(string strBasePath, List<string> listFilesPath, string tarFileName)//"20180524" + ".tar"
        {
            if (string.IsNullOrEmpty(strBasePath) || string.IsNullOrEmpty(tarFileName) || !System.IO.Directory.Exists(strBasePath))
                return false;

            
            string strOupFileAllPath = Path.Combine(strBasePath, tarFileName);//一个完整的文件路径 .tar
            Stream outStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);//打开.tar文件
            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            Environment.CurrentDirectory = strBasePath;
            
            for (int i = 0; i < listFilesPath.Count; i++)
            {
                string fileName = listFilesPath[i];
               TarEntry entry = TarEntry.CreateEntryFromFile(fileName);//将文件写到.tar文件中去
                entry.Name =Path.GetFileName(fileName);
                archive.WriteEntry(entry, true);
                File.Delete(fileName);
            }

            if (archive != null)
            {
                archive.Close();
            }
            outStream.Close();
            return true;
        }



        /// <summary>  
        /// 生成 ***.tar.gz 文件  
        /// </summary>  
        /// <param name="strBasePath">文件基目录（源文件、生成文件所在目录）</param>  
        /// <param name="strSourceFolderName">待压缩的源文件夹名</param>  
        public static bool CreatTarGzArchive(string strBasePath, string strSourceFolderName)
        {
            //if (string.IsNullOrEmpty(strBasePath)
            //    || string.IsNullOrEmpty(strSourceFolderName)
            //    || !System.IO.Directory.Exists(strBasePath)
            //    || System.IO.Directory.Exists(Path.Combine(strBasePath, strSourceFolderName)))
            //{
            //    return false;
            //}

            Environment.CurrentDirectory = strBasePath;
            string strSourceFolderAllPath = Path.Combine(strBasePath, strSourceFolderName);
            string strOupFileAllPath = Path.Combine(strBasePath, strSourceFolderName + ".tar.gz");

            Stream outTmpStream = new FileStream(strOupFileAllPath, FileMode.OpenOrCreate);
            //注意此处源文件大小大于4096KB  
            Stream outStream = new GZipOutputStream(outTmpStream);
            TarArchive archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            TarEntry entry = TarEntry.CreateEntryFromFile(strBasePath);
            archive.WriteEntry(entry, true);

            if (archive != null)
            {
                archive.Close();
            }
            outTmpStream.Close();
            outStream.Close();
            return true;
        }

        private static void targz(string strFile, TarArchive archive, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {

                if (Directory.Exists(file))
                {
                    targz(file, archive, staticFile);
                }
                else // 否则直接压缩文件
                {
                    // TarArchive archive = TarArchive.CreateOutputTarArchive(s, TarBuffer.DefaultBlockFactor);
                    TarEntry entry = TarEntry.CreateEntryFromFile(file);//将文件写到.tar文件中去
                    archive.WriteEntry(entry, true);

                }
            }
        }

        private static void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar) strFile += Path.DirectorySeparatorChar;
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {

                if (Directory.Exists(file))
                {
                    zip(file, s, staticFile);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    FileStream fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public static string UnZipFile(string TargetFile, string fileDir)
        {
            string rootFile = " ";
            try
            {
                //读取压缩文件(zip文件)，准备解压缩
                ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                ZipEntry theEntry;
                string path = fileDir;
                //解压出来的文件保存的路径

                string rootDir = " ";
                //根目录下的第一个子文件夹的名称
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    rootDir = Path.GetDirectoryName(theEntry.Name);
                    //得到根目录下的第一级子文件夹的名称
                    if (rootDir.IndexOf("\\") >= 0)
                    {
                        rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                    }
                    string dir = Path.GetDirectoryName(theEntry.Name);
                    //根目录下的第一级子文件夹的下的文件夹的名称
                    string fileName = Path.GetFileName(theEntry.Name);
                    //根目录下的文件名称
                    if (dir != " ")
                    //创建根目录下的子文件夹,不限制级别
                    {
                        if (!Directory.Exists(fileDir + "\\" + dir))
                        {
                            path = fileDir + "\\" + dir;
                            //在指定的路径创建文件夹
                            Directory.CreateDirectory(path);
                        }
                    }
                    else if (dir == " " && fileName != "")
                    //根目录下的文件
                    {
                        path = fileDir;
                        rootFile = fileName;
                    }
                    else if (dir != " " && fileName != "")
                    //根目录下的第一级子文件夹下的文件
                    {
                        if (dir.IndexOf("\\") > 0)
                        //指定文件保存的路径
                        {
                            path = fileDir + "\\" + dir;
                        }
                    }

                    if (dir == rootDir)
                    //判断是不是需要保存在根目录下的文件
                    {
                        path = fileDir + "\\" + rootDir;
                    }

                    //以下为解压缩zip文件的基本步骤
                    //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                    if (fileName != String.Empty)
                    {
                        FileStream streamWriter = File.Create(path + "\\" + fileName);

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        streamWriter.Close();
                    }
                }
                s.Close();

                return rootFile;
            }
            catch (Exception ex)
            {
                return "1; " + ex.Message;
            }
        }

    }
}
