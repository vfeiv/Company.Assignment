namespace Company.Assignment.Common.Filters;

public readonly record struct AggregateFilter
{
    public AggregateFilter() { }

    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Column and sort direction to sort by. 
    /// </summary>
    public SortOptions? SortOptions { get; init; }

    /// <summary>
    /// Weather Filters
    /// </summary>
    public WeatherFilter Weather { get; init; } = new WeatherFilter();

    /// <summary>
    /// StockPrice Filters
    /// </summary>
    public StockPriceFilter StockPrice { get; init; } = new StockPriceFilter();

    /// <summary>
    /// Article Filters
    /// </summary>
    public ArticleFilter Article { get; init; } = new ArticleFilter();
}

