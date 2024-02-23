namespace Company.Assignment.Common.Dtos;

public readonly record struct WeatherDto
{
    /// <summary>
    /// Group of weather parameters (Rain, Snow, Clouds etc.)
    /// </summary>
    public IReadOnlyList<WeatherCondition>? WeatherConditions { get; init; }

    /// <summary>
    /// Temperature data. Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit
    /// </summary>
    public Temperature Temperature { get; init; }

    /// <summary>
    /// Atmospheric pressure on the sea level, hPa
    /// </summary>
    public double Pressure { get; init; }

    /// <summary>
    /// Humidity, %
    /// </summary>
    public double Humidity { get; init; }

    /// <summary>
    /// Location details (coordinates, city)
    /// </summary>
    public Location Location { get; init; }

    /// <summary>
    /// Date
    /// </summary>
    public DateTimeOffset? Date { get; init; }
}

public readonly record struct WeatherCondition
{
    /// <summary>
    /// Weather condition (Rain, Snow, Clouds etc.)
    /// </summary>
    public string? Condition { get; init; }

    /// <summary>
    /// Weather condition description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Weather condition icon
    /// </summary>
    public string? Icon { get; init; }
}

public readonly record struct Temperature
{
    /// <summary>
    /// Temperature. Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit
    /// </summary>
    public double Temp { get; init; }

    /// <summary>
    /// Temperature. This temperature parameter accounts for the human perception of weather.
    /// Unit Default: Kelvin, Metric: Celsius, Imperial: Fahrenheit
    /// </summary>
    public double FeelsLike { get; init; }

    /// <summary>
    /// Minimum temperature at the moment.
    /// This is minimal currently observed temperature (within large megalopolises and urban areas).
    /// </summary>
    public double TempMin { get; init; }

    /// <summary>
    /// Maximum temperature at the moment.
    /// This is maximal currently observed temperature (within large megalopolises and urban areas).
    /// </summary>
    public double TempMax { get; init; }
}

public readonly record struct Location
{
    /// <summary>
    ///  Longitude of the location
    /// </summary>
    public double Lon { get; init; }

    /// <summary>
    /// Latitude of the location
    /// </summary>
    public double Lat { get; init; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; init; }
}
