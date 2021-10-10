using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lljxww.Common.Attributes
{
    public class AttributeHelper
    {
        public static TTarget? GetPropAttribute<TSource, TTarget>(string propertyName) where TTarget : Attribute
        {
            PropertyInfo? prop = typeof(TSource).GetProperty(propertyName);
            TTarget? attr = prop?.GetCustomAttribute<TTarget>(false);

            if (attr is null)
            {
                return default;
            }
            else
            {
                return attr;
            }
        }

        public static IEnumerable<TTarget> GetPropAttributies<TSource, TTarget>() where TTarget : Attribute
        {
            PropertyInfo[]? props = typeof(TSource).GetProperties();

            IList<TTarget> results = new List<TTarget>();

            foreach (PropertyInfo? prop in props)
            {
                TTarget? attr = prop.GetCustomAttribute<TTarget>(false);
                if (attr is not null)
                {
                    results.Add(attr);
                }
            }

            return results;
        }
    }
}
