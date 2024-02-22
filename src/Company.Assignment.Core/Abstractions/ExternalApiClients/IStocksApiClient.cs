using Company.Assignment.Common.Dtos;

namespace Company.Assignment.Core.Abstractions.ExternalApiClients;

public interface IStocksApiClient
{
    Task<ApiResponse<IReadOnlyList<StockPriceDto>>> GetStockPrices(CancellationToken cancellationToken = default);
}
