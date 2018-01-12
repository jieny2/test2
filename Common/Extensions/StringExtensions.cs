using System.Text.RegularExpressions;

namespace System.Text
{
    public static class StringExtensions
    {
        /// <summary>
        /// 是否为null或者string.Empty
        /// </summary>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 是否为非null或者非string.Empty
        /// </summary>
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 拼接字符串
        /// </summary>
        public static string ConcatString(this string source, string str)
        {
            return string.Concat(source, str);
        }

        /// <summary>
        /// 左截取，length为0则返回string.Empty
        /// </summary>
        /// <remarks>source如果为null，则报NullReferenceException异常</remarks>
        public static string Left(this string source, int length)
        {
            if (length > source.Length)
            {
                throw new ArgumentException("指定的长度大于字符串长度");
            }

            return source.Substring(0, length);
        }

        /// <summary>
        /// 右截取，length为0则返回string.Empty
        /// </summary>
        /// <remarks>source如果为null，则报NullReferenceException异常</remarks>
        public static string Right(this string source, int length)
        {
            if (length > source.Length)
            {
                throw new ArgumentException("指定的长度需大于字符串长度");
            }

            return source.Substring(source.Length - length);
        }

        #region 类型转换
        #region Integer类型转换
        public static int ToInt(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                int result;
                Int32.TryParse(source, out result);

                return result;
            }
        }

        public static int? ToIntNullable(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                int result;
                return Int32.TryParse(source, out result) ? result : (int?)null;
            }
        }
        #endregion

        #region Decimal类型转换
        public static decimal ToDecimal(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                decimal result;
                Decimal.TryParse(source, out result);

                return result;
            }
        }

        public static decimal? ToDecimalNullable(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                decimal result;
                return Decimal.TryParse(source, out result) ? result : (decimal?)null;
            }
        }
        #endregion

        #region 日期类型转换
        /// <summary>
        /// string类型日期转换成datetiem类型
        /// </summary>
        /// <returns>转换失败则返回【System.DateTime.MinValue】，0001-01-01</returns>
        public static DateTime ToDateTime(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                DateTime result;
                return DateTime.TryParse(source, out result) ? result : DateTime.MinValue;
            }
        }

        /// <summary>
        /// string类型日期转换成datetiem?类型
        /// </summary>
        /// <returns>转换失败则返回null</returns>
        public static DateTime? ToDateTimeNullable(this string source)
        {
            if (source == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                DateTime result;
                return DateTime.TryParse(source, out result) ? result : (DateTime?)null;
            }
        }
        #endregion
        #endregion

        #region 正则表达式判断
        public static string Match(this string source, string pattern)
        {
            return Regex.Match(source, pattern).Value;
        }

        public static bool IsMatch(this string source, string pattern)
        {
            return Regex.IsMatch(source, pattern);
        }

        public static bool IsNumber(this string source)
        {
            return Regex.IsMatch(source, @"^[-+]?\d+(\.\d+)?$");
        }

        public static bool IsMobile(this string source)
        {
            return Regex.IsMatch(source, @"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$");
        }

        public static bool IsPhone(this string source)
        {
            return Regex.IsMatch(source, @"^(\(\d{3,4}-)|\d{3.4}-)?\d{7,8}$");
        }

        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }
        #endregion
    }
}
