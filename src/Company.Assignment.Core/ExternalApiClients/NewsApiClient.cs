using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.Exceptions;
using Company.Assignment.Core.ExternalApiClients.Models.News;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients;

public class NewsApiClient(
    HttpClient httpClient,
    ILogger<BaseExternalApiClient> logger,
    IMapper<ArticleResponse, ArticleDto> mapper,
    JsonSerializerOptions jsonSerializerOptions) : BaseExternalApiClient(httpClient, logger, jsonSerializerOptions), INewsApiClient
{
    private readonly IMapper<ArticleResponse, ArticleDto> _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));

    public async Task<ApiResponse<IReadOnlyList<ArticleDto>>> GetTopHeadlines(AggregateFilter? aggregateFilter, CancellationToken cancellationToken = default)
    {
        ApiResponse<TopHeadLinesRespone> externalApiResponse;

        if (aggregateFilter == null)
        {
            aggregateFilter = new AggregateFilter();
        }

        var queryParams = new Dictionary<string, StringValues>
            {
                { "country", "us" },
            };

        if (!string.IsNullOrEmpty(aggregateFilter.Value.SearchTerm))
        {
            queryParams.Add("q", aggregateFilter.Value.SearchTerm);
        }

        try
        {
            externalApiResponse = await GetRequest<TopHeadLinesRespone>("v2/top-headlines", queryParams, cancellationToken);
        }
        catch (ExternalApiException ex)
        {
            var errorResponseJson = ex.Data[ExternalApiException.ERROR_RESPONSE_KEY];
            NewsApiErrorResponse? apiErrorResponse = null;
            if (errorResponseJson != null)
            {
                try
                {
                    apiErrorResponse = JsonSerializer.Deserialize<NewsApiErrorResponse>(
                        errorResponseJson != null ? (string)errorResponseJson : string.Empty, _jsonSerializerOptions);
                }
                catch (Exception)
                {
                    Logger.LogWarning("{NewsApiClient} : Cannot deserialize {apiErrorResponse}", nameof(NewsApiClient), apiErrorResponse);
                }
            }
            return new ApiResponse<IReadOnlyList<ArticleDto>>
            {
                Success = false,
                StatusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError,
                ErrorMessage = apiErrorResponse is not null
                ? $"{apiErrorResponse.Value.Status}.{apiErrorResponse.Value.Code}.{apiErrorResponse.Value.Message}"
                : errorResponseJson != null
                    ? (string)errorResponseJson
                    : ex.Message
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{NewsApiClient} : {Message}", nameof(NewsApiClient), ex.Message);

            return new ApiResponse<IReadOnlyList<ArticleDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = "Something went wrong"
            };
        }

        var articles = _mapper.Map(externalApiResponse.Data.Articles) ?? [];

        if (aggregateFilter.Value.SortOptions.HasValue && aggregateFilter.Value.SortOptions.Value.SortBy == SortBy.Date)
        {
            articles = aggregateFilter.Value.SortOptions.Value.SortDirection == SortDirection.Asc ?
                 articles.OrderBy(x => x.PublishedAt).ToList() : articles.OrderByDescending(x => x.PublishedAt).ToList();
        }

        return new ApiResponse<IReadOnlyList<ArticleDto>>
        {
            Success = externalApiResponse.Success,
            StatusCode = externalApiResponse.StatusCode,
            ErrorMessage = externalApiResponse.ErrorMessage,
            Data = articles
        };
    }
}
