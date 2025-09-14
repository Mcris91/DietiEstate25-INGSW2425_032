using AutoMapper;
using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IPasswordService passwordService,
    IUserRepository userRepository,
    IUserService userService,
    IJwtService jwtService,
    IMapper mapper) : Controller
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
    {
        if (await userRepository.GetUserByEmailAsync(request.Email) is not null)
            return BadRequest(new {error = "Email already exists."});
        
        var passwordValidation = userService.ValidatePassword(request.Password);
        if (passwordValidation != "")
            return BadRequest(new {error = passwordValidation});
        
        var user = mapper.Map<User>(request);
        user.Email = user.Email.ToLowerInvariant();
        user.Password = passwordService.HashPassword(request.Password);
        await userRepository.AddUserAsync(user);
        
        var response = new LoginRequestDto
        {
            Email = user.Email,
            Password = user.Password,
        };
        return CreatedAtAction(nameof(GetTokenPair), new { }, response);
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenPair([FromBody] LoginRequestDto request)
    {
        var user = await userService.AuthenticateUserAsync(request.Email, request.Password);
        if (user is null) return BadRequest(new {error = "Invalid email or password."});
        
        return Ok(new LoginResponseDto() {
            Access = jwtService.GenerateJwtAccessToken(user.Id.ToString()),
            Refresh = jwtService.GenerateJwtRefreshToken(user.Id.ToString())
        });
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