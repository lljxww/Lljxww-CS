using Dapper;
using System.Reflection;

namespace Lljxww.Common.Dapper.Extensions
{
    public class ColumnHelper
    {
        public static void SetMapper(Assembly assembly)
        {
            foreach (Type? type in assembly.GetTypes())
            {
                bool marked = false;
                foreach (PropertyInfo? item in type.GetProperties())
                {
                    if (item.GetCustomAttribute(typeof(ColumnAttribute)) != null)
                    {
                        marked = true;
                        break;
                    }
                }

                if (!marked)
                {
                    continue;
                }

                TypeMapper fallback = new(new SqlMapper.ITypeMap[]
                {
                    new CustomPropertyTypeMap(
                        type,
                        (type, columnName) =>
                            type.GetProperties().FirstOrDefault(prop =>
                                prop.GetCustomAttributes(false)
                                    .OfType<ColumnAttribute>()
                                    .Any(attr => attr.Name == columnName))
                    ),
                    new DefaultTypeMap(type)
                });

                SqlMapper.SetTypeMap(type, fallback);
            }
        }
    }
}

