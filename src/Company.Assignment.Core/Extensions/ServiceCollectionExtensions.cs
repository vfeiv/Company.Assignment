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
using Company.Assignment.Core.ExternalApiClients.Decoratios;

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
        services.AddDecorator<INewsApiClient, CachedNewsApiClient>();
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

    /// <summary>
    /// source: https://github.com/dotnet/runtime/issues/36021
	/// Registers a <typeparamref name="TService"/> decorator on top of the previous registration of that type.
	/// </summary>
	/// <param name="lifetime">If no lifetime is provided, the lifetime of the previous registration is used.</param>
	public static IServiceCollection AddDecorator<TService, TImplementation>(
        this IServiceCollection services,
        ServiceLifetime? lifetime = null)
        where TService : class
        where TImplementation : TService
    {
        var decoratorFactory = ActivatorUtilities.CreateFactory(typeof(TImplementation),
            [typeof(TService)]);

        return AddDecorator<TService>(
            services,
            (serviceProvider, decoratedInstance) =>
                (TService)decoratorFactory(serviceProvider, [decoratedInstance]),
            lifetime);
    }

    /// <summary>
    /// source: https://github.com/dotnet/runtime/issues/36021
    /// Registers a <typeparamref name="TService"/> decorator on top of the previous registration of that type.
    /// </summary>
    /// <param name="decoratorFactory">Constructs a new instance based on the the instance to decorate and the <see cref="IServiceProvider"/>.</param>
    /// <param name="lifetime">If no lifetime is provided, the lifetime of the previous registration is used.</param>
    public static IServiceCollection AddDecorator<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService, TService> decoratorFactory,
        ServiceLifetime? lifetime = null)
        where TService : class
    {
        // By convention, the last registration wins
        var previousRegistration = services.LastOrDefault(
            descriptor => descriptor.ServiceType == typeof(TService)) ?? throw new InvalidOperationException($"Tried to register a decorator for type {typeof(TService).Name} when no such type was registered.");

        // Get a factory to produce the original implementation
        var decoratedServiceFactory = previousRegistration.ImplementationFactory;
        if (decoratedServiceFactory is null && previousRegistration.ImplementationInstance != null)
            decoratedServiceFactory = _ => previousRegistration.ImplementationInstance;
        if (decoratedServiceFactory is null && previousRegistration.ImplementationType != null)
            decoratedServiceFactory = serviceProvider => ActivatorUtilities.CreateInstance(
                serviceProvider, previousRegistration.ImplementationType, []);

        if (decoratedServiceFactory is null) // Should be impossible
            throw new Exception($"Tried to register a decorator for type {typeof(TService).Name}, but the registration being wrapped specified no implementation at all.");

        var registration = new ServiceDescriptor(
            typeof(TService), CreateDecorator, lifetime ?? previousRegistration.Lifetime);

        services.Add(registration);

        return services;

        // Local function that creates the decorator instance
        TService CreateDecorator(IServiceProvider serviceProvider)
        {
            var decoratedInstance = (TService)decoratedServiceFactory(serviceProvider);
            var decorator = decoratorFactory(serviceProvider, decoratedInstance);
            return decorator;
        }
    }
}
