using AutoFixture.Xunit2;
using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.OpenWeatherMap;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;
using Company.Assignment.Core.Mappers;
using FluentAssertions;
using Xunit;

namespace Company.Assignment.Core.Tests.Mappers;

public class StockPriceResponseToStockPriceDtoTests
{
    private readonly StockPriceResponseToStockPriceDto _mapper = new();

    [Theory, AutoData]
    public void ShouldMapStockPriceDto_WhenFromStockPriceResponseIsNotNull(
        DateTime date,
        double open,
        double high,
        double low,
        double close,
        double volume,
        double adjOpen,
        double adjHigh,
        double adjLow,
        double adjClose,
        double adjVolume,
        double divCash,
        double splitFactor)
    {
        var from = new StockPriceResponse
        {
            Date = date,
            Open = open,
            High = high,
            Low = low,
            Close = close,
            Volume = volume,
            AdjOpen = adjOpen,
            AdjHigh = adjHigh,
            AdjLow = adjLow,
            AdjClose = adjClose,
            AdjVolume = adjVolume,
            DivCash = divCash,
            SplitFactor = splitFactor
        };

        var to = _mapper.Map(from);

        to.Date.Should().Be(from.Date);
        to.Open.Should().Be(from.Open);
        to.High.Should().Be(from.High);
        to.Low.Should().Be(from.Low);
        to.Close.Should().Be(from.Close);
        to.Volume.Should().Be(from.Volume);
        to.AdjOpen.Should().Be(from.AdjOpen);
        to.AdjHigh.Should().Be(from.AdjHigh);
        to.AdjLow.Should().Be(from.AdjLow);
        to.AdjClose.Should().Be(from.AdjClose);
        to.AdjVolume.Should().Be(from.AdjVolume);
        to.DivCash.Should().Be(from.DivCash);
        to.SplitFactor.Should().Be(from.SplitFactor);
    }

    [Fact]
    public void ShouldReturnDefault_WhenFromIsDefault()
    {
        var to = _mapper.Map(default(StockPriceResponse));

        to.Should().Be(default(StockPriceDto));
    }
}
