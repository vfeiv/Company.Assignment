namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct OpenWeatherMapApiErrorResponse
{
    public string? Cod { get; init; }

    public string? Message { get; init; }
}
