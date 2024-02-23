namespace Company.Assignment.Core.Options;

public class ExternalApisOptions : Dictionary<string, ExternalApiOptions>
{
    public const string ConfigurationKey = "ExternalApis";
}

public class ExternalApiOptions
{
    /// <summary>
    /// Base url of the external API
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// Api Key used to authenticate to external API
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Supported Authorization types: <see cref="AuthorizationType"/>
    /// </summary>
    public AuthorizationType AuthorizationType { get; set; }

    public Dictionary<string, string>? Headers { get; set; }
}

public enum AuthorizationType : short
{
    None,
    QueryParams,
    Bearer
}
