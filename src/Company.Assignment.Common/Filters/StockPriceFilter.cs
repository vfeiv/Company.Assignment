using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct StockPriceFilter
{
    public StockPriceFilter() { }

    public string Ticker { get; init; } = "aapl";

    public DateTime? StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public static ValueTask<StockPriceFilter?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var isStartDateParsed = DateTime.TryParse(context.Request.Query["startDate"], out var startDate);
        var isEndDateParsed = DateTime.TryParse(context.Request.Query["endDate"], out var endDate);

        if (!isStartDateParsed || !isEndDateParsed)
        {
            return new ValueTask<StockPriceFilter?>(new StockPriceFilter());
        }

        return ValueTask.FromResult<StockPriceFilter?>(new StockPriceFilter
        {
            Ticker = context.Request.Query["ticker"],
            StartDate = startDate,
            EndDate = endDate,
        });
    }
}
