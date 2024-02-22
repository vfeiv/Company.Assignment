namespace Company.Assignment.Core.ExternalApiClients.Models;

public readonly record struct ExternalApiResponse<T>
{
    public bool Success { get; init; }

    public T? Data { get; init; }

    public string? ErrorMessage { get; init; }
}
