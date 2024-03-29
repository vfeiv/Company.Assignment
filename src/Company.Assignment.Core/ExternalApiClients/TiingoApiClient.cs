﻿using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.Exceptions;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;
using Company.Assignment.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net;
using System.Text.Json;

namespace Company.Assignment.Core.ExternalApiClients;

public class TiingoApiClient(
    HttpClient httpClient,
    ILogger<BaseExternalApiClient> logger,
    IOptions<ExternalApisOptions> externalApiOptions,
    IMapper<StockPriceResponse, StockPriceDto> mapper,
    JsonSerializerOptions jsonSerializerOptions) : BaseExternalApiClient(httpClient, logger, jsonSerializerOptions), ITiingoApiClient
{
    private readonly IOptions<ExternalApisOptions> _externalApiOptions = externalApiOptions ?? throw new ArgumentNullException(nameof(externalApiOptions));
    private readonly IMapper<StockPriceResponse, StockPriceDto> _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));

    public async Task<ApiResponse<IReadOnlyList<StockPriceDto>>> GetStockPrices(AggregateFilter? aggregateFilter, CancellationToken cancellationToken = default)
    {
        ApiResponse<IReadOnlyList<StockPriceResponse>?> externalApiResponse;

        if (aggregateFilter == null)
        {
            aggregateFilter = new AggregateFilter();
        }

        var queryParams = new Dictionary<string, StringValues>
            {
                { "token", _externalApiOptions.Value["Tiingo"].ApiKey }
            };

        if (aggregateFilter.Value.StockPrice.StartDate is not null)
            queryParams.Add("startDate", aggregateFilter.Value.StockPrice.StartDate.Value.ToString("yyyy-MM-dd"));

        if (aggregateFilter.Value.StockPrice.EndDate is not null)
            queryParams.Add("endDate", aggregateFilter.Value.StockPrice.EndDate.Value.ToString("yyyy-MM-dd"));

        try
        {
            externalApiResponse = await GetRequest<IReadOnlyList<StockPriceResponse>>($"/tiingo/daily/{aggregateFilter.Value.StockPrice.Ticker}/prices", queryParams, cancellationToken);
        }
        catch (ExternalApiException ex)
        {
            var errorResponseJson = ex.Data[ExternalApiException.ERROR_RESPONSE_KEY];
            TiingoApiErrorResponse? apiErrorResponse = null;
            if (errorResponseJson != null)
            {
                try
                {
                    apiErrorResponse = JsonSerializer.Deserialize<TiingoApiErrorResponse>(
                        errorResponseJson != null ? (string)errorResponseJson : string.Empty, _jsonSerializerOptions);
                }
                catch (Exception)
                {
                    Logger.LogWarning("{TiingoApiClient} : Cannot deserialize {apiErrorResponse}", nameof(TiingoApiClient), apiErrorResponse);
                }
            }
            return new ApiResponse<IReadOnlyList<StockPriceDto>>
            {
                Success = false,
                StatusCode = ex.StatusCode ?? HttpStatusCode.InternalServerError,
                ErrorMessage = apiErrorResponse != null ? apiErrorResponse.Value.Detail : errorResponseJson != null ? (string)errorResponseJson : ex.Message
            };
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{TiingoApiClient} : {Message}", nameof(TiingoApiClient), ex.Message);

            return new ApiResponse<IReadOnlyList<StockPriceDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = "Something went wrong"
            };
        }

        return new ApiResponse<IReadOnlyList<StockPriceDto>>
        {
            Success = externalApiResponse.Success,
            StatusCode = externalApiResponse.StatusCode,
            Data = _mapper.Map(externalApiResponse.Data ?? [])
        };
    }
}
