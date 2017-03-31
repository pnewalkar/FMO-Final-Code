using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Fmo.Common.Enums;

namespace Fmo.Common
{
    public static class EnumExtensions
    {
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            return default(T);
        }

        public static string GetStringValue(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value", "paramater cannot be null");
            }
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(ValueAttribute), false) as ValueAttribute[];

            // Return the first if there was a match.
            if (attribs == null)
            {
                return value.ToString();
            }

            return attribs.Length > 0 ? attribs[0].Value : value.ToString();
        }

        public static T GetEnumValue<T>(this string value, bool ignoreCase)
        {
            return (T)GetEnumStringValue(value, typeof(T), ignoreCase);
        }

        public static T GetEnumValue<T>(this string value)
        {
            return (T)GetEnumStringValue(value, typeof(T), true);
        }

        public static string GetDescription(this Enum en) // ext method
        {

            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        private static object GetEnumStringValue(string value, Type enumType, bool ignoreCase)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("enumType should be a valid enum", "enumType");
            }

            foreach (var fieldInfo in from fieldInfo in enumType.GetFields()
                                      let attributes = fieldInfo.GetCustomAttributes(typeof(ValueAttribute), false)
                                      where attributes.Length > 0
                                      let attribute = (ValueAttribute)attributes[0]
                                      where String.Compare(attribute.Value, value, ignoreCase) == 0
                                      select fieldInfo)
            {
                return Enum.Parse(enumType, fieldInfo.Name);
            }

            return Enum.Parse(enumType, value);
        }
    }
}
