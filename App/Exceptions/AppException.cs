using System.Net;

namespace lanstreamer_api.App.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }
    
    public AppException(HttpStatusCode httpStatusCode, string? message) : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }
}
