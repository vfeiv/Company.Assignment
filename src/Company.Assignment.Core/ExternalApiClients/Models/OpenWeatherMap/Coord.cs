namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct Coord
{
    /// <summary>
    ///  Longitude of the location
    /// </summary>
    public double Lon { get; init; }

    /// <summary>
    /// Latitude of the location
    /// </summary>
    public double Lat { get; init; }
}
