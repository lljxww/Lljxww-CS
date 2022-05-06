using System;

namespace Lljxww.Common.Utilities.Attributes;

/// <summary>
///     标记
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RemarkAttribute : Attribute
{
    /// <summary>
    ///     附带信息
    /// </summary>
    public string? Info { get; set; }

    /// <summary>
    ///     自定义信息
    /// </summary>
    public object? CustomData { get; set; }
}