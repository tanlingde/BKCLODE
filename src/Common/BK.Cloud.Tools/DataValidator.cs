using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BK.Cloud.Tools
{
    /// <summary>
    /// 通过Framwork类库中的Regex类实现了一些特殊功能数据检查

    /// </summary>
    public class DataValidator
    {

        private static Regex RegNumber = new Regex("^[0-9]+$");


        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");


        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");


        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$ 


        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样  


        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        private static DataValidator instance = null;
        public static DataValidator Instance
        {
            get { return DataValidator.instance ?? (DataValidator.instance = new DataValidator()); }
        }
        private DataValidator()
        {
        }
        /// <summary>
        /// 判断输入的字符串只包含汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseCh(string input)
        {
            Match m = RegCHZN.Match(input);
            return m.Success;
        }
        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，

        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，

        /// 也可以没有间隔

        /// \(0\d{2}\)[- ]?\d{8}|0\d{2}[- ]?\d{8}|\(0\d{3}\)[- ]?\d{7}|0\d{3}[- ]?\d{7}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(string input)
        {
            string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号

        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1[3458]\\d{9}$");
            return regex.IsMatch(input);

        }


        /**/
        /// <summary>
        /// 验证日期
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string source)
        {
            try
            {
                DateTime time = Convert.ToDateTime(source);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 非空验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNull(string input)
        {
            Regex regex = new Regex("^\\S+$");
            return regex.IsMatch(input);
        }
        /**/
        /// <summary>
        /// 验证身份证是否有效

        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool IsIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = IsIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = IsIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        public static bool IsIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证

            }
            return true;//符合GB11643-1999标准
        }

        public static bool IsIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }


        /// <summary>
        /// 判断输入的字符串只包含数字

        /// 可以匹配整数和浮点数
        /// ^-?\d+$|^(-?\d+)(\.\d+)?$
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /**/
        /// <summary>
        /// 验证是不是正常字符 字母，数字，下划线的组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string source)
        {
            return Regex.IsMatch(source, @"[\w\d_]+", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 匹配非负整数
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNagtive(string input)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 匹配正整数

        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUint(string input)
        {
            Regex regex = new Regex("^[0-9]*[1-9][0-9]*$");
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 判断输入的字符串字包含英文字母

        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglisCh(string input)
        {
            Regex regex = new Regex("^[A-Za-z]+$");
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是一个合法的Email地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是一个超链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsURL(string input)
        {
            //string pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            string pattern = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        /// <summary>
        /// 判断输入的字符串是否是表示一个IP地址
        /// </summary>
        /// <param name="input">被比较的字符串</param>
        /// <returns>是IP地址则为Tr </returns>
        public static bool IsIPv4(string input)
        {

            string[] IPs = input.Split('.');
            Regex regex = new Regex(@"^\d+$");
            for (int i = 0; i < IPs.Length; i++)
            {
                if (!regex.IsMatch(IPs[i]))
                {
                    return false;
                }
                if (Convert.ToUInt16(IPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 计算字符串的字符长度，一个汉字字符将被计算为两个字符
        /// </summary>
        /// <param name="input">需要计算的字符串</param>
        /// <returns>返回字符串的长度</returns>
        public static int GetCount(string input)
        {
            return Regex.Replace(input, @"[\一-\龥/g]", "aa").Length;
        }

        /// <summary>
        /// 调用Regex中IsMatch函数实现一般的正则表达式匹配

        /// </summary>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="input">要搜索匹配项的字符串</param>
        /// <returns>如果正则表达式找到匹配项，则为 tr ；否则，为 false。</returns>
        public static bool IsMatch(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        /// <summary>
        /// 从输入字符串中的第一个字符开始，用替换字符串替换指定的正则表达式模式的所有匹配项。

        /// </summary>
        /// <param name="pattern">模式字符串</param>
        /// <param name="input">输入字符串</param>
        /// <param name="replacement">用于替换的字符串</param>
        /// <returns>返回被替换后的结果</returns>
        public static string Replace(string pattern, string input, string replacement)
        {
            Regex regex = new Regex(pattern);
            return regex.Replace(input, replacement);
        }

        /// <summary>
        /// 在由正则表达式模式定义的位置拆分输入字符串。

        /// </summary>
        /// <param name="pattern">模式字符串</param>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string[] Split(string pattern, string input)
        {
            Regex regex = new Regex(pattern);
            return regex.Split(input);
        }

        /// <summary> 
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制 
        /// </summary> 
        /// <param name="req">Request</param> 
        /// <param name="inputKey">Request的键值</param> 
        /// <param name="maxLen">最大长度</param> 
        /// <returns>返回Request查询字符串。不是数字,返回String.Empty</returns> 
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (!string.IsNullOrEmpty(inputKey))
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }

        /// <summary> 
        /// 检测字符串类型 
        /// </summary> 
        /// <param name="inputData">输入字符串</param> 
        /// <param name="checktype">0:不检测| 1:数字| 2:符号数字| 3: 浮点数| 4:符号浮点| 5: 中文?| 6:邮件?</param> 
        /// <returns></returns> 
        public static bool CheckString(string inputData, int checktype)
        {


            bool _return = false;


            switch (checktype)
            {


                case 0:


                    _return = true;


                    break;


                case 1:


                    _return = IsNumber(inputData);


                    break;


                case 2:


                    _return = IsNumberSign(inputData);


                    break;


                case 3:


                    _return = IsDecimal(inputData);


                    break;


                case 4:


                    _return = IsDecimalSign(inputData);


                    break;


                case 5:


                    _return = IsChineseCh(inputData);


                    break;


                case 6:


                    _return = IsEmail(inputData);


                    break;


                default:


                    _return = false;


                    break;


            }


            return _return;


        }



        /// <summary> 
        /// 是否数字字符串 可带正负号 
        /// </summary> 
        /// <param name="inputData">输入字符串</param> 
        /// <returns></returns> 
        public static bool IsNumberSign(string inputData)
        {


            Match m = RegNumberSign.Match(inputData);


            return m.Success;


        }


        /// <summary> 
        /// 是否是浮点数 
        /// </summary> 
        /// <param name="inputData">输入字符串</param> 
        /// <returns></returns> 
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }


        /// <summary> 


        /// 是否是浮点数 可带正负号 


        /// </summary> 


        /// <param name="inputData">输入字符串</param> 


        /// <returns></returns> 


        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }





        public static string GetShortDate(string dt)
        {
            return Convert.ToDateTime(dt).ToShortDateString();
        }











        /// <summary> 
        /// 检查字符串最大长度，返回指定长度的串 
        /// </summary> 
        /// <param name="sqlInput">输入字符串</param> 
        /// <param name="maxLength">最大长度</param> 
        /// <returns></returns>          
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (!string.IsNullOrEmpty(sqlInput))
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串 


                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;

        }

        /// <summary>
        /// 过滤字符
        /// </summary>
        public static string FilterSql(string sInput)
        {
            if (sInput == null)
                return null;
            if (sInput.Trim() == "")
                return string.Empty;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(";
            if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            {
                return null;
            }
            output = output.Replace("'", "''");
            return output;
        }

      

    }


}
