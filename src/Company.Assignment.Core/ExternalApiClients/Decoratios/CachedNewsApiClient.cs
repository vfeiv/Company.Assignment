using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Constants;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients.Decoratios;

internal class CachedNewsApiClient(
    IDistributedCache distributedCache,
    JsonSerializerOptions jsonSerializerOptions,
    INewsApiClient newsApiClient) : INewsApiClient
{

    private readonly IDistributedCache _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
    private readonly INewsApiClient _newsApiClient = newsApiClient ?? throw new ArgumentNullException(nameof(newsApiClient));

    public async Task<ApiResponse<IReadOnlyList<ArticleDto>>> GetTopHeadlines(AggregateFilter? aggregateFilter, CancellationToken cancellationToken = default)
    {
        if (aggregateFilter == null)
        {
            aggregateFilter = new AggregateFilter();
        }

        var cacheKey = string.Format(CachingConstants.ArticlesCacheKey, aggregateFilter.Value.SearchTerm + aggregateFilter.Value.Article.Country);
        var articlesJson = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(articlesJson))
        {
            var articlesFromCache = JsonSerializer.Deserialize<ApiResponse<IReadOnlyList<ArticleDto>>>(articlesJson, _jsonSerializerOptions);
            if (articlesFromCache != default)
            {
                return articlesFromCache;
            }
        }

        var articles = await _newsApiClient.GetTopHeadlines(aggregateFilter, cancellationToken);

        await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(articles, _jsonSerializerOptions), token: cancellationToken);

        return articles;
    }
}