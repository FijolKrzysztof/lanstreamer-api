using System.Net;

namespace lanstreamer_api.App.Exceptions;

public class AppException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }
    public Exception? Exception { get; }
    
    public AppException(HttpStatusCode httpStatusCode, string? message, Exception? exception = null) : base(message)
    {
        HttpStatusCode = httpStatusCode;
        Exception = exception;
    }
}
