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
    IMapper mapper) : Controller
{
    /// <summary>
    /// Handles user sign-up requests for clients. This method validates the user input,
    /// checks if the email already exists in the repository, validates the password,
    /// hashes the password, and stores the new user in the repository.
    /// </summary>
    /// <param name="request">The request data containing email, password, and role.</param>
    /// <returns>An IActionResult indicating the result of the sign-up operation.
    /// Returns a 201 Created response if successful, or a 400 Bad Request response for errors such as
    /// non-client roles, duplicate emails, or invalid password validation.</returns>
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserRequestDto request)
    {
        if (request.Role != UserRole.Client)
            return BadRequest(new {error = "Only clients can be created."});
        
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
        return CreatedAtAction(nameof(GetTokenPair), new { response });
    }

    /// <summary>
    /// Generates an access token and a refresh token for the user. This method authenticates
    /// the user based on the provided email and password and subsequently generates the token pair
    /// if the authentication is successful.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <returns>An IActionResult containing the login response with the generated access and refresh tokens.
    /// Returns 200 OK if the operation is successful, or 400 Bad Request if authentication fails.</returns>
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenPair([FromBody] LoginRequestDto request)
    {
        var user = await userService.AuthenticateUserAsync(request.Email, request.Password);
        if (user is null) return BadRequest(new {error = "Invalid email or password."});
        
        return Ok(new LoginResponseDto() {
            Access = jwtService.GenerateJwtAccessToken(user),
            Refresh = jwtService.GenerateJwtRefreshToken(user)
        });
    }

    /// <summary>
    /// Refreshes the access token by validating the provided refresh token, checking the user's identity,
    /// and generating a new access token for the user if the operation is successful.
    /// </summary>
    /// <param name="refreshToken">The JWT refresh token provided by the client for token refresh.</param>
    /// <returns>An IActionResult containing the new access token in the response body if the operation succeeds.
    /// Returns a 401 Unauthorized response for invalid or expired tokens, and a 400 Bad Request response if the user
    /// associated with the refresh token is not found.</returns>
    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshAccessToken([FromBody] string refreshToken)
    {
        var principal = jwtService.ValidateJwtRefreshToken(refreshToken);
        if (principal is null) 
            return Unauthorized();

        var userId = principal.Identity!.Name;
        if (userId == null) 
            return Unauthorized();
        
        var user = await userRepository.GetUserByIdAsync(Guid.Parse(userId));
        if (user == null)
            return BadRequest(new {error = "Invalid refresh token. User not found."});
        
        return Ok(new { Access = jwtService.GenerateJwtAccessToken(user) });
    }
}