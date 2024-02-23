namespace Company.Assignment.Common.Dtos;

public readonly record struct StockPriceDto
{
    /// <summary>
    /// The date this data pertains to.
    /// </summary>
    public DateTime Date { get; init; }

    /// <summary>
    /// The opening price for the asset on the given date.
    /// </summary>
    public double Open { get; init; }

    /// <summary>
    /// The high price for the asset on the given date.
    /// </summary>
    public double High { get; init; }

    /// <summary>
    /// The low price for the asset on the given date.
    /// </summary>
    public double Low { get; init; }

    /// <summary>
    /// The closing price for the asset on the given date.
    /// </summary>
    public double Close { get; init; }

    /// <summary>
    /// The number of shares traded for the asset.
    /// </summary>
    public double Volume { get; init; }

    /// <summary>
    /// The adjusted opening price for the asset on the given date.
    /// </summary>
    public double AdjOpen { get; init; }

    /// <summary>
    /// The adjusted high price for the asset on the given date.
    /// </summary>
    public double AdjHigh { get; init; }

    /// <summary>
    /// The adjusted low price for the asset on the given date.
    /// </summary>
    public double AdjLow { get; init; }

    /// <summary>
    /// The adjusted closing price for the asset on the given date.
    /// </summary>
    public double AdjClose { get; init; }

    /// <summary>
    /// The number of shares traded for the asset.
    /// </summary>
    public double AdjVolume { get; init; }

    /// <summary>
    /// The dividend paid out on "date" (note that "date" will be the "exDate" for the dividend).
    /// </summary>
    public double DivCash { get; init; }

    /// <summary>
    /// The factor used to adjust prices when a company splits, reverse splits, or pays a distribution.
    /// </summary>
    public double SplitFactor { get; init; }
}