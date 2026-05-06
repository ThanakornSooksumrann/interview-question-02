using Example.API.Utils;

namespace Example.API.Models;

public class ApiResponse
{
    public int    Code    { get; set; }
    public string Message { get; set; }

    public ApiResponse(int code, string message)
    {
        Code    = code;
        Message = message;
    }
}

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }

    public ApiResponseWithData(T? data)
        : base(ApiStatusCodes.SUCCESS, ApiMessages.SERVICE_SUCCESS)
    {
        Data = data;
    }

    public ApiResponseWithData(int code, string message, T? data)
        : base(code, message)
    {
        Data = data;
    }
}
