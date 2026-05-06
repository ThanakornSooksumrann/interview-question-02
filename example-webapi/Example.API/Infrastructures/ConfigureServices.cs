using System.Net;
using System.Text;
using System.Text.Json;
using Example.API.Data;
using Example.API.Models;
using Example.API.Repositories;
using Example.API.Repositories.Interfaces;
using Example.API.Services;
using Example.API.Services.Interfaces;
using Example.API.Utils;
using Example.API.Utils.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Example.API.Infrastructures;

public static class ConfigureServices
{
    public static void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        #region CORS
        string[] allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", builder =>
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials());
        });
        #endregion

        #region Core
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(SwaggerHelper.ConfigureSwaggerGen);
        services.AddControllers();
        services.AddHttpContextAccessor();
        #endregion

        #region Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        #endregion

        #region Services / Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserTokenRepository, UserTokenRepository>();
        services.AddScoped<IAuthService, AuthService>();
        #endregion

        #region JWT Authentication
        string jwtSecret = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        byte[] key = Encoding.ASCII.GetBytes(jwtSecret);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";

                            var body = new ApiResponse(ApiStatusCodes.ERROR_EXPIRE_ACCESS_TOKEN, ApiMessages.ACCESS_TOKEN_EXPIRED);
                            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(body));
                            return context.Response.Body.WriteAsync(bytes).AsTask();
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        #endregion
    }
}
