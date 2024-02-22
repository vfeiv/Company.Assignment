namespace Company.Assignment.Core.Abstractions.Mappers;

public interface IMapper<TFrom, TTo>
{
    TTo Map(TFrom from);

    IReadOnlyList<TTo>? Map(IEnumerable<TFrom>? from);
}