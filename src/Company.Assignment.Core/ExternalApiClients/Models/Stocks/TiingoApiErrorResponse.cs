namespace Company.Assignment.Core.ExternalApiClients.Models.Stocks;

public readonly record struct TiingoApiErrorResponse
{
    public string Detail { get; init; }
}
