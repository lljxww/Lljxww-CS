using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lljxww.Common.Utilities.Attributes
{
    public static class AttributeHelper
    {
        public static TTarget? GetPropAttribute<TSource, TTarget>(string propertyName) where TTarget : Attribute
        {
            PropertyInfo? prop = typeof(TSource).GetProperty(propertyName);
            TTarget? attr = prop?.GetCustomAttribute<TTarget>(false);

            return attr ?? default;
        }

        public static List<TTarget?> GetPropAttributes<TSource, TTarget>() where TTarget : Attribute
        {
            PropertyInfo[]? props = typeof(TSource).GetProperties();

            return props.Select(prop => prop.GetCustomAttribute<TTarget>(false)).Where(attr => attr is not null).ToList();
        }

        public static TTarget? GetFieldAttribute<TSource, TTarget>(string propertyName) where TTarget : Attribute
        {
            FieldInfo? prop = typeof(TSource).GetField(propertyName);
            TTarget? attr = prop?.GetCustomAttribute<TTarget>(false);

            return attr ?? default;
        }

        public static List<TTarget?> GetFieldAttributes<TSource, TTarget>() where TTarget : Attribute
        {
            FieldInfo[]? props = typeof(TSource).GetFields();

            return props.Select(prop => prop.GetCustomAttribute<TTarget>(false)).Where(attr => attr is not null).ToList();
        }
    }
}
