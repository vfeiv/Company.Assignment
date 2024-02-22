using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Abstractions.ExternalApiClients;

namespace Company.Assignment.Core.Services;

public class AggregateService(IOpenWeatherMapApiClient openWeatherMapApiClient) : IAggregateService
{
    private readonly IOpenWeatherMapApiClient _openWeatherMapApiClient = openWeatherMapApiClient ?? throw new ArgumentNullException(nameof(openWeatherMapApiClient));

    public async Task<AggregatedData> GetAggregateData(CancellationToken cancellationToken = default)
    {
        var weatherApiResponse = await _openWeatherMapApiClient.GetWeather(cancellationToken);

        return new AggregatedData
        {
            Weather = weatherApiResponse
        };
    }
}
