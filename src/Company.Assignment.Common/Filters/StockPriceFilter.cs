using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Company.Assignment.Common.Filters;

public readonly record struct StockPriceFilter
{
    public StockPriceFilter() { }

    /// <summary>
    /// Ticker related to the asset.
    /// </summary>
    public string Ticker { get; init; } = "aapl";

    /// <summary>
    /// If startDate or endDate is not null, historical data will be queried.
    /// This filter limits metrics to on or after the startDate (>=).
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// If startDate or endDate is not null, historical data will be queried.
    /// This filter limits metrics to on or before the endDate(<=).
    /// </summary>
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
