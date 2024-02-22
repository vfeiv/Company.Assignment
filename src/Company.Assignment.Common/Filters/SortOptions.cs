using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct SortOptions
{
    public SortBy SortBy { get; init; }

    public SortDirection SortDirection { get; init; }

    public static ValueTask<SortOptions?> BindAsync(HttpContext context, ParameterInfo parameter)
    {

        var isSortByParsed = Enum.TryParse<SortBy>(context.Request.Query["sortBy"], true, out var sortBy);
        var isSortDirectionParsed = Enum.TryParse<SortDirection>(context.Request.Query["sortDirection"], true, out var sortDirection);

        if (!isSortByParsed || !isSortDirectionParsed)
        {
            return ValueTask.FromResult<SortOptions?>(null);
        }

        return ValueTask.FromResult<SortOptions?>(new SortOptions
        {
            SortBy = sortBy,
            SortDirection = sortDirection
        });
    }
}

public enum SortBy : short
{
    Date
}

public enum SortDirection : short
{
    Asc,
    Desc
}
