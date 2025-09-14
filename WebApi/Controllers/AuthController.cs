using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IJwtService jwtService) : Controller
{
    [HttpPost("signup")]
    public IActionResult SignUp([FromBody] SignUpRequestDto request)
    {
        // TODO: Implement signup logic (create user, add to database, hash password, etc.)
        var response = new LoginRequestDto
        {
            Email = request.Email,
            Password = request.Password,
        };
        return CreatedAtAction(nameof(GetTokenPair), new { }, response);
    }

    [HttpPost("token")]
    public IActionResult GetTokenPair([FromBody] LoginRequestDto request)
    {
        // TODO: Implement login logic (user repository and authentication)
        var userId = Guid.NewGuid();
        var response = new LoginResponseDto()
        {
            Access = jwtService.GenerateJwtAccessToken(userId.ToString()),
            Refresh = jwtService.GenerateJwtRefreshToken(userId.ToString())
        };
        return Ok(response);
    }
    
    [HttpPost("token/refresh")]
    public IActionResult RefreshAccessToken([FromBody] string refreshToken)
    {
        var principal = jwtService.ValidateJwtRefreshToken(refreshToken);
        if (principal is null) return Unauthorized();

        var userId = principal.Identity!.Name;
        if (userId != null)
            return Ok(new { access = jwtService.GenerateJwtAccessToken(userId) });
        
        return Unauthorized();
    }
}