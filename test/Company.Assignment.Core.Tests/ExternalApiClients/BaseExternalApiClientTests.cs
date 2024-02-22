using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Exceptions;
using Company.Assignment.Core.ExternalApiClients;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Company.Assignment.Core.Tests.ExternalApiClients;

public class BaseExternalApiClientTests
{
    private readonly Mock<ILogger<BaseExternalApiClient>> _loggerMock = new();
    private readonly Mock<HttpMessageHandler> _mockHandler = new();
    private readonly JsonSerializerOptions _jsonSerializerOptions = TestHelpers.GetJsonSerializerOptions();
    private readonly TestBaseExternalApiClient _client;

    public BaseExternalApiClientTests()
    {
        var httpClient = new HttpClient(_mockHandler.Object);

        _client = new TestBaseExternalApiClient(httpClient, _loggerMock.Object, _jsonSerializerOptions);
    }

    [Theory, AutoData]
    public async Task ShouldAddQueryParams_WhenParametersAreSupplied(string key, string value)
    {
        var queryParams = new Dictionary<string, StringValues>
        {
            { key, value }
        };

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get && x.RequestUri != null && x.RequestUri.PathAndQuery.Contains($"{key}={value}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("null")
            })
            .Verifiable();

        var response = await _client.GetRequest<object>("https://localhost", queryParams);

        _mockHandler.Verify();
    }

    [Fact]
    public async Task ShouldReturn_CorrectResult()
    {
        var apiResponse = new ApiResponse<dynamic>
        {
            Success = true,
            StatusCode = HttpStatusCode.OK,
            Data = new { }
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

        var response = await _client.GetRequest<object>("https://localhost");

        response.Data.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Success.Should().BeTrue();
    }

    [Theory, AutoData]
    public async Task ShouldThrow_ExternalApiException_WhenStatusCodeIsNotSuccessful(string response)
    {
        var expectedStatusCode = HttpStatusCode.Unauthorized;
        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(x => x.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = expectedStatusCode,
                Content = new StringContent(response)
            });

        var action = () => _client.GetRequest<object>("https://localhost");

        var exception = await action.Should().ThrowAsync<ExternalApiException>();
        exception.WithMessage("Unsuccessful response");
        exception.Which.Data.Should().NotBeNull();
        exception.Which.Data[ExternalApiException.ERROR_RESPONSE_KEY].Should().NotBeNull();
        exception.Which.Data[ExternalApiException.ERROR_RESPONSE_KEY].Should().Be(response);
        exception.Which.StatusCode.Should().Be(expectedStatusCode);
    }
}
