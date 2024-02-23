using System.Net;
using System.Text.Json.Serialization;

namespace Company.Assignment.Common.Dtos;

public readonly record struct ApiResponse<T>
{
    /// <summary>
    /// Success flag
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// <see cref="HttpStatusCode"/>
    /// </summary>
    public HttpStatusCode StatusCode { get; init; }

    /// <summary>
    /// Aggregated data. Returned only when <see cref="Success"/> is true
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; init; }

    /// <summary>
    /// Error message . Returned only when <see cref="Success"/> is false
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ErrorMessage { get; init; }
}