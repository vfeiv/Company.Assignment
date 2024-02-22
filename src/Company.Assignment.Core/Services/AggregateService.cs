using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;

namespace Company.Assignment.Core.Services;

public class AggregateService(IOpenWeatherMapApiClient openWeatherMapApiClient, ITiingoApiClient stocksApiClient) : IAggregateService
{
    private readonly IOpenWeatherMapApiClient _openWeatherMapApiClient = openWeatherMapApiClient ?? throw new ArgumentNullException(nameof(openWeatherMapApiClient));
    private readonly ITiingoApiClient _stocksApiClient = stocksApiClient ?? throw new ArgumentNullException(nameof(stocksApiClient));

    public async Task<AggregatedData> GetAggregateData(AggregateFilter? aggregateFilter, CancellationToken cancellationToken = default)
    {
        var weatherApiResponseTask = _openWeatherMapApiClient.GetWeather(aggregateFilter, cancellationToken);
        var stockPricesApiResponseTask = _stocksApiClient.GetStockPrices(aggregateFilter, cancellationToken);

        await Task.WhenAll(weatherApiResponseTask, stockPricesApiResponseTask);

        return new AggregatedData
        {
            Weather = weatherApiResponseTask.Result,
            StockPrices = stockPricesApiResponseTask.Result
        };
    }
}
