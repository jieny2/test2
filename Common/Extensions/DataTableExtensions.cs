using Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace System.Data
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// DataTable转entityList，注意实体类要和DataTable列对应，否则容易造成转换成功但内容错误的bug
        /// </summary>
        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            if (dt.Columns.Count > propertyInfos.Length)
            {
                string errorStr = "DataTable的列数大于实体类的字段数，转换失败！位置为【DataTableExtensions.cs】第24行附近";
                LogHelper.WriteLog(LogLevel.Error, errorStr);
                throw new Exception(errorStr);
            }
            bool flag = true;
            string[] strArr = new string[] { "ID", "Id", "id", "fID", "fId", "fid" };

            foreach (DataRow dr in dt.Rows)
            {
                T entity = new T();
                int i = 0; // 记录DataTable和实体类除了ID之外有多少个字段是一样的
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        // 不在strArr中的则自增，过滤掉这些典型的字段
                        if (!strArr.Contains(pi.Name))
                        {
                            i++;
                        }

                        // 判断此属性是否有Set方法
                        if (!pi.CanWrite)
                        {
                            continue;
                        }

                        object val = dr[pi.Name];
                        if (val != DBNull.Value)
                        {
                            pi.SetValue(entity, val, null);
                        }
                    }
                    else if (flag)
                    {
                        string infoStr = string.Format("DataTable的列没有实体类【{0}】的【{1}】字段，位置为【DataTableExtensions.cs】第59行附近", entity.GetType().Name, pi.Name);
                        LogHelper.WriteLog(LogLevel.Info, infoStr);
                    }
                }
                flag = false;

                if (i > 1)
                {
                    list.Add(entity);
                }
                else if (i == 1)
                {
                    // 除ID字段外，只有一个字段一样
                    throw new Exception("DataTable与实体的关系不是很好，转换失败！");
                }
                else
                {
                    throw new Exception("DataTable不包含实体的任何字段，两者没有半毛钱关系，转换失败！");
                }
            }

            return list;
        }

        public static int?[] ToArrayInt(this DataTable dt, string columnName)
        {
            int count = dt.Rows.Count;
            int?[] arr = new int?[count];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                //arr[count - 1] = (int?)dr[columnName];
                //count--;
                arr[i] = dr[columnName] == DBNull.Value ? null : (int?)dr[columnName];
                i++;
            }

            return arr;
        }

        public static string[] ToArrayString(this DataTable dt, string columnName)
        {
            int count = dt.Rows.Count;
            string[] arr = new string[count];
            int i = 0;

            foreach (DataRow dr in dt.Rows)
            {
                //arr[count - 1] = dr[columnName].ToString();
                //count--;
                arr[i] = dr[columnName].ToString();
                i++;
            }

            return arr;
        }

        /// <summary>
        /// 根据DataTable生成实体类到网站的GenerateClass目录下，并返回该类的内容字符串
        /// 属性重名会在后面加序号，如ID、ID1、ID2 ...
        /// 无列名的默认为Column1，默认类型为int且可以为NULL
        /// </summary>
        public static string GenerateClass(this DataTable dt, string className)
        {
            StringBuilder res = new StringBuilder();
            StringBuilder propertiesValue = new StringBuilder();
            bool isFirst = true;
            foreach (DataColumn dc in dt.Columns)
            {
                if (!isFirst)
                {
                    propertiesValue.AppendLine(Environment.NewLine);
                }
                else
                {
                    isFirst = false;
                }
                string typeName = DataTypeConvertToAliases(dc.DataType.Name);
                string isAllowDBNull = (dc.AllowDBNull && !dc.DataType.Name.Equals("String") && !dc.DataType.Name.Contains("[")) ? "?" : string.Empty;
                propertiesValue.AppendFormat("        public {0}{1} {2} {3}", typeName, isAllowDBNull, dc.ColumnName, "{ get; set; }");
            }
            res.Append("using System;");
            res.AppendLine();
            res.AppendFormat(@"
namespace XXX.Model
{{
    public partial class {0}
    {{
{1}
    }}
}}"
            , className, propertiesValue);
            string directory = HttpContext.Current.Server.MapPath("~/GenerateClass");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, className + ".cs");
            File.WriteAllText(path, res.ToString());

            return res.ToString();
        }

        // 内置类型表
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table
        // https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/keywords/built-in-types-table
        public static string DataTypeConvertToAliases(string dotNETFrameworkType)
        {
            string res = string.Empty;
            switch (dotNETFrameworkType)
            {
                case "Boolean":
                    res = "bool";
                    break;
                case "Byte":
                    res = "byte";
                    break;
                case "Byte[]":
                    res = "byte[]";
                    break;
                case "Char":
                    res = "char";
                    break;
                case "Decimal":
                    res = "decimal";
                    break;
                case "Double":
                    res = "double";
                    break;
                case "Int16":
                    res = "short";
                    break;
                case "Int32":
                    res = "int";
                    break;
                case "Int64":
                    res = "long";
                    break;
                case "Object":
                    res = "object";
                    break;
                case "SByte":
                    res = "sbyte";
                    break;
                case "Single":
                    res = "float";
                    break;
                case "String":
                    res = "string";
                    break;
                case "UInt16":
                    res = "ushort";
                    break;
                case "UInt32":
                    res = "uint";
                    break;
                case "UInt64":
                    res = "ulong";
                    break;
                default:
                    res = dotNETFrameworkType;
                    break;
            }

            return res;
        }
    }
}
