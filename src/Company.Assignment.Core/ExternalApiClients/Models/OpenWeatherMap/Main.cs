using System.Text.Json.Serialization;

namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct Main
{
    /// <summary>
    /// Temperature. Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit
    /// </summary>
    public double Temp { get; init; }

    /// <summary>
    /// Temperature. This temperature parameter accounts for the human perception of weather.
    /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit
    /// </summary>
    [JsonPropertyName("Feels_Like")]

    public double FeelsLike { get; init; }

    /// <summary>
    /// Atmospheric pressure on the sea level, hPa
    /// </summary>
    public double Pressure { get; init; }

    /// <summary>
    /// Humidity, %
    /// </summary>
    public double Humidity { get; init; }

    /// <summary>
    /// Minimum temperature at the moment.
    /// This is minimal currently observed temperature (within large megalopolises and urban areas).
    /// </summary>
    [JsonPropertyName("Temp_Min")]
    public double TempMin { get; init; }

    /// <summary>
    /// Maximum temperature at the moment.
    /// This is maximal currently observed temperature (within large megalopolises and urban areas).
    /// </summary>
    [JsonPropertyName("Temp_Max")]
    public double TempMax { get; init; }

    /// <summary>
    /// Atmospheric pressure on the sea level, hPa
    /// </summary>
    [JsonPropertyName("Sea_Level")]
    public double SeaLevel { get; init; }

    /// <summary>
    /// Atmospheric pressure on the ground level, hPa
    /// </summary>
    [JsonPropertyName("Grnd_Level")]
    public double GrndLevel { get; init; }
}
