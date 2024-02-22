using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Common.Filters;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Services;
using FluentAssertions;
using Moq;
using System.Net;
using Xunit;

namespace Company.Assignment.Core.Tests.Services;

public class AggregateServiceTests
{
    private readonly Mock<IOpenWeatherMapApiClient> _openWeatherMapApiClient = new();
    private readonly Mock<ITiingoApiClient> _tiingoApiClient = new();
    private readonly AggregateService _aggregateService;

    public AggregateServiceTests()
    {
        _aggregateService = new AggregateService(_openWeatherMapApiClient.Object, _tiingoApiClient.Object);
    }

    [Theory, AutoData]
    public async Task ShouldReturn_CorrectResult(double pressure, double high)
    {
        var expectedWeatherResponse = new ApiResponse<WeatherDto?>
        {
            Success = true,
            StatusCode = HttpStatusCode.OK,
            Data = new WeatherDto
            {
                Pressure = pressure
            }
        };

        var expectedStockPricesResponse = new ApiResponse<IReadOnlyList<StockPriceDto>>
        {
            Success = true,
            StatusCode = HttpStatusCode.OK,
            Data =
            [
                new StockPriceDto
                {
                    High = high
                }
            ]
        };

        _openWeatherMapApiClient
            .Setup(x => x.GetWeather(It.IsAny<AggregateFilter?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedWeatherResponse);

        _tiingoApiClient
            .Setup(x => x.GetStockPrices(It.IsAny<AggregateFilter?>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedStockPricesResponse);

        var response = await _aggregateService.GetAggregateData(null, It.IsAny<CancellationToken>());

        response.Weather.Should().BeEquivalentTo(expectedWeatherResponse);
        response.StockPrices.Should().BeEquivalentTo(expectedStockPricesResponse);
    }
}
