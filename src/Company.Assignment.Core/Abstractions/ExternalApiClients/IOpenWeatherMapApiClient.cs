using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;

namespace Company.Assignment.Core.Abstractions.ExternalApiClients;

public interface IOpenWeatherMapApiClient
{
    Task<ApiResponse<WeatherDto?>> GetWeather(AggregateFilter? aggregateFilter = null, CancellationToken cancellationToken = default);
}