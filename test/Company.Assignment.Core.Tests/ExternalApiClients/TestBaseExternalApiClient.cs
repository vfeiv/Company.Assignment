using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace Company.Assignment.Core.Tests.ExternalApiClients;

public class TestBaseExternalApiClient(
    HttpClient httpClient,
    ILogger<BaseExternalApiClient> logger,
    JsonSerializerOptions jsonSerializerOptions) : BaseExternalApiClient(httpClient, logger, jsonSerializerOptions)
{
    internal new async Task<ApiResponse<T?>> GetRequest<T>(string requestUri, Dictionary<string, StringValues>? queryParams = null, CancellationToken cancellationToken = default)
    {
        return await base.GetRequest<T>(requestUri, queryParams, cancellationToken);
    }
}


