using System;

namespace Lljxww.Common.Utilities.Attributes;

public static class AttributeExtension
{
    /// <summary>
    /// 获取属性或字段的指定特性
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static TTarget? GetInfo<TSource, TTarget>(this TSource source, AttributeTargets target, string name)
        where TTarget : Attribute
    {
        return target switch
        {
            AttributeTargets.Field => AttributeHelper.GetFieldAttribute<TSource, TTarget>(name),
            AttributeTargets.Property => AttributeHelper.GetPropAttribute<TSource, TTarget>(name),
            _ => throw new NotImplementedException()
        };
    }
}