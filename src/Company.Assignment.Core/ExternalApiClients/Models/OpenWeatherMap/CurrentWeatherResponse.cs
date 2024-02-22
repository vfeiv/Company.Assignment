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
    /// Visibility, meter. The maximum value of the visibility is 10 km
    /// </summary>
    public double Visibility { get; init; }
    /// <summary>
    /// Wind info
    /// </summary>
    public Wind Wind { get; init; }
    /// <summary>
    /// Rain info
    /// </summary>
    public Rain Rain { get; init; }
    /// <summary>
    /// Clouds info
    /// </summary>
    public Clouds Clouds { get; init; }
    /// <summary>
    /// Time of data calculation, unix, UTC
    /// </summary>
    public long Dt { get; init; }
    /// <summary>
    /// Sys info
    /// </summary>
    public Sys Sys { get; init; }
    /// <summary>
    /// Shift in seconds from UTC
    /// </summary>
    public int TimeZone { get; init; }
    /// <summary>
    ///  City ID
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// City name
    /// </summary>
    public string? Name { get; init; }
    /// <summary>
    /// Internal parameter
    /// </summary>
    public int Cod { get; init; }
}
