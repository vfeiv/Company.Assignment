namespace Company.Assignment.Core.Options;

public class ExternalApisOptions : Dictionary<string, ExternalApiOptions>
{
    public const string ConfigurationKey = "ExternalApis";
}

public class ExternalApiOptions
{
    public required string BaseUrl { get; set; }

    public string? ApiKey { get; set; }

    public AuthorizationType AuthorizationType { get; set; }

    public Dictionary<string, string>? Headers { get; set; }
}

public enum AuthorizationType : short
{
    None,
    QueryParams,
    Bearer
}
