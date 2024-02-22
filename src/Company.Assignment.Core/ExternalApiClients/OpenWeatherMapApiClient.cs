using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients;

public class OpenWeatherMapApiClient(
    HttpClient httpClient,
    ILogger<BaseExternalApiClient> logger,
    IOptions<ExternalApisOptions> externalApiOptions,
    IMapper<CurrentWeatherResponse, WeatherDto> mapper,
    JsonSerializerOptions jsonSerializerOptions) : BaseExternalApiClient(httpClient, logger, jsonSerializerOptions), IOpenWeatherMapApiClient
{
    private readonly IOptions<ExternalApisOptions> _externalApiOptions = externalApiOptions ?? throw new ArgumentNullException(nameof(externalApiOptions));
    private readonly IMapper<CurrentWeatherResponse, WeatherDto> _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ApiResponse<WeatherDto?>> GetWeather(CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, StringValues>()
        {
            { "lat", "37.58596" },
            { "lon", "23.45005" },
            { "appid", _externalApiOptions.Value["OpenWeatherMap"].ApiKey }
        };
        var externalApiResponse = await GetRequest<CurrentWeatherResponse>("/data/2.5/weather", queryParams, cancellationToken);

        return new ApiResponse<WeatherDto?>
        {
            Success = externalApiResponse.Value.Success,
            StatusCode = externalApiResponse.Value.StatusCode,
            ErrorMessage = externalApiResponse.Value.ErrorMessage,
            Data = externalApiResponse.Value.Data != default ? _mapper.Map(externalApiResponse.Value.Data) : null
        };
    }
}
