using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.Mappers;
using FluentAssertions;
using Xunit;

namespace Company.Assignment.Core.Tests.Mappers;

public class CurrentWeatherResponseToWeatherDtoTests
{
    private readonly CurrentWeatherResponseToWeatherDto _mapper = new();

    [Theory, AutoData]
    public void ShouldMapWeatherDto_WhenFromCurrentWeatherResponseIsNotNull(
        string name,
        double lat,
        double lon,
        string description,
        string icon,
        string main,
        double temp,
        double feelsLike,
        double tempMin,
        double tempMax,
        double humidity,
        double pressure)
    {
        var from = new CurrentWeatherResponse
        {
            Name = name,
            Coord = new Coord
            {
                Lat = lat,
                Lon = lon
            },
            Weather =
            [
                new() {
                    Id = 1,
                    Description = description,
                    Icon = icon,
                    Main = main
                }
            ],
            Dt = 1708632788,
            Main = new Main
            {
                Temp = temp,
                TempMin = tempMin,
                TempMax = tempMax,
                FeelsLike = feelsLike,
                Humidity = humidity,
                Pressure = pressure
            }
        };

        var to = _mapper.Map(from);

        to.Location.City.Should().Be(from.Name);
        to.Location.Lat.Should().Be(from.Coord.Lat);
        to.Location.Lon.Should().Be(from.Coord.Lon);

        to.Should().NotBeNull();
        to.WeatherConditions.Should().NotBeNull();
        to.WeatherConditions!.Count.Should().Be(1);
        to.WeatherConditions![0].Description.Should().Be(from.Weather[0].Description);
        to.WeatherConditions![0].Icon.Should().Be(from.Weather[0].Icon);
        to.WeatherConditions![0].Condition.Should().Be(from.Weather[0].Main);

        to.Should().NotBeNull();
        to.Date.Should().NotBeNull();
        to.Date!.Should().Be(DateTimeOffset.FromUnixTimeSeconds(from.Dt.Value));


        to.Temperature.Temp!.Should().Be(from.Main.Temp);
        to.Temperature.TempMin!.Should().Be(from.Main.TempMin);
        to.Temperature.TempMax!.Should().Be(from.Main.TempMax);
        to.Temperature.FeelsLike!.Should().Be(from.Main.FeelsLike);
        to.Pressure!.Should().Be(from.Main.Pressure);
        to.Humidity!.Should().Be(from.Main.Humidity);
    }

    [Fact]
    public void ShouldReturnDefault_WhenFromIsDefault()
    {
        var to = _mapper.Map(default(CurrentWeatherResponse));

        to.Should().Be(default(WeatherDto));
    }
}
