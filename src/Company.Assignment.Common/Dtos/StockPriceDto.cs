namespace Company.Assignment.Common.Dtos;

public readonly record struct StockPriceDto
{
    public DateTime Date { get; init; }

    public double Open { get; init; }

    public double High { get; init; }

    public double Low { get; init; }

    public double Close { get; init; }

    public double Volume { get; init; }

    public double AdjOpen { get; init; }

    public double AdjHigh { get; init; }

    public double AdjLow { get; init; }

    public double AdjClose { get; init; }

    public double AdjVolume { get; init; }

    public double DivCash { get; init; }

    public double SplitFactor { get; init; }
}