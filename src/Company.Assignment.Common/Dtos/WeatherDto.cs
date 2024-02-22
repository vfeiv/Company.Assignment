namespace Company.Assignment.Common.Dtos;

public readonly record struct WeatherDto
{
    public IReadOnlyList<WeatherCondition>? Conditions { get; init; }
    public Temperature Temperature { get; init; }
    public Wind Wind { get; init; }
    public double Pressure { get; init; }
    public double Humidity { get; init; }
    public Location Location { get; init; }
    public DateTimeOffset Date { get; init; }
}

public readonly record struct WeatherCondition
{
    public string? Condition { get; init; }
    public string? Description { get; init; }
    public string? Icon { get; init; }
}

public readonly record struct Temperature
{
    public double Temp { get; init; }

    public double FeelsLike { get; init; }

    public double TempMin { get; init; }

    public double TempMax { get; init; }
}

public readonly record struct Wind
{
    public double Speed { get; init; }

    public double Deg { get; init; }

    public double Gust { get; init; }
}

public readonly record struct Location
{
    public double Lon { get; init; }

    public double Lat { get; init; }

    public string? City { get; init; }
}
