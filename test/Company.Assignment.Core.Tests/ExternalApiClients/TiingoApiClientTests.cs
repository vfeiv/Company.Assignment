using AutoFixture.Xunit2;
using Company.Assignment.Core.ExternalApiClients;
using Company.Assignment.Core.Mappers;
using Company.Assignment.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;
using FluentAssertions;

namespace Company.Assignment.Core.Tests.ExternalApiClients;

public class TiingoApiClientTests
{
    private readonly Mock<ILogger<BaseExternalApiClient>> _loggerMock = new();
    private readonly Mock<IOptions<ExternalApisOptions>> _optionsMock = new();
    private readonly StockPriceResponseToStockPriceDto _mapper = new();
    private readonly Mock<HttpMessageHandler> _mockHandler = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions = TestHelpers.GetJsonSerializerOptions();
    private readonly TiingoApiClient _client;

    public TiingoApiClientTests()
    {
        _optionsMock.Setup(x => x.Value).Returns(new ExternalApisOptions
        {
            { "Tiingo", new ExternalApiOptions{
                ApiKey = "123",
                BaseUrl = "http://localhost"
            }}
        });
        var httpClient = new HttpClient(_mockHandler.Object)
        {
            BaseAddress = new Uri(_optionsMock.Object.Value["Tiingo"].BaseUrl)
        };

        _client = new TiingoApiClient(httpClient, _loggerMock.Object, _optionsMock.Object, _mapper, _jsonSerializerOptions);
    }

    [Theory, AutoData]
    public async Task ShouldReturn_CorrectResult(double high)
    {
        var apiResponse = new List<StockPriceResponse>
        {
            new StockPriceResponse
            {
                High = high,
            }
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(apiResponse, _jsonSerializerOptions))
            });

        var response = await _client.GetStockPrices(null);

        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(_mapper.Map(apiResponse));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
    }

    [Theory, AutoData]
    public async Task ShouldReturn_CorrectResult_WhenStatusCodeIsNotSuccessful(string detail)
    {
        var httpStatusCode = HttpStatusCode.Unauthorized;
        var errorResponse = new TiingoApiErrorResponse
        {
            Detail = detail
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(JsonSerializer.Serialize(errorResponse, _jsonSerializerOptions))
            });

        var response = await _client.GetStockPrices(null);

        response.Data.Should().BeNull();
        response.StatusCode.Should().Be(httpStatusCode);
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be(errorResponse.Detail);
    }

    [Fact]
    public async Task ShouldReturn_CorrectResult_WhenExceptionOccures()
    {
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception());

        var response = await _client.GetStockPrices(null);

        response.Data.Should().BeNull();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be("Something went wrong");
    }
}
