namespace Company.Assignment.Core.ExternalApiClients.Models.News;

public readonly record struct NewsApiErrorResponse
{
    public string Status { get; init; }
    public string Code { get; init; }
    public string Message { get; init; }
}
