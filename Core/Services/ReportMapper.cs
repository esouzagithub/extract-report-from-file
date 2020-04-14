using System;
using System.Globalization;
using System.Reflection;
using Core.Attributes;
using Core.Reports.Interfaces;

namespace Core.Services
{
    internal static class ReportMapper
    {
        public static T Reader<T>(string line) where T : IReport
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            var properties = obj.GetType().GetTypeInfo().GetProperties();

            foreach (var propertyInfo in properties)
            {
                var customAttribute = propertyInfo.GetCustomAttribute<ReportMapperAttribute>();

                if (customAttribute == null)
                {
                    continue;
                }

                var value = line.Substring(customAttribute.StartIndex, customAttribute.Length).Trim();

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                propertyInfo.SetValue(obj, ConvertType(propertyInfo, value), null);

            }

            return obj;
        }

        private static object ConvertType(PropertyInfo propertyInfo, string value)
        {
            if (propertyInfo.PropertyType == typeof(long))
            {
                long.TryParse(value,
                    NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture,
                    out var result);

                return result;
            }

            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                DateTime.TryParse(value, out var result);
                return result;
            }

            return Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture);
        }
    }
}