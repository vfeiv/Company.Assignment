using System.Text.Json;

namespace Company.Assignment.Core.Tests;

public static class TestHelpers
{
    private static JsonSerializerOptions? _jsonSerializerOptions;

    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        _jsonSerializerOptions ??= new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        return _jsonSerializerOptions;
    }
}
