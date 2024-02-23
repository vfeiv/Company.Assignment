namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct CurrentWeatherResponse
{
    /// <summary>
    /// Coordinates of the location
    /// </summary>
    public Coord Coord { get; init; }

    /// <summary>
    /// Weather info
    /// </summary>
    public IReadOnlyList<Weather>? Weather { get; init; }

    /// <summary>
    /// Internal parameter
    /// </summary>
    public string? Base { get; init; }

    /// <summary>
    /// Main info
    /// </summary>
    public Main Main { get; init; }

    /// <summary>
    /// Time of data calculation, unix, UTC
    /// </summary>
    public long? Dt { get; init; }

    /// <summary>
    /// City name
    /// </summary>
    public string? Name { get; init; }
}
