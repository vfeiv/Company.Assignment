using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients;

public abstract class BaseExternalApiClient(HttpClient httpClient, ILogger<BaseExternalApiClient> logger, JsonSerializerOptions jsonSerializerOptions)
{
    protected readonly ILogger<BaseExternalApiClient> Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));

    public async Task<ApiResponse<T?>> GetRequest<T>(string requestUri, Dictionary<string, StringValues>? queryParams = null, CancellationToken cancellationToken = default)
    {
        if (queryParams is not null)
            requestUri = QueryHelpers.AddQueryString(requestUri, queryParams.Select(x => new KeyValuePair<string, StringValues>(x.Key, x.Value)));

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

        await EnsureSuccessStatusCode(httpResponseMessage);

        var content = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken);

        return new ApiResponse<T?>
        {
            Success = true,
            StatusCode = httpResponseMessage.StatusCode,
            Data = JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions)
        };
    }

    private async Task EnsureSuccessStatusCode(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return;
        }

        var errorResponse = await httpResponseMessage.Content.ReadAsStringAsync();

        Logger.LogError("Http request {RequestUri} failed with status code {StatusCode} and message {responseString}",
            httpResponseMessage.RequestMessage?.RequestUri,
            httpResponseMessage.StatusCode,
            errorResponse);

        throw new ExternalApiException("Unsuccessful response", errorResponse, null, httpResponseMessage.StatusCode);
    }
}
