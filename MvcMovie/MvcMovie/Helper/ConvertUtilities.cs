using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace MvcMovie.Helper
{
    public static class ConvertUtilities
    {
        //
        // 摘要:
        //     Json序列化
        //
        // 參數:
        //   values:
        //     物件
        //
        // 類型參數:
        //   T:
        //     Model
        public static string JsonSerialize<T>(T values)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(values);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return result;
        }

        //
        // 摘要:
        //     Json序列化
        //
        // 參數:
        //   values:
        //     物件
        //
        //   settings:
        //     序列化設定
        //
        // 類型參數:
        //   T:
        //     Model
        public static string JsonSerialize<T>(T values, JsonSerializerSettings settings)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(values, settings);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return result;
        }

        //
        // 摘要:
        //     Json反序列化
        //
        // 參數:
        //   values:
        //     JsonString
        //
        // 類型參數:
        //   T:
        //     Model
        public static T JsonDeserialize<T>(string values) where T : class, new()
        {
            T result = new T();
            try
            {
                result = JsonConvert.DeserializeObject<T>(values);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return result;
        }

        //
        // 摘要:
        //     Json反序列化
        //
        // 參數:
        //   values:
        //     JsonString
        //
        //   settings:
        //     序列化設定
        //
        // 類型參數:
        //   T:
        //     Model
        public static T JsonDeserialize<T>(string values, JsonSerializerSettings settings) where T : class, new()
        {
            T result = new T();
            try
            {
                result = JsonConvert.DeserializeObject<T>(values, settings);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return result;
        }

        //
        // 摘要:
        //     Json反序列化
        //
        // 參數:
        //   values:
        //     JsonString
        //
        //   type:
        //     類型
        public static object JsonDeserializeObject(string values, Type type)
        {
            return JsonConvert.DeserializeObject(values, type);
        }

        //
        // 摘要:
        //     Json反序列化
        //
        // 參數:
        //   values:
        //     JsonString
        //
        //   type:
        //     類型
        //
        //   settings:
        //     序列化設定
        public static object JsonDeserializeObject(string values, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.DeserializeObject(values, type, settings);
        }

        //
        // 摘要:
        //     轉換日期格式(yyyy/MM/dd HH:mm:ss)
        public static string ConvertDatetimetoString(this DateTime datetime)
        {
            return $"{datetime:yyyy/MM/dd HH:mm:ss}";
        }

        //
        // 摘要:
        //     轉換日期格式(yyyy/MM/dd)
        public static string ConvertToShortDateString(this DateTime datetime)
        {
            return $"{datetime:yyyy/MM/dd}";
        }

        //
        // 摘要:
        //     轉換日期格式(hh:mm:ss)
        public static string ConvertToShortTimeString(this DateTime datetime)
        {
            return $"{datetime:hh:mm:ss}";
        }

        //
        // 摘要:
        //     Datetime to ToLongDateString
        public static string ConvertToLongDateString(this DateTime datetime)
        {
            return $"{datetime:D}";
        }

        //
        // 摘要:
        //     轉換日期格式(HH:mm:ss)
        public static string ConvertToLongTimeString(this DateTime datetime)
        {
            return $"{datetime:HH:mm:ss}";
        }

        //
        // 摘要:
        //     轉換日期格式(yyyyMMdd)
        public static string ConvertToDate(this DateTime datetime)
        {
            return $"{datetime:yyyyMMdd}";
        }

        //
        // 摘要:
        //     轉換日期格式(yyyyMMddHHmmss)
        public static string ConvertToDateTime(this DateTime datetime)
        {
            return $"{datetime:yyyyMMddHHmmss}";
        }

        //
        // 摘要:
        //     將DateTime轉換成西元日期字串
        public static string ConvertDateTime(this string datetime)
        {
            CultureInfo cultureInfo = new CultureInfo("zh-TW");
            cultureInfo.DateTimeFormat.Calendar = new TaiwanCalendar();
            return DateTime.Parse(datetime, cultureInfo).ToShortDateString();
        }

        //
        // 摘要:
        //     無分隔號日期字串轉換有分隔號的日期字串 ex.yyyy/MM/dd HH:mm:ss
        public static string ConvertDateTimeString(this string datetime)
        {
            if (datetime != null && datetime.Length >= 14)
            {
                return datetime.Substring(0, 4) + "/" + datetime.Substring(4, 2) + "/" + datetime.Substring(6, 2) + " " + datetime.Substring(8, 2) + ":" + datetime.Substring(10, 2) + ":" + datetime.Substring(12);
            }

            if (datetime != null && datetime.Length == 8)
            {
                return datetime.Substring(0, 4) + "/" + datetime.Substring(4, 2) + "/" + datetime.Substring(6, 2);
            }

            return datetime;
        }

        //
        // 摘要:
        //     有分隔號日期字串轉換無分隔號的日期字串 ex.yyyyMMddHHmmss
        public static string ConvertDateTimeNoSignString(this string datetime)
        {
            datetime = datetime.Trim();
            if (datetime != null && datetime.Length >= 16)
            {
                return datetime.Substring(0, 4) + datetime.Substring(5, 2) + datetime.Substring(8, 2) + datetime.Substring(11, 2) + datetime.Substring(14, 2) + datetime.Substring(17);
            }

            return datetime;
        }

        public static string LastDayOfMonth(this DateTime datetime)
        {
            DateTime date = datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1.0)
                .Date;
            return date.ConvertToShortDateString();
        }

        //
        // 摘要:
        //     取得日期字串
        //
        // 參數:
        //   DateTime:
        //
        //   PadLeft:
        public static string ParseDayString(this DateTime DateTime, int PadLeft = 2)
        {
            return DateTime.Date.ToString().PadLeft(PadLeft, '0');
        }

        //
        // 摘要:
        //     轉換日期字串為INT64
        //
        // 參數:
        //   DateTime:
        //
        //   PadLeft:
        public static long ConvertDatetimeToLong(this DateTime DateTime)
        {
            return Convert.ToInt64(DateTime.ToString("yyyyMMddHHmmss"));
        }

        //
        // 摘要:
        //     轉換日期格式(yyyyMMddHHmmssfff)
        public static string ConvertToLongDateTime(this DateTime datetime)
        {
            return $"{datetime:yyyyMMddHHmmssfff}";
        }

        //
        // 摘要:
        //     XML檔反序列
        //
        // 類型參數:
        //   data:
        //     Model
        public static string XmlSerialize<T>(T data) where T : class
        {
            string empty = string.Empty;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
                StringBuilder stringBuilder = new StringBuilder();
                StringWriter textWriter = new StringWriter(stringBuilder);
                xmlSerializer.Serialize(textWriter, data);
                empty = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                empty = ex.Message;
            }

            return empty.ToString();
        }

        //
        // 摘要:
        //     XML檔反序列
        //
        // 參數:
        //   xmlfilePath:
        //     XML檔路徑
        //
        // 類型參數:
        //   T:
        //     Model
        public static T XmlDeserialize<T>(string xmlfilePath) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (TextReader textReader = new StringReader(xmlfilePath))
            {
                return xmlSerializer.Deserialize(textReader) as T;
            }
        }

        //
        // 摘要:
        //     轉Int16
        //
        // 參數:
        //   data:
        //     資料
        public static short ConvertInt16(this object data)
        {
            if (short.TryParse(data.ToString(), out short result))
            {
                return result;
            }

            throw new Exception();
        }

        //
        // 摘要:
        //     轉Int32
        //
        // 參數:
        //   data:
        //     資料
        public static int ConvertInt32(this object data)
        {
            if (int.TryParse(data.ToString(), out int result))
            {
                return result;
            }

            throw new Exception();
        }

        //
        // 摘要:
        //     轉Int64
        //
        // 參數:
        //   data:
        //     資料
        public static long ConvertInt64(this object data)
        {
            if (long.TryParse(data.ToString(), out long result))
            {
                return result;
            }

            throw new Exception();
        }

        //
        // 摘要:
        //     物件清單轉不同namespace
        //
        // 參數:
        //   model:
        //     轉入物件
        //
        // 類型參數:
        //   T:
        //     回傳物件
        public static List<T> ModelListConvert<T>(this IEnumerable<object> model) where T : class, new()
        {
            List<T> list = new List<T>();
            Type typeFromHandle = typeof(T);
            List<PropertyInfo> list2 = typeFromHandle.GetProperties().ToList();
            foreach (object item in model)
            {
                T val = new T();
                foreach (PropertyInfo item2 in list2)
                {
                    PropertyInfo property = item.GetType().GetProperty(item2.Name);
                    if (property != null)
                    {
                        item2.SetValue(val, property.GetValue(item, null), null);
                    }
                }

                list.Add(val);
            }

            return list;
        }

        //
        // 摘要:
        //     物件清單轉不同namespace
        //
        // 參數:
        //   model:
        //     轉入物件
        //
        // 類型參數:
        //   T:
        //     回傳物件
        public static T ModelConvert<T>(this object item) where T : class, new()
        {
            T val = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                PropertyInfo property = item.GetType().GetProperty(propertyInfo.Name);
                if (property != null)
                {
                    propertyInfo.SetValue(val, property.GetValue(item, null), null);
                }
            }

            return val;
        }
    }
}
