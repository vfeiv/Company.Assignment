using Company.Assignment.Common.Abstractions.Services;
using Company.Assignment.Core.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAggregateService, AggregateService>();
        return services;
    }
}
