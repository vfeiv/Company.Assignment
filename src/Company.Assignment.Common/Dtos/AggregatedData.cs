namespace Company.Assignment.Common.Dtos;

public readonly record struct AggregatedData
{
    public ApiResponse<WeatherDto?> Weather { get; init; }
    public ApiResponse<IReadOnlyList<StockPriceDto>> StockPrices { get; init; }
}
