using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Common
{
    public class Tools
    {
        /// <summary>
        /// 字节数组转十六进制字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="isUpperCase">是否大写</param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] byteArray, bool isUpperCase)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (isUpperCase)
                {
                    sb.Append(byteArray[i].ToString("X2"));
                }
                else
                {
                    sb.Append(byteArray[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 字节数组比对
        /// </summary>
        /// <param name="array1">byte数组1</param>
        /// <param name="array2">byte数组2</param>
        /// <returns>成功返回true，否则返回false</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1 != null && array1.Length > 0 && array1.Length == array2.Length)
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>字符串</returns>
        public static string ErrorHandler(Exception ex)
        {
            string str = string.Empty;
            if (ex.GetType().FullName == "System.Data.EntityCommandExecutionException")
            {
                if (ex.StackTrace.LastIndexOf(":") > 0)
                {
                    str = "访问数据库出错:" + (ex.StackTrace == null ? "" : ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":"))) + ";" + (ex.InnerException == null ? "" : ex.InnerException.Message);
                }
                else
                {
                    str = ex.InnerException.Message;
                }
            }
            else
            {
                if (ex.StackTrace.LastIndexOf(":") > 0)
                {
                    str = "错误" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(":")) + ";" + ex.Message;
                }
                else
                {
                    str = ex.Message;
                }
            }

            return str;
        }

        /// <summary>
        /// 表单转实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>实体</returns>
        /// <备注></备注>
        /// <备注>控件的name属性的值必须包含实体的字段名</备注>
        public static T RequestToEntity<T>() where T : new()
        {
            T entity = new T();
            try
            {
                PropertyInfo[] propertyInfos = entity.GetType().GetProperties();
                foreach (PropertyInfo pi in propertyInfos)
                {
                    //string val = HttpContext.Current.Request[pi.Name.Substring(1)]; // 如果用这个，控件的name属性的值必须包含实体的字段名（不包含第一个字符，即不包含f）
                    string val = HttpContext.Current.Request[pi.Name];
                    if (!string.IsNullOrEmpty(val))
                    {
                        // 把请求的内容转换为的entity该属性对应的类型并给entity赋值
                        pi.SetValue(entity, Convert.ChangeType(val, TypeProcess(pi)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        /// <summary>
        /// 表单转实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="nameHead">控件的name属性的值必须以此字符串开头</param>
        /// <param name="index">控件的name属性的值必须包含实体的字段名，一般填0即可</param>
        /// <returns>List<T></returns>
        /// <备注>控件的name属性的值必须符合规则且要同名（重复）</备注>
        public static List<T> RequestToEntityList<T>(string nameHead, int index) where T : new()
        {
            List<T> entityList = new List<T>();
            try
            {
                PropertyInfo[] propertyInfos = new T().GetType().GetProperties();
                //var qty = System.Web.HttpContext.Current.Request[nameHead + propertyInfos[index].Name.Substring(1)].Split(',').Length; // 同名会以逗号【，】分隔 // 如果用这个，控件的name属性的值必须包含实体的字段名（不包含第一个字符，即不包含f）
                var qty = HttpContext.Current.Request[nameHead + propertyInfos[index].Name].Split(',').Length; // 同名会以逗号【，】分隔
                for (int i = 0; i < qty; i++)
                {
                    T entity = new T();
                    foreach (var pi in propertyInfos)
                    {
                        //string[] vals = System.Web.HttpContext.Current.Request.Params.GetValues(nameHead + pi.Name.Substring(1)); // 别用逗号分割的方法，因为用户可能会填英文逗号【,】会引发错误
                        string[] vals = HttpContext.Current.Request.Params.GetValues(nameHead + pi.Name); // 别用逗号分割的方法，因为用户可能会填英文逗号【,】会引发错误
                        if (vals.Length > 0)
                        {
                            if (vals != null && !string.IsNullOrEmpty(vals[i]))
                            {
                                // 把请求的内容转换为的entity该属性对应的类型并给entity赋值
                                pi.SetValue(entity, Convert.ChangeType(vals[i], TypeProcess(pi)));
                            }
                        }
                    }
                    entityList.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entityList;
        }

        /// <summary>
        /// 表单转实体列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="nameHead">控件的name属性的值必须以此字符串开头</param>
        /// <param name="index">控件的name属性的值必须包含实体的字段名，一般填0或1即可</param>
        /// <returns>List<T></returns>
        /// <备注>控件的name属性的值必须符合规则且要序号要正确，从0开始，如fName0</备注>
        public static List<T> RequestToEntityListDifferentName<T>(string nameHead, int index) where T : new()
        {
            List<T> entityList = new List<T>();
            try
            {
                PropertyInfo[] propertyInfos = new T().GetType().GetProperties();
                int qty = 0;
                foreach (string item in HttpContext.Current.Request.Form.Keys)
                {
                    if (item.Substring(0, item.Length - 1) == (nameHead + propertyInfos[index].Name))
                    {
                        qty++;
                    }
                }
                for (int i = 0; i < qty; i++)
                {
                    T entity = new T();
                    foreach (var pi in propertyInfos)
                    {
                        string val = HttpContext.Current.Request.Form[nameHead + pi.Name + i.ToString()];
                        if (val != null && !string.IsNullOrEmpty(val))
                        {
                            // 把请求的内容转换为的entity该属性对应的类型并给entity赋值
                            pi.SetValue(entity, Convert.ChangeType(val, TypeProcess(pi)));
                        }
                    }
                    entityList.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entityList;
        }

        /// <summary>
        /// 表单转实体列表，表单中传上来的如img[0][fName]
        /// </summary>
        public static List<T> RequestToEntityListArrayName<T>(string nameHead, int index) where T : new()
        {
            List<T> entityList = new List<T>();
            try
            {
                PropertyInfo[] propertyInfos = new T().GetType().GetProperties();
                int qty = 0;
                foreach (string item in HttpContext.Current.Request.Form.Keys)
                {
                    if (item.Contains(nameHead + qty + "][" + propertyInfos[index].Name + "]"))
                    {
                        qty++;
                    }
                }
                for (int i = 0; i < qty; i++)
                {
                    T entity = new T();
                    foreach (var pi in propertyInfos)
                    {
                        string val = HttpContext.Current.Request.Form[nameHead + i.ToString() + "][" + pi.Name + "]"];
                        if (val != null && !string.IsNullOrEmpty(val))
                        {
                            // 把请求的内容转换为的entity该属性对应的类型并给entity赋值
                            pi.SetValue(entity, Convert.ChangeType(val, TypeProcess(pi)));
                        }
                    }
                    entityList.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entityList;
        }

        /// <summary>
        /// 可空类型的强制转换处理
        /// </summary>
        /// <param name="pi">PropertyInfo的实例</param>
        /// <returns>Type</returns>
        private static Type TypeProcess(PropertyInfo pi)
        {
            Type columnType;
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                columnType = pi.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                columnType = pi.PropertyType;
            }

            return columnType;
        }
    }
}
