namespace Example.API.Utils;

public static class ApiStatusCodes
{
    public const int SUCCESS                    = 200;
    public const int ERROR_BUSINESS             = 400;
    public const int UNAUTHORIZED               = 401;
    public const int NOT_FOUND                  = 404;
    public const int ERROR_EXPIRE_ACCESS_TOKEN  = 419;
    public const int INTERNAL_SERVER_ERROR      = 500;
}
