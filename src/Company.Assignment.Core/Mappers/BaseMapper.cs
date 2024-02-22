using Company.Assignment.Core.Abstractions.Mappers;

namespace Company.Assignment.Core.Mappers;

public abstract class BaseMapper<TFrom, TTo> : IMapper<TFrom, TTo>
{
    public abstract TTo Map(TFrom from);

    public IReadOnlyList<TTo>? Map(IEnumerable<TFrom>? from)
    {
        if (from == default) return default;

        return from.Select(x => Map(x)).ToList();
    }
}
