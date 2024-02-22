using Company.Assignment.Core.Options;
using System.Net.Http.Headers;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddExternalApis();
        services.AddMappers();
        services.AddServices();
        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configurationSection)
    {
        ExternalApisOptions externalApisOptions = [];
        configurationSection.Bind(externalApisOptions);
        services.Configure<ExternalApisOptions>(configurationSection);
        if (externalApisOptions is null)
        {
            return services;
        }
        foreach (var externalApiKv in externalApisOptions)
        {
            _ = services.AddHttpClient(externalApiKv.Key, httpClient =>
            {
                httpClient.BaseAddress = new Uri(externalApiKv.Value.BaseUrl);

                if (externalApiKv.Value.AuthorizationType != AuthorizationType.None && string.IsNullOrEmpty(externalApiKv.Value.ApiKey))
                {
                    throw new InvalidOperationException($"Authorization token or ApiKey not found for {externalApiKv.Key} Api with Authorization Type {externalApiKv.Value.AuthorizationType}");
                }

                switch (externalApiKv.Value.AuthorizationType)
                {
                    case AuthorizationType.Bearer:
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", externalApiKv.Value.ApiKey);
                        break;
                    case AuthorizationType.None:
                    case AuthorizationType.QueryParams:
                    default:
                        break;
                }

                foreach (var headerKv in externalApiKv.Value.Headers ?? [])
                {
                    httpClient.DefaultRequestHeaders.Add(headerKv.Key, headerKv.Value);
                }
            });
        }
        return services;
    }
}
