using Example.API.Models.Requests;
using Example.API.Models.Responses;

namespace Example.API.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task LogoutAsync(int userId);
}
