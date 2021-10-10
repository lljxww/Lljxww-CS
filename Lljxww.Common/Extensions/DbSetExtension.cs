using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Lljxww.Common.Extensions
{
    public static class DbSetExtension
    {
        public static void Delete<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> func) where T : class
        {
            IEnumerable<T> contents = dbSet.Where(func);
            
            if (!contents.Any())
            {
                return;
            }

            dbSet.RemoveRange(contents);
        }

        public static void Update<T>(this DbSet<T?> dbSet, Expression<Func<T>> values, Expression<Func<T?, bool>> match) where T : class
        {
            T? instance = dbSet.SingleOrDefault(match);

            if (instance == null)
            {
                return;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            T value = values.Compile().Invoke();

            foreach (PropertyInfo property in properties)
            {
                object? propertyValue = property.GetValue(value);
                if (propertyValue != default)
                {
                    property.SetValue(instance, propertyValue);
                }
            }

            dbSet.Update(instance);
        }
    }
}