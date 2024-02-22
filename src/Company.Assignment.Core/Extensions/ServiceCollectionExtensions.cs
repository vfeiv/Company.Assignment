using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.Abstractions.ExternalApiClients;
using Company.Assignment.Core.Abstractions.Mappers;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.ExternalApiClients;
using Company.Assignment.Core.Options;
using Company.Assignment.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Company.Assignment.Core.Mappers;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;
using Company.Assignment.Core.ExternalApiClients.Models.News;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        services.AddScoped<IOpenWeatherMapApiClient, OpenWeatherMapApiClient>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<BaseExternalApiClient>>();
            var options = sp.GetRequiredService<IOptions<ExternalApisOptions>>();
            var mapper = sp.GetRequiredService<IMapper<CurrentWeatherResponse, WeatherDto>>();
            var jsonSerializerOptions = sp.GetRequiredService<JsonSerializerOptions>();
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("OpenWeatherMap");
            return new OpenWeatherMapApiClient(httpClient, logger, options, mapper, jsonSerializerOptions);
        });
        services.AddScoped<ITiingoApiClient, TiingoApiClient>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<BaseExternalApiClient>>();
            var options = sp.GetRequiredService<IOptions<ExternalApisOptions>>();
            var mapper = sp.GetRequiredService<IMapper<StockPriceResponse, StockPriceDto>>();
            var jsonSerializerOptions = sp.GetRequiredService<JsonSerializerOptions>();
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("Tiingo");
            return new TiingoApiClient(httpClient, logger, options, mapper, jsonSerializerOptions);
        });
        services.AddScoped<INewsApiClient, NewsApiClient>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<BaseExternalApiClient>>();
            var mapper = sp.GetRequiredService<IMapper<ArticleResponse, ArticleDto>>();
            var jsonSerializerOptions = sp.GetRequiredService<JsonSerializerOptions>();
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("News");
            return new NewsApiClient(httpClient, logger, mapper, jsonSerializerOptions);
        });
        return services;
    }

    public static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IMapper<CurrentWeatherResponse, WeatherDto>, CurrentWeatherResponseToWeatherDto>();
        services.AddScoped<IMapper<StockPriceResponse, StockPriceDto>, StockPriceResponseToStockPriceDto>();
        services.AddScoped<IMapper<ArticleResponse, ArticleDto>, ArticleResponseToArticleDto>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            return new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        });

        services.AddScoped<IAggregateService, AggregateService>();
        return services;
    }
}
