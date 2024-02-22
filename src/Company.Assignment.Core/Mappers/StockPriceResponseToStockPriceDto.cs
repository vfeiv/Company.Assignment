using Company.Assignment.Common.Dtos;
using Company.Assignment.Core.ExternalApiClients.Models.Stocks;

namespace Company.Assignment.Core.Mappers;

public class StockPriceResponseToStockPriceDto : BaseMapper<StockPriceResponse, StockPriceDto>
{
    public override StockPriceDto Map(StockPriceResponse from) =>
        new()
        {
            Date = from.Date,
            Open = from.Open,
            High = from.High,
            Low = from.Low,
            Close = from.Close,
            Volume = from.Volume,
            AdjOpen = from.AdjOpen,
            AdjHigh = from.AdjHigh,
            AdjLow = from.AdjLow,
            AdjClose = from.AdjClose,
            AdjVolume = from.AdjVolume,
            DivCash = from.DivCash,
            SplitFactor = from.SplitFactor
        };
}
