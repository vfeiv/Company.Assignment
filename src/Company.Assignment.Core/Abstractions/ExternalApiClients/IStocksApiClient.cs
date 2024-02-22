using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;

namespace Company.Assignment.Core.Abstractions.ExternalApiClients;

public interface IStocksApiClient
{
    Task<ApiResponse<IReadOnlyList<StockPriceDto>>> GetStockPrices(AggregateFilter aggregateFilter, CancellationToken cancellationToken = default);
}
