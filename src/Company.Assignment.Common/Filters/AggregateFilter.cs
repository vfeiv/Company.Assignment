namespace Company.Assignment.Common.Filters;

public readonly record struct AggregateFilter
{
    public AggregateFilter() { }

    public WeatherFilter Weather { get; init; } = new WeatherFilter();

    public StockPriceFilter StockPrice { get; init; } = new StockPriceFilter();
}