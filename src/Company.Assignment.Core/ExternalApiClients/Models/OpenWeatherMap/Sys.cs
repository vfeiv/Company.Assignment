namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct Sys
{
    /// <summary>
    ///  Internal parameter
    /// </summary>
    public int Type { get; init; }

    /// <summary>
    /// Internal parameter
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Country code (GB, JP etc.)
    /// </summary>
    public string Country { get; init; }

    /// <summary>
    /// Sunrise time, unix, UTC
    /// </summary>
    public long Sunrise { get; init; }

    /// <summary>
    /// Sunset time, unix, UTC
    /// </summary>
    public long Sunset { get; init; }
}
