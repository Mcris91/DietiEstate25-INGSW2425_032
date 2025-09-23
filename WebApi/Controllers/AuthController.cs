using AutoMapper;
using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

/// <summary>
/// Controller responsible for handling authentication-related endpoints such as user signup, token generation,
/// and token refresh.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class AuthController(
    IPasswordService passwordService,
    IUserRepository userRepository,
    IUserService userService,
    IJwtService jwtService,
    IUserSessionService userSessionService,
    IMapper mapper) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await userService.AuthenticateUserAsync(request.Email, request.Password);
        if (user is null) return Unauthorized();

        var idToken = jwtService.GenerateJwtIdToken(user);
        var accessToken = jwtService.GenerateJwtAccessToken(user);
        var refreshToken = jwtService.GenerateJwtRefreshToken(user);

        var sessionId = await userSessionService.CreateSessionAsync(user, accessToken, refreshToken);
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            // Secure = true, // https only, uncomment in production
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromDays(30)
        };
        Response.Cookies.Append("session_id", sessionId, cookieOptions);
        Response.Cookies.Append("id_token", idToken, cookieOptions);

        return Ok(mapper.Map<LoginResponseDto>(user));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var sessionId = Request.Cookies["session_id"];
        if (!string.IsNullOrEmpty(sessionId))
            await userSessionService.InvalidateSessionAsync(new Guid(sessionId));
        Response.Cookies.Delete("session_id");
        Response.Cookies.Delete("id_token");
        return Ok();
    }
    
}