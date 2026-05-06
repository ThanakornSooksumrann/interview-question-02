using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using Example.API.Common.Utilities;
using Example.API.Exceptions;
using Example.API.Models;
using Example.API.Models.Requests;
using Example.API.Models.Responses;
using Example.API.Services.Interfaces;
using Example.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Example.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthController : ControllerBase
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthController));

    private readonly IAuthService authService;

    public AuthController(IAuthService _authService)
    {
        LoggerUtil.GetLoggerThreadId();
        this.authService = _authService;
    }

    [HttpPost("register")]
    [SwaggerOperation(summary: "สมัครสมาชิก")]
    [SwaggerResponse((int)HttpStatusCode.OK,       Type = typeof(ApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            await authService.RegisterAsync(request);
            return Ok(new ApiResponse(ApiStatusCodes.SUCCESS, ApiMessages.SERVICE_SUCCESS));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ApiResponse(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new ApiResponse(ApiStatusCodes.INTERNAL_SERVER_ERROR, ApiMessages.SERVICE_ERROR));
        }
    }

    [HttpPost("login")]
    [SwaggerOperation(summary: "ลงชื่อเข้าใช้งาน")]
    [SwaggerResponse((int)HttpStatusCode.OK,       Type = typeof(ApiResponseWithData<AuthResponse>))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            AuthResponse result = await authService.LoginAsync(request);
            return Ok(new ApiResponseWithData<AuthResponse>(result));
        }
        catch (BusinessException ex)
        {
            return Unauthorized(new ApiResponse(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new ApiResponse(ApiStatusCodes.INTERNAL_SERVER_ERROR, ApiMessages.SERVICE_ERROR));
        }
    }

    [HttpGet("me")]
    [Authorize]
    [SwaggerOperation(summary: "ดึงข้อมูลผู้ใช้งานปัจจุบัน")]
    [SwaggerResponse((int)HttpStatusCode.OK,           Type = typeof(ApiResponseWithData<object>))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Type = typeof(ApiResponse))]
    public IActionResult GetCurrentUser()
    {
        try
        {
            string? username = User.FindFirstValue(ClaimTypes.Name);
            return Ok(new ApiResponseWithData<object>(new { username }));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new ApiResponse(ApiStatusCodes.INTERNAL_SERVER_ERROR, ApiMessages.SERVICE_ERROR));
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(summary: "ออกจากระบบ")]
    [SwaggerResponse((int)HttpStatusCode.OK,           Type = typeof(ApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                throw new BusinessException(ApiStatusCodes.UNAUTHORIZED, ApiMessages.UNAUTHORIZED);

            await authService.LogoutAsync(userId);
            return Ok(new ApiResponse(ApiStatusCodes.SUCCESS, ApiMessages.SERVICE_SUCCESS));
        }
        catch (BusinessException ex)
        {
            return Unauthorized(new ApiResponse(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message, ex);
            return StatusCode((int)HttpStatusCode.InternalServerError,
                new ApiResponse(ApiStatusCodes.INTERNAL_SERVER_ERROR, ApiMessages.SERVICE_ERROR));
        }
    }
}
