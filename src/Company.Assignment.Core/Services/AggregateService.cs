using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Exceptions;

namespace Company.Assignment.Core.Services;

public class AggregateService(IOpenWeatherMapApiClient openWeatherMapApiClient, IStocksApiClient stocksApiClient) : IAggregateService
{
    private readonly IOpenWeatherMapApiClient _openWeatherMapApiClient = openWeatherMapApiClient ?? throw new ArgumentNullException(nameof(openWeatherMapApiClient));
    private readonly IStocksApiClient _stocksApiClient = stocksApiClient ?? throw new ArgumentNullException(nameof(stocksApiClient));

    public async Task<AggregatedData> GetAggregateData(AggregateFilter aggregateFilter, CancellationToken cancellationToken = default)
    {
        var weatherApiResponseTask = _openWeatherMapApiClient.GetWeather(aggregateFilter, cancellationToken);
        var stockPricesApiResponseTask = _stocksApiClient.GetStockPrices(aggregateFilter, cancellationToken);

        var allTasks = Task.WhenAll(weatherApiResponseTask, stockPricesApiResponseTask);

        try
        {
            await allTasks;
        }
        catch (ExternalApiException)
        {

        }

        return new AggregatedData
        {
            Weather = !weatherApiResponseTask.IsFaulted ? weatherApiResponseTask.Result : default,
            StockPrices = !stockPricesApiResponseTask.IsFaulted ? stockPricesApiResponseTask.Result : default
        };
    }
}
