namespace Example.API.Utils;

public static class ApiMessages
{
    public const string SERVICE_SUCCESS              = "Success.";
    public const string SERVICE_ERROR               = "An error occurred. Please try again later.";

    public const string UNAUTHORIZED                = "Unauthorized.";
    public const string INVALID_REQUEST_DATA        = "Invalid request data.";

    public const string SECURITY_INVALID_LOGIN      = "Invalid username or password.";
    public const string SECURITY_INVALID_USER_NAME  = "Username already exists.";
    public const string SECURITY_INVALID_ACCESS_TOKEN  = "Invalid access token.";
    public const string SECURITY_INVALID_REFRESH_TOKEN = "Invalid refresh token.";
    public const string ACCESS_TOKEN_EXPIRED        = "Access token expired.";
    public const string PASSWORD_MISMATCH           = "Password and Confirm Password do not match.";
}
