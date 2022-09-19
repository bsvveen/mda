using System.Reflection;
using System.Text.RegularExpressions;

namespace MDA.Infrastructure
{
    public static class Extentions
    {
        public static Boolean LettersOnly(this string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }

        public static Boolean IsSqlSave(this string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9-_]*$");
        }

        public static T GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            var attrType = typeof(T);
            T attribute = (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();
            return attribute;
        }

        public static object GetAttributeProp<T>(this PropertyInfo property, string propName, object defaultValue) where T : Attribute
        {
            var attrType = typeof(T);
            T attribute = (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();

            if (attribute == null)
                return defaultValue;

            var retValue = attribute.GetType().GetProperty(propName).GetValue(attribute, null);

            if (retValue == null)
                return defaultValue;

            return retValue;
        }
    }
}