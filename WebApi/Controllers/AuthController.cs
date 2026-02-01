using AutoMapper;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.AgencyModels;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class AuthController(
    //IUserVerificationRepository userVerificationRepository,
    IPasswordResetService passwordResetService,
    IUserSessionService userSessionService,
    IAgencyRepository agencyRepository,
    IPasswordService passwordService,
    IUserRepository userRepository,
    IEmailService emailService,
    IUserService userService,
    IJwtService jwtService,
    //IBackgroundJobClient jobClient,
    IMapper mapper) : Controller
{
    [HttpPost("register-agency")]
    public async Task<IActionResult> RegisterAgency([FromBody] RegisterAgencyDto request)
    {
        if (await userRepository.GetUserByEmailAsync(request.Email) is not null)
            return BadRequest("Email already exists.");
        
        var agency = new Agency()
        {
            Name = request.Name
        };

        var administrator = new User()
        {
            Email = request.Email.ToLowerInvariant(),
            Password = passwordService.HashPassword(request.Password),
            AgencyId = agency.Id,
            Role = UserRole.SuperAdmin
        };
        
        agency.Administrator = administrator;
        
        await agencyRepository.AddAgencyAsync(agency);

        return Ok();
    }
    
    
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

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutFromAllDevices()
    {
        var sessionId = Request.Cookies["session_id"];
        if (string.IsNullOrEmpty(sessionId))
            return Unauthorized();
        
        var session = await userSessionService.GetSessionAsync(new Guid(sessionId));
        if (session is null) 
            return Unauthorized();

        await userSessionService.InvalidateAllUserSessionsAsync(session.UserId);
        Response.Cookies.Delete("session_id");
        Response.Cookies.Delete("id_token");
        return Ok();
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] UserRequestDto request)
    {
        if (await userRepository.GetUserByEmailAsync(request.Email) is not null)
            return BadRequest("Email already exists.");
        
        var passwordValidation = passwordService.ValidatePasswordStrength(request.Password);
        if (!string.IsNullOrWhiteSpace(passwordValidation))
            return BadRequest(passwordValidation);
        
        var user = mapper.Map<User>(request);
        user.Id = Guid.NewGuid();
        user.Email = user.Email.ToLowerInvariant();
        user.Password = passwordService.HashPassword(request.Password);
        await userRepository.AddUserAsync(user);
        
        var userVerification = new UserVerification()
        {
            UserId = user.Id
        };
        //await userVerificationRepository.AddVerificationAsync(userVerification);

        var emailData = await emailService.PrepareEmailAsync(EmailType.Verification, user.FirstName, user.Email);
        //jobClient.Enqueue(() => emailService.SendEmailAsync(emailData));
        
        return CreatedAtAction(
            actionName: "GetUserById", 
            controllerName: "User",  
            routeValues: new { userId = user.Id }, 
            value: user);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        if (await userRepository.GetUserByEmailAsync(request.Email) is not { } user) return Ok();
        
        var resetRequestToken = await passwordResetService.CreatePasswordResetRequestAsync(request.Email);
        var emailData = await emailService.PrepareEmailAsync(EmailType.PasswordReset, user.FirstName, user.Email);
        //jobClient.Enqueue(() => emailService.SendEmailAsync(emailData));
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        var passwordReset = await passwordResetService.GetResetRequestAsync(request.Email);
        if (passwordReset is null || await userRepository.GetUserByEmailAsync(passwordReset.Email) is not { } user)
            return NotFound();
        
        user.Password = passwordService.HashPassword(request.Password);
        await userRepository.UpdateUserAsync(user);
        
        await userSessionService.InvalidateAllUserSessionsAsync(user.Id);
        Response.Cookies.Delete("session_id");
        Response.Cookies.Delete("id_token");
        await passwordResetService.InvalidateResetTokenAsync(request.Email);
        
        return Ok();
    }

    [HttpPost("validate-reset-token")]
    public async Task<IActionResult> ValidateResetToken([FromBody] ValidatePasswordResetTokenRequestDto request)
    {
        if (!await passwordResetService.ValidateResetTokenAsync(request.Email, request.Token))
            return BadRequest("Token expired or not found");
        
        return Ok();
    }

}








