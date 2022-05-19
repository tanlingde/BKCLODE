using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Collections.Generic;


namespace BK.Cloud.Tools
{
    /// <summary>
    /// 升级服务客户端
    /// </summary>
    public class FTPApplication
    {

        public static void UpdataFile1(string filePath, string user, string pwd, string ip, string port, string remoteName = "")
        {
            string ftpdic = "ftp://{0}:{1}";
            ftpdic = string.Format(ftpdic, ip, port);
            FTPApplication.UpMyLoadFile(filePath, ftpdic, user, pwd, remoteName);
        }

        public static string UpdataFile(string filePath, string user, string pwd, string ip, string port, string remotesavePath = "", string dic = "")
        {
            if (dic == null)
                return "";

            string CrcCode = CRC16.GetCrc16Code(filePath);
            
            
            dic = dic.Replace("\\", "/");
            if (!string.IsNullOrEmpty(dic) && !dic.EndsWith("/"))
            {
                dic = dic + "/";
            }
            if (!string.IsNullOrEmpty(dic) && dic.StartsWith("/"))
            {
                dic = dic.Remove(0, 1);
            }
            remotesavePath = remotesavePath.Replace("\\", "/");
            string ftpdic = "ftp://{0}:{1}/{2}{3}";

            ftpdic = string.Format(ftpdic, ip, port, remotesavePath, dic);
            FTPApplication.UpMyLoadFile(filePath, ftpdic, user, pwd, Path.GetFileName(filePath)+CrcCode);
            return ftpdic;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">要上传到FTP服务器的文件</param>
        /// <param name="ftpPath"></param>
        public static void UpMyLoadFile(string localFile, string ftpDicPath, string ftpUser, string ftpPassword, string ftpName)
        {
            if (ftpUser == null)
            {
                ftpUser = "";
            }
            if (ftpPassword == null)
            {
                ftpPassword = "";
            }

            if (!File.Exists(localFile))
            {
                return;
            }

            FtpWebRequest ftpWebRequest = null;
            FileStream localFileStream = null;
            Stream requestStream = null;
            try
            {
                if (!ftpDicPath.EndsWith("/"))
                {
                    ftpDicPath += "/";
                }
                if (!DirectoryIsExist(new Uri(ftpDicPath), ftpUser, ftpPassword))
                {
                    CreateDirectory(new Uri(ftpDicPath), ftpUser, ftpPassword);
                }
                DeleteFtpFile(ftpDicPath, ftpName, ftpUser, ftpPassword);
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpDicPath + ftpName));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpWebRequest.ContentLength = localFile.Length;
                int buffLength = 4096;
                byte[] buff = new byte[buffLength];
                int contentLen;
                localFileStream = new FileInfo(localFile).OpenRead();
                requestStream = ftpWebRequest.GetRequestStream();
                contentLen = localFileStream.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    requestStream.Write(buff, 0, contentLen);
                    contentLen = localFileStream.Read(buff, 0, buffLength);
                }
            }
            catch (Exception ex)
            {
                // UpdateLog.WriteLog("FileUpLoad0001", ex);
                throw ex;
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (localFileStream != null)
                {
                    localFileStream.Close();
                }
            }
        }


        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="ServerIP"></param>
        /// <param name="pFtpUserID"></param>
        /// <param name="pFtpPW"></param>
        /// <param name="FileSource"></param>
        /// <param name="FileCategory"></param>
        /// <returns></returns>
        public static bool checkDirectory(string ServerIP, string pFtpUserID, string pFtpPW, string FileSource, string FileCategory)
        {
            //检测目录是否存在
            Uri uri = new Uri("ftp://" + ServerIP + "/" + FileSource + "/");
            if (!DirectoryIsExist(uri, pFtpUserID, pFtpPW))
            {
                //创建目录
                uri = new Uri("ftp://" + ServerIP + "/" + FileSource);
                if (CreateDirectory(uri, pFtpUserID, pFtpPW))
                {
                    //检测下一级目录是否存在
                    uri = new Uri("ftp://" + ServerIP + "/" + FileSource + "/" + FileCategory + "/");
                    if (!DirectoryIsExist(uri, pFtpUserID, pFtpPW))
                    {
                        //创建目录
                        uri = new Uri("ftp://" + ServerIP + "/" + FileSource + "/" + FileCategory);
                        if (CreateDirectory(uri, pFtpUserID, pFtpPW))
                        {
                            return true;
                        }
                        else { return false; }
                    }
                    else
                    {
                        return true;
                    }
                }
                else { return false; }
            }
            else
            {
                //检测下一级目录是否存在
                uri = new Uri("ftp://" + ServerIP + "/" + FileSource + "/" + FileCategory + "/");
                if (!DirectoryIsExist(uri, pFtpUserID, pFtpPW))
                {
                    //创建目录
                    uri = new Uri("ftp://" + ServerIP + "/" + FileSource + "/" + FileCategory);
                    if (CreateDirectory(uri, pFtpUserID, pFtpPW))
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// ftp创建目录(创建文件夹)
        /// </summary>
        /// <param name="IP">IP服务地址</param>
        /// <param name="UserName">登陆账号</param>
        /// <param name="UserPass">密码</param>
        /// <param name="FileSource"></param>
        /// <param name="FileCategory"></param>
        /// <returns></returns>
        public static bool CreateDirectory(Uri IP, string UserName, string UserPass)
        {
            try
            {
                FtpWebRequest FTP = (FtpWebRequest)FtpWebRequest.Create(IP);
                FTP.Credentials = new NetworkCredential(UserName, UserPass);
                FTP.Proxy = null;
                FTP.KeepAlive = false;
                FTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FTP.UseBinary = true;
                FtpWebResponse response = FTP.GetResponse() as FtpWebResponse;
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="pFtpServerIP"></param>
        /// <param name="pFtpUserID"></param>
        /// <param name="pFtpPW"></param>
        /// <returns>false不存在，true存在</returns>
        public static bool DirectoryIsExist(Uri pFtpServerIP, string pFtpUserID, string pFtpPW)
        {
            string[] value = GetFileList(pFtpServerIP, pFtpUserID, pFtpPW);
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string[] GetFileList(Uri pFtpServerIP, string pFtpUserID, string pFtpPW)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(pFtpServerIP);
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(pFtpUserID, pFtpPW);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                return null;
            }
        }

        public static void Download(string fileName, string SaveDic, string user, string pwd, string ip, string port, Action<int, int> processAction = null)
        {
            FtpWebRequest reqFTP;

            try
            {

                FileStream outputStream = new FileStream(SaveDic + "\\" + fileName, FileMode.Create);
                string ftpPath = "ftp://" + ip + ":" + port + "/" + fileName;
                ftpPath = ftpPath.Replace("\\", "/");
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long cl = GetFileSize(ip, port, user, pwd, fileName);
                //response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                if (ftpStream == null)
                    return;
                const int bufferSize = 1024;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                long curentCount = 0;
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                if (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                }
                curentCount += readCount;
                while (curentCount < cl)
                {
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    if (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                    }
                    curentCount += readCount;
                    if (processAction != null)
                    {
                        processAction((int)(curentCount / 100), (int)(cl / 100));
                    }
                }
                if (processAction != null)
                {
                    processAction((int)(curentCount / 100), (int)(cl / 100));
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static int GetFileSize(string ip, string port, string uid, string pwd, string fileName)
        {
            FtpWebRequest request;
            try
            {
                string ftpPath = "ftp://" + ip + ":" + port + "/" + fileName;
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(uid, pwd);
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                int dataLength = (int)request.GetResponse().ContentLength;
                return dataLength;
            }
            catch (Exception ex)
            {
                return 1337;
            }
        }


        private static FtpWebRequest Connect(String path, string uid, string upwd)//连接ftp
        {
            // 根据uri创建FtpWebRequest对象

            var reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));

            // 指定数据传输类型

            reqFTP.UseBinary = true;

            // ftp用户名和密码

            reqFTP.Credentials = new NetworkCredential(uid, upwd);
            return reqFTP;
        }

        //删除文件
        public static bool DeleteFtpFile(string strFTPDicPath, string ftpfilename, string uid, string upwd)
        {
            try
            {
                bool isexist = CheckFTPFile(ftpfilename, strFTPDicPath, uid, upwd);
                if (!isexist)
                    return true;
                if (!strFTPDicPath.EndsWith("/"))
                {
                    strFTPDicPath += "/";
                }
                strFTPDicPath = strFTPDicPath + ftpfilename;
                var reqFTP = Connect(strFTPDicPath, uid, upwd);//连接         
                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                // 指定执行什么命令

                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// method to check the existance of a file on the server
        /// </summary>
        /// <param name="fileName">file name e.g file1.txt</param>
        /// <param name="strFTPPath">FTP server path i.e: ftp://yourserver/foldername</param>
        /// <param name="strftpUserID">username</param>
        /// <param name="strftpPassword">password</param>
        /// <returns>true (if file exists) or false</returns>
        public static bool CheckFTPFile(string fileName, string strFTPDicPath, string strftpUserID, string strftpPassword)
        {
            FtpWebRequest reqFTP;
            // dirName = name of the directory to create.
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFTPDicPath));
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(strftpUserID, strftpPassword);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader ftpStream = new StreamReader(response.GetResponseStream());

            List<string> files = new List<string>();
            string line = ftpStream.ReadLine();
            while (line != null)
            {
                files.Add(line);
                line = ftpStream.ReadLine();
            }
            ftpStream.Close();
            response.Close();
            return files.Contains(fileName);
        }







        /// <summary>

        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串

        /// </summary>

        /// <param name="CnStr">汉字字符串</param>

        /// <returns>相对应的汉语拼音首字母串</returns>

        public static string GetSpellCode(string CnStr)
        {

            string strTemp = "";

            int iLen = CnStr.Length;

            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {

                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));
            }

            return strTemp;

        }

        /// <summary>

        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母

        /// </summary>

        /// <param name="CnChar">单个汉字</param>

        /// <returns>单个大写字母</returns>

        private static string GetCharSpellCode(string CnChar)
        {

            long iCnChar;

            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回首字母

            if (ZW.Length == 1)
            {

                return CutString(CnChar.ToUpper(), 1);

            }
            else
            {

                // get the array of byte from the single char

                int i1 = (short)(ZW[0]);

                int i2 = (short)(ZW[1]);

                iCnChar = i1 * 256 + i2;

            }

            // iCnChar match the constant

            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {

                return "A";

            }

            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {

                return "B";

            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {

                return "C";

            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {

                return "D";

            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {

                return "E";

            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {

                return "F";

            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {

                return "G";

            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {

                return "H";

            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {

                return "J";

            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {

                return "K";

            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {

                return "L";

            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {

                return "M";

            }
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {

                return "N";

            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {

                return "O";

            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {

                return "P";

            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {

                return "Q";

            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {

                return "R";

            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {

                return "S";

            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {

                return "T";

            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {

                return "W";

            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {

                return "X";

            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {

                return "Y";

            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {

                return "Z";

            }
            else

                return ("?");

        }


        #region 截取字符长度 static string CutString(string str, int len)
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="str">被截取的字符串</param>
        /// <param name="len">所截取的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int len)
        {
            if (str == null || str.Length == 0 || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;


            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// //截取字符串中文 字母
        /// </summary>
        /// <param name="content">源字符串</param>
        /// <param name="length">截取长度！</param>
        /// <returns></returns>
        public static string SubTrueString(object content, int length)
        {
            string strContent = content.ToString();// NoHTML(content.ToString());

            bool isConvert = false;
            int splitLength = 0;
            int currLength = 0;
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            for (int i = 0; i < strContent.Length; i++)
            {
                code = Char.ConvertToUtf32(strContent, i);
                if (code >= chfrom && code <= chend)
                {
                    currLength += 2; //中文
                }
                else
                {
                    currLength += 1;//非中文
                }
                splitLength = i + 1;
                if (currLength >= length)
                {
                    isConvert = true;
                    break;
                }
            }
            if (isConvert)
            {
                return strContent.Substring(0, splitLength);
            }
            else
            {
                return strContent;
            }
        }

        public static int GetStringLenth(object content)
        {
            string strContent = content.ToString();// NoHTML(content.ToString());
            int currLength = 0;
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            for (int i = 0; i < strContent.Length; i++)
            {
                code = Char.ConvertToUtf32(strContent, i);
                if (code >= chfrom && code <= chend)
                {
                    currLength += 2; //中文
                }
                else
                {
                    currLength += 1;//非中文
                }

            }
            return currLength;
        }

        #endregion
    }




}
