using System.Text.Json.Serialization;

namespace Company.Assignment.Common.Dtos;

public readonly record struct ApiResponse<T>
{

    public bool Success { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorMessage { get; init; }
}
