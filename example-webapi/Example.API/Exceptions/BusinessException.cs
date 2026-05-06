namespace Example.API.Exceptions;

public class BusinessException : Exception
{
    public int Code { get; }

    public BusinessException(int code, string message) : base(message)
    {
        Code = code;
    }
}
