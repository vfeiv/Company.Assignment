using Company.Assignment.Core.ExternalApiClients.Models;
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

    internal async Task<ExternalApiResponse<T>?> GetRequest<T>(string requestUri, Dictionary<string, StringValues>? queryParams = null, CancellationToken cancellationToken = default)
    {
        if (queryParams is not null)
            requestUri = QueryHelpers.AddQueryString(requestUri, queryParams.Select(x => new KeyValuePair<string, StringValues>(x.Key, x.Value)));

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

        var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

        return await GetExternalApiResponse<T>(httpResponseMessage);
    }

    private async Task<ExternalApiResponse<T>> GetExternalApiResponse<T>(HttpResponseMessage httpResponseMessage)
    {
        string? errorMessage = null;

        var content = await httpResponseMessage.Content.ReadAsStringAsync();

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            errorMessage = content;

            Logger.LogError("Http request {RequestUri} failed with status code {StatusCode} and message {responseString}",
                httpResponseMessage.RequestMessage?.RequestUri,
                httpResponseMessage.StatusCode,
                errorMessage);
        }

        return new ExternalApiResponse<T>
        {
            Success = httpResponseMessage.IsSuccessStatusCode,
            StatusCode = httpResponseMessage.StatusCode,
            ErrorMessage = errorMessage,
            Data = httpResponseMessage.IsSuccessStatusCode ? JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions) : default
        };
    }
}
