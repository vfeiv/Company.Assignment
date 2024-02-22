using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.Exceptions;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net;
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
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));

    public async Task<ApiResponse<WeatherDto?>> GetWeather(AggregateFilter? aggregateFilter = null, CancellationToken cancellationToken = default)
    {
        ApiResponse<CurrentWeatherResponse> externalApiResponse;

        if(aggregateFilter == null)
        {
            aggregateFilter = new AggregateFilter();
        }

        var queryParams = new Dictionary<string, StringValues>()
        {
            { "lat", new StringValues(aggregateFilter.Value.Weather.Lat.ToString()) },
            { "lon", new StringValues(aggregateFilter.Value.Weather.Lon.ToString()) },
            { "appid", _externalApiOptions.Value["OpenWeatherMap"].ApiKey }
        };

        try
        {
            externalApiResponse = await GetRequest<CurrentWeatherResponse>("/data/2.5/weather", queryParams, cancellationToken);
        }
        catch (ExternalApiException ex)
        {
            var errorResponseJson = ex.Data[ExternalApiException.ERROR_RESPONSE_KEY];
            OpenWeatherMapApiErrorResponse? apiErrorResponse = null;
            if (errorResponseJson != null)
            {
                try
                {
                    apiErrorResponse = JsonSerializer.Deserialize<OpenWeatherMapApiErrorResponse>(
                        errorResponseJson != null ? (string)errorResponseJson : string.Empty, _jsonSerializerOptions);
                }
                catch (Exception)
                {
                    Logger.LogWarning("{OpenWeatherMapApiClient} : Cannot deserialize {apiErrorResponse}", nameof(OpenWeatherMapApiClient), apiErrorResponse);
                }
            }
            return new ApiResponse<WeatherDto?>
            {
                Success = false,
                StatusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError,
                ErrorMessage = apiErrorResponse is not null ? $"{apiErrorResponse.Value.Cod}.{apiErrorResponse.Value.Message}" : errorResponseJson != null ? (string)errorResponseJson : ex.Message
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{OpenWeatherMapApiClient} : {Message}", nameof(OpenWeatherMapApiClient), ex.Message);

            return new ApiResponse<WeatherDto?>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = "Something went wrong"
            };
        }

        return new ApiResponse<WeatherDto?>
        {

            Success = externalApiResponse.Success,
            StatusCode = externalApiResponse.StatusCode,
            Data = externalApiResponse.Data != default ? _mapper.Map(externalApiResponse.Data) : null
        };
    }
}
