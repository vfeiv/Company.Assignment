using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct ArticleFilter
{
    public string Country { get; init; }

    public static ValueTask<ArticleFilter?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        return ValueTask.FromResult<ArticleFilter?>(new ArticleFilter
        {
            Country = context.Request.Query["country"]
        });
    }
}

