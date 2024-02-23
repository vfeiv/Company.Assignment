namespace Company.Assignment.Common.Dtos;

public readonly record struct AggregatedData
{
    /// <summary>
    /// Weather Data
    /// </summary>
    public ApiResponse<WeatherDto?> Weather { get; init; }

    /// <summary>
    /// StockPrices Data
    /// </summary>
    public ApiResponse<IReadOnlyList<StockPriceDto>> StockPrices { get; init; }

    /// <summary>
    /// Articles Data
    /// </summary>
    public ApiResponse<IReadOnlyList<ArticleDto>> Articles { get; init; }
}
