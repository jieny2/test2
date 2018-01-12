using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class DateTimeExtensions
    {
        public static bool IsMinValue(this DateTime source)
        {
            return source.Equals(DateTime.MinValue) ? true : false;
        }

        public static bool IsMinValue(this Nullable<DateTime> source)
        {
            return IsMinValue(source.Value);
        }

        #region 获取DateTime对象中的部分内容或其它与日期相关的信息
        /// <summary>
        /// 获取此实例所表示的日期是星期几，星期一至日分别为1-7
        /// </summary>
        public static int GetWeekDay(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            if (source.DayOfWeek == DayOfWeek.Sunday)
            {
                return 7;
            }

            return (int)source.DayOfWeek;
        }

        /// <summary>
        /// 获取此实例所表示的日期是星期几，星期一至日分别为1-7
        /// </summary>
        public static int GetWeekDay(this Nullable<DateTime> source)
        {
            return GetWeekDay(source.Value);
        }

        /// <summary>
        /// 获取此实例所表示的日期是星期几（中文）
        /// </summary>
        public static string GetChineseWeekDay(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            string[] strArray = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

            return strArray[(int)source.DayOfWeek];
        }

        /// <summary>
        /// 获取此实例所表示的日期是星期几（中文）
        /// </summary>
        public static string GetChineseWeekDay(this Nullable<DateTime> source)
        {
            return GetChineseWeekDay(source.Value);
        }

        /// <summary>
        /// 获取此实例所表示的日期所在季度(1、2、3、4)
        /// </summary>
        public static int GetQuarter(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            return (source.Month + 2) / 3;
        }

        /// <summary>
        /// 获取此实例所表示的日期所在季度(1、2、3、4)
        /// </summary>
        public static int GetQuarter(this Nullable<DateTime> source)
        {
            return GetQuarter(source.Value);
        }

        /// <summary>
        /// 1-7分别为年月日时分秒毫秒，8为星期几（1-7），9为第几季度
        /// </summary>
        public static string DatePart(this DateTime source, int type)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            string val = string.Empty;
            switch (type)
            {
                case 1:
                    val = source.Year.ToString();
                    break;
                case 2:
                    val = source.Month.ToString();
                    break;
                case 3:
                    val = source.Day.ToString();
                    break;
                case 4:
                    val = source.Hour.ToString();
                    break;
                case 5:
                    val = source.Minute.ToString();
                    break;
                case 6:
                    val = source.Second.ToString();
                    break;
                case 7:
                    val = source.Millisecond.ToString();
                    break;
                case 8:
                    val = GetWeekDay(source).ToString();
                    break;
                case 9:
                    val = GetQuarter(source).ToString();
                    break;
                default:
                    break;
            }

            return val;
        }

        /// <summary>
        /// 1-7分别为年月日时分秒毫秒，8为星期几（1-7），9为第几季度
        /// </summary>
        public static string DatePart(this Nullable<DateTime> source, int type)
        {
            return DatePart(source.Value, type);
        }
        #endregion

        #region 以下均返回DateTime
        #region 周一相关
        /// <summary>
        /// 根据日期获取当前周周一的DateTime
        /// </summary>
        public static DateTime GetCurWeekMonday(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            if (source.DayOfWeek == DayOfWeek.Monday)
            {
                return source;
            }
            else
            {
                return source.AddDays(1 - source.GetWeekDay());
            }
        }

        /// <summary>
        /// 根据日期获取当前周周一的DateTime
        /// </summary>
        public static DateTime GetCurWeekMonday(this Nullable<DateTime> source)
        {
            return GetCurWeekMonday(source.Value);
        }

        /// <summary>
        /// 根据日期获取上周一的DateTime
        /// </summary>
        public static DateTime GetPreWeekMonday(this DateTime source)
        {
            return GetCurWeekMonday(source).AddDays(-7);
        }

        /// <summary>
        /// 根据日期获取上周一的DateTime
        /// </summary>
        public static DateTime GetPreWeekMonday(this Nullable<DateTime> source)
        {
            return GetPreWeekMonday(source.Value);
        }

        /// <summary>
        /// 根据日期获取下周一的DateTime
        /// </summary>
        public static DateTime GetNextWeekMonday(this DateTime source)
        {
            return GetCurWeekMonday(source).AddDays(7);
        }

        /// <summary>
        /// 根据日期获取下周一的DateTime
        /// </summary>
        public static DateTime GetNextWeekMonday(this Nullable<DateTime> source)
        {
            return GetNextWeekMonday(source.Value);
        }
        #endregion

        #region 周日相关
        /// <summary>
        /// 根据日期获取当前周周日的DateTime
        /// </summary>
        public static DateTime GetCurWeekSunday(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            if (source.DayOfWeek == DayOfWeek.Sunday)
            {
                return source;
            }
            else
            {
                return source.AddDays(7 - source.GetWeekDay());
            }
        }

        /// <summary>
        /// 根据日期获取当前周周日的DateTime
        /// </summary>
        public static DateTime GetCurWeekSunday(this Nullable<DateTime> source)
        {
            return GetCurWeekSunday(source.Value);
        }

        /// <summary>
        /// 根据日期获取上周周日的DateTime
        /// </summary>
        public static DateTime GetPreWeekSunday(this DateTime source)
        {
            return GetCurWeekSunday(source).AddDays(-7);
        }

        /// <summary>
        /// 根据日期获取上周周日的DateTime
        /// </summary>
        public static DateTime GetPreWeekSunday(this Nullable<DateTime> source)
        {
            return GetPreWeekSunday(source.Value);
        }

        /// <summary>
        /// 根据日期获取下周周日的DateTime
        /// </summary>
        public static DateTime GetNextWeekSunday(this DateTime source)
        {
            return GetCurWeekSunday(source).AddDays(7);
        }

        /// <summary>
        /// 根据日期获取下周周日的DateTime
        /// </summary>
        public static DateTime GetNextWeekSunday(this Nullable<DateTime> source)
        {
            return GetNextWeekSunday(source.Value);
        }
        #endregion

        #region 月初
        /// <summary>
        /// 根据日期获取当月第一天的DateTime
        /// </summary>
        public static DateTime GetCurMonthFirst(this DateTime source)
        {
            if (source.IsMinValue())
            {
                throw new ArgumentException("该时间对象为0001/1/1 0:00:00，可能有错误");
            }

            if (source.Day == 1)
            {
                return source;
            }
            else
            {
                return source.AddDays(1 - source.Day);
                // 另外一种方法
                //return source.ToString("yyyy-MM-01").ToDateTime();
            }
        }

        /// <summary>
        /// 根据日期获取当月第一天的DateTime
        /// </summary>
        public static DateTime GetCurMonthFirst(this Nullable<DateTime> source)
        {
            return GetCurMonthFirst(source.Value);
        }
        #endregion

        #region 月末
        /// <summary>
        /// 根据日期获取当月最后一天的DateTime
        /// </summary>
        public static DateTime GetCurMonthLast(this DateTime source)
        {
            return GetCurMonthFirst(source).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 根据日期获取当月最后一天的DateTime
        /// </summary>
        public static DateTime GetCurMonthLast(this Nullable<DateTime> source)
        {
            return GetCurMonthLast(source.Value);
        }
        #endregion

        #region 季度初
        /// <summary>
        /// 根据日期获取当前季度第一天的DateTime
        /// </summary>
        public static DateTime GetCurQuarterBegin(this DateTime source)
        {
            List<int> quarterBeginMonth = new List<int> { 1, 4, 7, 10 };
            if (quarterBeginMonth.Contains(source.Month))
            {
                return GetCurMonthFirst(source);
            }
            else
            {
                int month = quarterBeginMonth[source.GetQuarter() - 1];
                return new DateTime(source.Year, month, 1);
            }
        }

        /// <summary>
        /// 根据日期获取当前季度第一天的DateTime
        /// </summary>
        public static DateTime GetCurQuarterBegin(this Nullable<DateTime> source)
        {
            return GetCurQuarterBegin(source.Value);
        }
        #endregion

        #region 季度末
        /// <summary>
        /// 根据日期获取当前季度最后一天的DateTime
        /// </summary>
        public static DateTime GetCurQuarterEnd(this DateTime source)
        {
            return GetCurQuarterBegin(source).AddMonths(3).AddDays(-1);
        }

        /// <summary>
        /// 根据日期获取当前季度最后一天的DateTime
        /// </summary>
        public static DateTime GetCurQuarterEnd(this Nullable<DateTime> source)
        {
            return GetCurQuarterEnd(source.Value);
        }
        #endregion
        #endregion

        #region 类型转换
        #region Integer类型转换
        public static int ToInt(this DateTime source, int length = 8)
        {
            if (!(length > 0 && length <= 8))
            {
                throw new ArgumentException("参数length的值必须在[1,8]区间内");
            }

            int result;
            Int32.TryParse(source.ToString("yyyyMMdd").Left(length), out result);
            if (result == 0)
            {
                throw new Exception("DateTime转Int32失败");
            }

            return result;
        }

        public static int ToInt(this Nullable<DateTime> source, int length = 8)
        {
            return ToInt(source.Value, length);
        }

        public static int? ToIntNullable(this DateTime source, int length = 8)
        {
            if (!(length > 0 && length <= 8))
            {
                throw new ArgumentException("参数length的值必须在[1,8]区间内");
            }

            int result;
            Int32.TryParse(source.ToString("yyyyMMdd").Left(length), out result);

            return result == 0 ? (int?)null : result;
        }

        public static int? ToIntNullable(this Nullable<DateTime> source, int length = 8)
        {
            return ToIntNullable(source.Value, length);
        }
        #endregion

        #region long（Int64）类型转换
        public static long ToLongInt(this DateTime source, int length = 14)
        {
            if (!(length > 0 && length <= 17))
            {
                throw new ArgumentException("参数length的值必须在[1,17]区间内");
            }

            long result;
            Int64.TryParse(source.ToString("yyyyMMddHHmmssfff").Left(length), out result);
            if (result == 0)
            {
                throw new Exception("DateTime转Int64失败");
            }

            return result;
        }

        public static long ToLongInt(this Nullable<DateTime> source, int length = 14)
        {
            return ToLongInt(source.Value, length);
        }

        public static long? ToLongIntNullable(this DateTime source, int length = 14)
        {
            if (!(length > 0 && length <= 17))
            {
                throw new ArgumentException("参数length的值必须在[1,17]区间内");
            }

            long result;
            Int64.TryParse(source.ToString("yyyyMMddHHmmssfff").Left(length), out result);

            return result == 0 ? (long?)null : result;
        }

        public static long? ToLongIntNullable(this Nullable<DateTime> source, int length = 14)
        {
            return ToLongIntNullable(source.Value, length);
        }
        #endregion
        #endregion

        #region 修改DateTime对象的内容
        /// <summary>
        /// 设置时间部分的值，时分秒
        /// </summary>
        public static DateTime SetTime(this DateTime source, int hour = 23, int minute = 59, int second = 59)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentException("小时参数错误");
            }
            if (hour < 0 || hour > 59)
            {
                throw new ArgumentException("分钟参数错误");
            }
            if (hour < 0 || hour > 59)
            {
                throw new ArgumentException("秒参数错误");
            }

            return source.ToString("yyyy-MM-dd " + Convert.ToString(hour) + ":" + Convert.ToString(minute) + ":" + Convert.ToString(second)).ToDateTime();
        }
        #endregion
    }
}
