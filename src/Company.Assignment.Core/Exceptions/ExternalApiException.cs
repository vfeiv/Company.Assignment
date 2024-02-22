using System.Net;

namespace Company.Assignment.Core.Exceptions;

public class ExternalApiException : HttpRequestException
{
    public const string ERROR_RESPONSE_KEY = "ErrorResponse";

    public ExternalApiException(string message, 
        string errorResponse, 
        HttpRequestException? inner, 
        HttpStatusCode statusCode) : base(message, inner, statusCode)
    {
        Data.Add(ERROR_RESPONSE_KEY, errorResponse);
    }
}
