using System.Net;
using System.Text.Json.Serialization;

namespace Company.Assignment.Common.Dtos;

public readonly record struct ApiResponse<T>
{

    public bool Success { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ErrorMessage { get; init; }
}