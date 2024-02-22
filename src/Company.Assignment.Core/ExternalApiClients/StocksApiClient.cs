using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;
using Company.Assignment.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients;

public class StocksApiClient(
    HttpClient httpClient,
    ILogger<BaseExternalApiClient> logger,
    IOptions<ExternalApisOptions> externalApiOptions,
    IMapper<StockPriceResponse, StockPriceDto> mapper,
    JsonSerializerOptions jsonSerializerOptions) : BaseExternalApiClient(httpClient, logger, jsonSerializerOptions), IStocksApiClient
{
    private readonly IOptions<ExternalApisOptions> _externalApiOptions = externalApiOptions ?? throw new ArgumentNullException(nameof(externalApiOptions));
    private readonly IMapper<StockPriceResponse, StockPriceDto> _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ApiResponse<IReadOnlyList<StockPriceDto>>> GetStockPrices(CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, StringValues>
            {
                { "token", _externalApiOptions.Value["Tiingo"].ApiKey }
            };

        var externalApiResponse = await GetRequest<List<StockPriceResponse>>($"/tiingo/daily/aapl/prices", queryParams, cancellationToken);

        return new ApiResponse<IReadOnlyList<StockPriceDto>>
        {
            Success = externalApiResponse.Value.Success,
            StatusCode = externalApiResponse.Value.StatusCode,
            ErrorMessage = externalApiResponse.Value.ErrorMessage,
            Data = _mapper.Map(externalApiResponse.Value.Data ?? [])
        };
    }
}
