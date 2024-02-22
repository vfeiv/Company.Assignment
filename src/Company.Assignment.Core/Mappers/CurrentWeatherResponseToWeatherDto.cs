﻿using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;

namespace Company.Assignment.Core.Mappers;

public class CurrentWeatherResponseToWeatherDto : BaseMapper<CurrentWeatherResponse, WeatherDto>
{
    public override WeatherDto Map(CurrentWeatherResponse from) =>
        new()
        {
            Conditions = from.Weather?.Select(x => new WeatherCondition
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
            Wind = new Common.Dtos.Wind
            {
                Speed = from.Wind.Speed,
                Deg = from.Wind.Deg,
                Gust = from.Wind.Gust
            },
            Pressure = from.Main.Pressure,
            Humidity = from.Main.Humidity,
            Location = new Location
            {
                Lon = (double)from.Coord.Lon,
                Lat = (double)from.Coord.Lat,
                City = from.Name
            },
            Date = DateTimeOffset.FromUnixTimeSeconds(from.Dt)
        };
}
