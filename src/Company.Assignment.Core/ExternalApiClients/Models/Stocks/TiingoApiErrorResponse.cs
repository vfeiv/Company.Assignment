namespace Company.Assignment.Core.ExternalApiClients.Models.Stocks;

internal readonly record struct TiingoApiErrorResponse
{
    public string Detail { get; init; }
}
