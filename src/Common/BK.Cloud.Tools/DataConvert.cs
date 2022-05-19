using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BK.Cloud.Tools
{
    public class DataConvert
    {
        /// <summary>
        /// 十进制转换为二进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToBin(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 2;
                X = X / 2;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 16进制转ASCII码
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string HexToAscii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(
                    Convert.ToString(
                        Convert.ToChar(Int32.Parse(hexString.Substring(i, 2),
                                                   System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }

            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }
            string b = new string(c);
            return b;
        }

        /// <summary>
        /// 16进制字符串转换为二进制数组 比如输入：41 42 25 得到结果：AB%
        /// </summary>
        /// <param name="hexstring">字符串每个字节之间都应该有空格，大多数的串口通讯资料上面的16进制都是字节之间都是用空格来分割的。</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {
            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        /// <summary>
        /// 十进制转换为八进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToOtc(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 8;
                X = X / 8;
                b = b + a * Pow(10, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 十进制转换为十六进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string DecToHex(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            int X = Convert.ToInt32(x);
            Stack a = new Stack();
            int i = 0;
            while (X > 0)
            {
                a.Push(Convert.ToString(X % 16));
                X = X / 16;
                i++;
            }
            while (a.Count != 0)
                z += ToHex(Convert.ToString(a.Pop()));
            if (string.IsNullOrEmpty(z))
            {
                z = "0";
            }
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string BinToDec(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(2, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }

        /// <summary>
        /// 二进制转换为十进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToDec(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                sb.AppendFormat("{0:D2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 二进制转换为十六进制，定长转换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="iLength"></param>
        /// <returns></returns>
        public static string BinToHex(string x, short iLength)
        {
            StringBuilder sb = new StringBuilder();
            int iCount = 0;

            iCount = x.Length / iLength;

            if (x.Length % iLength > 0)
            {
                iCount += 1;
            }

            int X = 0;

            for (int i = 0; i < iCount; i++)
            {
                if ((i + 1) * iLength > x.Length)
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, (x.Length - iLength)));
                }
                else
                {
                    X = Convert.ToInt32(x.Substring(i * iLength, iLength));
                }
                int j = 0;
                long a, b = 0;
                while (X > 0)
                {
                    a = X % 10;
                    X = X / 10;
                    b = b + a * Pow(2, j);
                    j++;
                }
                //前补0
                sb.Append(DecToHex(b.ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 八进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string OctToDec(string x)
        {
            string z = null;
            int X = Convert.ToInt32(x);
            int i = 0;
            long a, b = 0;
            while (X > 0)
            {
                a = X % 10;
                X = X / 10;
                b = b + a * Pow(8, i);
                i++;
            }
            z = Convert.ToString(b);
            return z;
        }


        /// <summary>
        /// 十六进制转换为十进制
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string HexToDec(string x)
        {
            if (string.IsNullOrEmpty(x))
            {
                return "0";
            }
            string z = null;
            Stack a = new Stack();
            int i = 0, j = 0, l = x.Length;
            long Tong = 0;
            while (i < l)
            {
                a.Push(ToDec(Convert.ToString(x[i])));
                i++;
            }
            while (a.Count != 0)
            {
                Tong = Tong + Convert.ToInt64(a.Pop()) * Pow(16, j);
                j++;
            }
            z = Convert.ToString(Tong);
            return z;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static long Pow(long x, long y)
        {
            int i = 1;
            long X = x;
            if (y == 0)
                return 1;
            while (i < y)
            {
                x = x * X;
                i++;
            }
            return x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static string ToDec(string x)
        {
            switch (x)
            {
                case "A":
                    return "10";
                case "B":
                    return "11";
                case "C":
                    return "12";
                case "D":
                    return "13";
                case "E":
                    return "14";
                case "F":
                    return "15";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static string ToHex(string x)
        {
            switch (x)
            {
                case "10":
                    return "A";
                case "11":
                    return "B";
                case "12":
                    return "C";
                case "13":
                    return "D";
                case "14":
                    return "E";
                case "15":
                    return "F";
                default:
                    return x;
            }
        }

        /// <summary>
        /// 将16进制BYTE数组转换成16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        ///  上一方法反转
        /// 将16进制字符串数组转换成16进制BYTE
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="discarded"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (Uri.IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        private static byte HexToByte(string hex)
        {
            byte tt = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return tt;
        }

        private static string DoubleFormat(Double doubleValue)
        {
            try
            {
                string doubleValueStr = doubleValue.ToString();

                int eIndex = doubleValueStr.ToUpper().IndexOf("E");
                String format = "";
                if (eIndex == -1)
                {
                    return doubleValueStr;
                }
                else
                {
                    string cardinalNumberStr = doubleValueStr.Substring(0, eIndex);
                    string exponentialStr = doubleValueStr.Substring(eIndex + 1);
                    if (exponentialStr.StartsWith("+"))
                    {
                        exponentialStr = exponentialStr.Substring(1);
                    }
                    int exponential = Int32.Parse(exponentialStr);
                    if (exponential > 0)
                    {
                        if ((cardinalNumberStr.Length - 2 - exponential) > 0)
                        {
                            format = "#.";
                            for (int i = 0; i < (cardinalNumberStr.Length - 2 - exponential); i++)
                            {
                                format += 0;
                            }
                        }
                        else
                        {
                            format = "#.0";
                        }
                    }
                    else if (exponential < 0)
                    {
                        format = "0.";

                        for (int i = 0; i < (cardinalNumberStr.Substring(cardinalNumberStr.IndexOf(".") + 1).Length - exponential); i++)
                        {
                            format += 0;
                        }
                    }
                    else
                    {
                        format = "#.0";
                    }
                    if (format.Length == 2)
                    {
                        format += 0;
                    }
                    return doubleValue.ToString(format);

                }
            }
            catch (Exception e)
            {
            }
            return "";
        }


        /// <summary>
        /// 判断输入的数是否是科学计数法。如果是的话，就会将其换算成Decimal并且返回，否则就返回false。
        /// </summary>
        /// <param name="num"></param>
        /// <param name="CompleteNum"></param>
        /// <returns></returns>
        private bool ChkNum(string num, ref decimal CompleteNum)
        {
            bool result = false;
            bool resultSymbol = num.Contains("*");
            bool result0 = num.Contains("^");
            if ((resultSymbol == true) && (result0 == true))
            { //当数字中有*和^的时候，进行下面的判断
                int IntSymbol = num.IndexOf("*");
                int Symbol0 = num.IndexOf("^");
                if (((Symbol0 - IntSymbol) == 3))
                {//当*在^前面的时候
                    string numA = num.Substring(0, IntSymbol);//截取*号前面的数字（基数）；
                    string numB = num.Substring(IntSymbol + 1, Symbol0 - IntSymbol - 1);//截取10；
                    string numC = num.Substring(Symbol0 + 1, num.Length - Symbol0 - 1);//获得幂次数
                    Regex regNum0 = new Regex(@"^(\-|\+)?\d+(\.\d+)?$");
                    Regex regNum2 = new Regex(@"^-[1-9]\d*$|^[1-9]\d*$");
                    if ((regNum0.IsMatch(numA)) && (numB == "10") && (regNum2.IsMatch(numC)))
                    {
                        decimal dcNumA;
                        decimal.TryParse(numA, out dcNumA);
                        decimal dcNumC;
                        decimal.TryParse(numC, out dcNumC);//将幂次数转换成decimal类型
                        decimal zhengshu = 10;
                        if (dcNumC > 0)
                        {//当幂次数为整数的时候
                            for (int i = 0; i < dcNumC - 1; i++)
                            {
                                zhengshu *= 10;
                            }
                        }
                        else
                        {//当幂次数为负数的时候
                            for (int i = 0; i < Math.Abs(dcNumC) + 1; i++)
                            {
                                zhengshu /= 10;
                            }
                        }
                        CompleteNum = dcNumA * zhengshu;
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrToToHexByte(string hexString)
        {

            hexString = hexString.Replace(" ", "");
            hexString = hexString.Replace("0x", "");

            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>  
        /// 取整数的某一位  
        /// </summary>  
        /// <param name="resource">要取某一位的整数</param>  
        /// <param name="mask">要取的位置索引，自右至左为0-7</param>  
        /// <returns>返回某一位的值（0或者1）</returns>  
        public static int GetBitValue(int resource, int mask)
        {
            return resource >> mask & 1;
        }


        /// <summary>  
        /// 取整数的某n位  
        /// </summary>  
        /// <param name="resource">要取某一位的整数</param>  
        /// <param name="mask">要取的位置索引，自右至左为0-7</param>  
        /// <returns>返回某一位的值（0或者1）</returns>  
        public static string GetBitValue(int resource, int mask, int length)
        {
            string val = "";
            for (var startpos = mask + length - 1; startpos >= mask; startpos--)
            {
                val += resource >> startpos & 1;
            }
            return val;
        }

        /// <summary>  
        /// 将整数的某位置为0或1  
        /// </summary>  
        /// <param name="mask">整数的某位</param>  
        /// <param name="a">整数</param>  
        /// <param name="flag">是否置1，TURE表示置1，FALSE表示置0</param>  
        /// <returns>返回修改过的值</returns>  
        public static int SetBitValue(int mask, int a, bool flag)
        {
            if (flag)
            {
                a |= (0x1 << mask);
            }
            else
            {
                a &= ~(0x1 << mask);
            }
            return a;
        }

    }
}
