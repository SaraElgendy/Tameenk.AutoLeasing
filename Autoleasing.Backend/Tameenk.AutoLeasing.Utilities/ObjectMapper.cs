using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;


namespace Tameenk.AutoLeasing.Utilities
{
    public static class ObjectMapper<T> where T : class, new()
    {
        public static T MapReaderToObjectList(SqlDataReader reader)
        {
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var item = new T();
            Type t = item.GetType();
            foreach (PropertyInfo property in t.GetProperties())
            {
                Type type = property.PropertyType;
                string readerValue = string.Empty;
                if (columns.Contains(property.Name))
                {
                    if (reader[property.Name] != DBNull.Value)
                    {
                        readerValue = reader[property.Name].ToString();
                    }
                    if (!string.IsNullOrEmpty(readerValue))
                    {
                        if (type == typeof(string))
                        {
                            property.SetValue(item, readerValue, null);
                        }
                        else if (type == typeof(int))
                        {
                            property.SetValue(item, Convert.ToInt32(readerValue), null);
                        }
                        else if (type == typeof(Int64) || type == typeof(Int64?))
                        {
                            property.SetValue(item, Convert.ToInt64(readerValue), null);
                        }
                        else if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            property.SetValue(item, Convert.ToDateTime(readerValue), null);
                        }
                        else if (type == typeof(float) || type == typeof(float?))
                        {
                            float floatVar = 0;
                            float.TryParse(readerValue, out floatVar);
                            property.SetValue(item, floatVar, null);
                        }
                        else if (type == typeof(bool) || type == typeof(bool?))
                        {
                            property.SetValue(item, Convert.ToBoolean(readerValue), null);
                        }
                        else if (type == typeof(int?) || type == typeof(int))
                        {
                            property.SetValue(item, Convert.ToInt32(readerValue), null);
                        }
                        else if (type == typeof(Decimal?) || type == typeof(Decimal))
                        {
                            property.SetValue(item, Convert.ToDecimal(readerValue), null);
                        }
                        else if (type == typeof(double) || type == typeof(double?))
                        {
                            property.SetValue(item, Convert.ToDouble(readerValue), null);
                        }
                    }
                }
            }
            return item;
        }
    }


}
