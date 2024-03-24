namespace RouteWise.Service.Exceptions;

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
    public CustomException(string message, Exception innerException) : base(message, innerException) { }
    public int StatusCode = 422;
}