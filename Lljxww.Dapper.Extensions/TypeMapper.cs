using System.Reflection;
using static Dapper.SqlMapper;

namespace Lljxww.Dapper.Extensions;

internal class TypeMapper : ITypeMap
{
    private readonly IEnumerable<ITypeMap> _mappers;

    public TypeMapper(IEnumerable<ITypeMap> mappers)
    {
        _mappers = mappers;
    }

    public ConstructorInfo? FindConstructor(string[] names, Type[] types)
    {
        foreach (ITypeMap? mapper in _mappers)
        {
            try
            {
                ConstructorInfo result = mapper.FindConstructor(names, types);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }

    public IMemberMap? GetConstructorParameter(ConstructorInfo constructor, string columnName)
    {
        foreach (ITypeMap? mapper in _mappers)
        {
            try
            {
                IMemberMap? result = mapper.GetConstructorParameter(constructor, columnName);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }

    public IMemberMap? GetMember(string columnName)
    {
        foreach (ITypeMap? mapper in _mappers)
        {
            try
            {
                IMemberMap? result = mapper.GetMember(columnName);
                if (result != null)
                {
                    return result;
                }
            }
            catch (NotImplementedException)
            {
            }
        }

        return null;
    }

    public ConstructorInfo? FindExplicitConstructor()
    {
        return _mappers
            .Select(mapper => mapper.FindExplicitConstructor())
            .FirstOrDefault(result => result != null);
    }
}