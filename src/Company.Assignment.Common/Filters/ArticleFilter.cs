using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct ArticleFilter
{
    /// <summary>
    /// The 2-letter ISO 3166-1 code of the country you want to get headlines for.
    /// </summary>
    public string Country { get; init; }

    public static ValueTask<ArticleFilter?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        return ValueTask.FromResult<ArticleFilter?>(new ArticleFilter
        {
            Country = context.Request.Query["country"]
        });
    }
}

