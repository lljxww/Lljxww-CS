using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lljxww.Extensions;

public static class ExpressionExtension
{
    /// <summary>
    /// Lambda表达式拼接
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="merge"></param>
    /// <returns></returns>
    public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
        Func<Expression, Expression, Expression> merge)
    {
        Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
            .Select((f, i)
                => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);
        Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    /// <summary>
    /// and扩展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.And);
    }

    /// <summary>
    /// or扩展
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.Or);
    }
}

/// <summary>
/// </summary>
public class ParameterRebinder : ExpressionVisitor
{
    private readonly Dictionary<ParameterExpression, ParameterExpression> _map;

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    private ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {
        _map = map ?? [];
    }

    /// <summary>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="exp"></param>
    /// <returns></returns>
    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
    {
        return new ParameterRebinder(map).Visit(exp);
    }

    /// <summary>
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    protected override Expression VisitParameter(ParameterExpression p)
    {
        if (_map.TryGetValue(p, out ParameterExpression? replacement))
        {
            p = replacement;
        }

        return base.VisitParameter(p);
    }
}