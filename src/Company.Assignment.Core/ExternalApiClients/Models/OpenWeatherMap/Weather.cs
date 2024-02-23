namespace Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

public readonly record struct Weather
{
    /// <summary>
    /// Weather condition id
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Group of weather parameters (Rain, Snow, Clouds etc.)
    /// </summary>
    public string Main { get; init; }

    /// <summary>
    /// Weather condition within the group
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Weather icon id
    /// </summary>
    public string Icon { get; init; }
}

public readonly record struct Rain
{
    /// <summary>
    /// Rain volume for the last 1 hour, mm.
    /// Please note that only mm as units of measurement are available for this parameter
    /// </summary>
    public double OneH { get; init; }
}

public readonly record struct Wind
{
    /// <summary>
    /// Wind speed. Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour
    /// </summary>
    public double Speed { get; init; }

    /// <summary>
    ///  Wind direction, degrees (meteorological)
    /// </summary>
    public double Deg { get; init; }

    /// <summary>
    /// Wind gust. Unit Default: meter/sec, Metric: meter/sec, Imperial: miles/hour
    /// </summary>
    public double Gust { get; init; }
}

public readonly record struct Clouds
{
    /// <summary>
    /// Cloudiness, %
    /// </summary>
    public double All { get; init; }
}
