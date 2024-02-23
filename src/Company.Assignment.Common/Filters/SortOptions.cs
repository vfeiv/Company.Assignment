using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct SortOptions
{
    /// <summary>
    /// Column to sort by. 
    /// </summary>
    public SortBy SortBy { get; init; }

    /// <summary>
    /// Sort direction.
    /// Prepend "desc" if you want descending order or "asc" for ascending. Default is "asc".
    /// </summary>
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
