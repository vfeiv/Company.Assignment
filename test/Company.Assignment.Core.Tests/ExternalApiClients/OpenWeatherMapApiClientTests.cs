using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.Mappers;
using Company.Assignment.Core.Options;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Company.Assignment.Core.Tests.ExternalApiClients;

public class OpenWeatherMapApiClientTests
{
    private readonly Mock<ILogger<BaseExternalApiClient>> _loggerMock = new();
    private readonly Mock<IOptions<ExternalApisOptions>> _optionsMock = new();
    private readonly CurrentWeatherResponseToWeatherDto _mapper = new();
    private readonly Mock<HttpMessageHandler> _mockHandler = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions = TestHelpers.GetJsonSerializerOptions();
    private readonly OpenWeatherMapApiClient _client;

    public OpenWeatherMapApiClientTests()
    {
        _optionsMock.Setup(x => x.Value).Returns(new ExternalApisOptions
        {
            { "OpenWeatherMap", new ExternalApiOptions{
                ApiKey = "123",
                BaseUrl = "http://localhost"
            }}
        });
        var httpClient = new HttpClient(_mockHandler.Object);
        httpClient.BaseAddress = new Uri(_optionsMock.Object.Value["OpenWeatherMap"].BaseUrl);

        _client = new OpenWeatherMapApiClient(httpClient, _loggerMock.Object, _optionsMock.Object, _mapper, _jsonSerializerOptions);
    }

    [Theory, AutoData]
    public async Task ShouldReturn_CorrectResult(string weatherBase)
    {
        var apiResponse = new CurrentWeatherResponse
        {
            Base = weatherBase
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

        var response = await _client.GetWeather();

        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(_mapper.Map(apiResponse));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
    }

    [Theory, AutoData]
    public async Task ShouldReturn_CorrectResult_WhenStatusCodeIsNotSuccessful(string cod, string message)
    {
        var httpStatusCode = HttpStatusCode.Unauthorized;
        var errorResponse = new OpenWeatherMapApiErrorResponse
        {
            Cod = cod,
            Message = message
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

        var response = await _client.GetWeather();

        response.Data.Should().BeNull();
        response.StatusCode.Should().Be(httpStatusCode);
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be($"{errorResponse.Cod}.{errorResponse.Message}");
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

        var response = await _client.GetWeather();

        response.Data.Should().BeNull();
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be("Something went wrong");
    }
}
