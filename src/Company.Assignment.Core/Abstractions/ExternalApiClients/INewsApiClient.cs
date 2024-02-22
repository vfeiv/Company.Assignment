using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;

namespace Company.Assignment.Core.Abstractions.ExternalApiClients;

public interface INewsApiClient
{
    Task<ApiResponse<IReadOnlyList<ArticleDto>>> GetTopHeadlines(AggregateFilter? aggregateFilter, CancellationToken cancellationToken = default);
}
