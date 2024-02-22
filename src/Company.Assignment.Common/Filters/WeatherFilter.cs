using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Company.Assignment.Common.Filters;

public readonly record struct WeatherFilter
{
    public WeatherFilter() { }

    public double Lon { get; init; } = 23.45005;

    public double Lat { get; init; } = 37.58596;

    public static ValueTask<WeatherFilter?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var isLatParsed = double.TryParse(context.Request.Query["lat"], out var lat);
        var isLonParsed = double.TryParse(context.Request.Query["lon"], out var lon);

        if (!isLatParsed || !isLonParsed)
        {
            return new ValueTask<WeatherFilter?>(new WeatherFilter());
        }

        return ValueTask.FromResult<WeatherFilter?>(new WeatherFilter
        {
            Lat = lat,
            Lon = lon
        });
    }
}
