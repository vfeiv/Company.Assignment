using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

namespace Company.Assignment.Core.Mappers;

public class CurrentWeatherResponseToWeatherDto : BaseMapper<CurrentWeatherResponse, WeatherDto>
{
    public override WeatherDto Map(CurrentWeatherResponse from) =>
        new()
        {
            WeatherConditions = from.Weather?.Select(x => new WeatherCondition
            {
                Condition = x.Main,
                Description = x.Description,
                Icon = x.Icon
            }).ToList(),
            Temperature = new Temperature
            {
                Temp = from.Main.Temp,
                FeelsLike = from.Main.FeelsLike,
                TempMin = from.Main.TempMin,
                TempMax = from.Main.TempMax
            },
            Pressure = from.Main.Pressure,
            Humidity = from.Main.Humidity,
            Location = new Location
            {
                Lon = (double)from.Coord.Lon,
                Lat = (double)from.Coord.Lat,
                City = from.Name
            },
            Date = from.Dt != null ? DateTimeOffset.FromUnixTimeSeconds(from.Dt.Value) : null
        };
}
