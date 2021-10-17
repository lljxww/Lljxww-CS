using System;

namespace Lljxww.Common.Attributes
{
    public static class AttributeExtension
    {
        /// <summary>
        /// 获取属性或字段的指定特性
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static TTarget? GetInfo<TTarget, TSource>(this TSource source, AttributeTargets target) where TTarget : Attribute
        {
            var type = typeof(TSource);

            switch (target)
            {
                case AttributeTargets.Field:
                {
                    return AttributeHelper.GetFieldAttribute<TSource, TTarget>(type.Name);
                }
                case AttributeTargets.Property:
                {
                    return AttributeHelper.GetPropAttribute<TSource, TTarget>(type.Name);
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}