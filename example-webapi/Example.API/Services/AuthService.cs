using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Example.API.Common.Utilities;
using Example.API.Entities;
using Example.API.Exceptions;
using Example.API.Models.Requests;
using Example.API.Models.Responses;
using Example.API.Repositories.Interfaces;
using Example.API.Services.Interfaces;
using Example.API.Utils;
using Microsoft.IdentityModel.Tokens;

namespace Example.API.Services;

public class AuthService : IAuthService
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthService));

    private readonly IUserRepository      userRepository;
    private readonly IUserTokenRepository userTokenRepository;
    private readonly IConfiguration       configuration;

    public AuthService(
        IUserRepository      _userRepository,
        IUserTokenRepository _userTokenRepository,
        IConfiguration       _configuration)
    {
        LoggerUtil.GetLoggerThreadId();
        this.userRepository      = _userRepository;
        this.userTokenRepository = _userTokenRepository;
        this.configuration       = _configuration;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (request.Password != request.ConfirmPassword)
                throw new BusinessException(ApiStatusCodes.ERROR_BUSINESS, ApiMessages.PASSWORD_MISMATCH);

            if (await userRepository.ExistsAsync(request.Username))
                throw new BusinessException(ApiStatusCodes.ERROR_BUSINESS, ApiMessages.SECURITY_INVALID_USER_NAME);

            User user = new User
            {
                Username     = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt    = DateTime.UtcNow
            };

            await userRepository.AddAsync(user);
            await userRepository.SaveChangesAsync();
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            User? user = await userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new BusinessException(ApiStatusCodes.UNAUTHORIZED, ApiMessages.SECURITY_INVALID_LOGIN);

            string accessToken  = GenerateAccessToken(user);
            string refreshToken = GenerateRefreshToken();

            int refreshTokenExpiryDays = configuration.GetValue<int>("Jwt:RefreshTokenExpiryDays");

            UserToken userToken = new UserToken
            {
                UserId       = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt    = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
                IsRevoked    = false,
                CreatedAt    = DateTime.UtcNow
            };

            await userTokenRepository.AddAsync(userToken);
            await userTokenRepository.SaveChangesAsync();

            return new AuthResponse
            {
                Token        = accessToken,
                RefreshToken = refreshToken,
                Username     = user.Username
            };
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    public async Task LogoutAsync(int userId)
    {
        try
        {
            await userTokenRepository.RevokeAllByUserIdAsync(userId);
            await userTokenRepository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            throw;
        }
    }

    private string GenerateAccessToken(User user)
    {
        byte[]               keyBytes = Encoding.ASCII.GetBytes(configuration["Jwt:Secret"]!);
        SymmetricSecurityKey key      = new SymmetricSecurityKey(keyBytes);
        SigningCredentials   creds    = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        int accessTokenExpiryHours = configuration.GetValue<int>("Jwt:AccessTokenExpiryHours");

        JwtSecurityToken token = new JwtSecurityToken(
            claims:             claims,
            expires:            DateTime.UtcNow.AddHours(accessTokenExpiryHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
