using Company.Assignment.Common.Dtos;

namespace Company.Assignment.Core.Abstractions.ExternalApiClients;

public interface IOpenWeatherMapApiClient
{
    Task<ApiResponse<WeatherDto?>> GetWeather(CancellationToken cancellationToken = default);
}