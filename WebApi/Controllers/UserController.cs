using AutoMapper;
using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Application.Dtos.Requests;
using DietiEstate.Application.Dtos.Responses;
using DietiEstate.Application.Interfaces.Repositories;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Enums;
using DietiEstate.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UserController(
    IMapper mapper,
    IPasswordService passwordService,
    IUserRepository userRepository) : Controller
{
    [HttpGet]
    [Authorize(Policy = "ReadUser")]
    public async Task<ActionResult<PagedResponseDto<UserResponseDto>>> GetUsers(
        [FromQuery] UserFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {

        filterDto.ApplyRoleFilters(User.GetRole(), User.GetUserId(), User.GetAgencyId());
        
        if (pageNumber.HasValue ^ pageSize.HasValue) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be provided for pagination."});
        if (pageNumber <= 0 || pageSize <= 0) 
            return BadRequest(new {error = "Both pageNumber and pageSize must be greater than zero."});

        var listings = await userRepository.GetUsersAsync(filterDto, pageNumber, pageSize);
        return Ok(new PagedResponseDto<UserResponseDto>(
            listings.ToList().Select(mapper.Map<UserResponseDto>), 
            pageSize, pageNumber));
    }

    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "ReadUser")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user is null)
            return NotFound();

        return Ok(mapper.Map<UserResponseDto>(user));
    }
    
    [HttpPost("create-support-admin")]
    [Authorize(Policy = "WriteSupportAdmin")]
    public async Task<ActionResult> CreateSupportAdminUser(UserRequestDto request)
    {
        var agencyId = User.GetAgencyId();
        if (agencyId == Guid.Empty)
            return Unauthorized();

        if (await userRepository.GetUserByEmailAsync(request.Email) is not null)
            return BadRequest("La mail è gia in uso");

        var passwordValidation = passwordService.ValidatePasswordStrength(request.Password);
        if (passwordValidation != "")
            return BadRequest(new {error = passwordValidation});
        
        var user = mapper.Map<User>(request);
        user.Email = user.Email.ToLowerInvariant();
        user.Password = passwordService.HashPassword(request.Password);
        user.Role = UserRole.SupportAdmin;
        user.AgencyId = agencyId;
        await userRepository.AddUserAsync(user);

        return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, mapper.Map<UserResponseDto>(user));
    }

    [HttpPost("create-agent")]
    [Authorize(Policy = "WriteAgent")]
    public async Task<ActionResult> CreateAgentUser([FromBody] UserRequestDto request)
    {
        var agencyId = User.GetAgencyId();
        if (agencyId == Guid.Empty)
            return Unauthorized();
        
        var passwordValidation = passwordService.ValidatePasswordStrength(request.Password);
        if (passwordValidation != "")
            return BadRequest(new {error = passwordValidation});
        
        var user = mapper.Map<User>(request);
        user.Email = user.Email.ToLowerInvariant();
        user.Password = passwordService.HashPassword(request.Password);
        user.Role = UserRole.EstateAgent;
        user.AgencyId = agencyId;
        await userRepository.AddUserAsync(user);

        return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, mapper.Map<UserResponseDto>(user));
    }

    [HttpPut("{userId:Guid}")]
    [Authorize(Policy = "WriteUser")]
    public async Task<IActionResult> PutUser(Guid userId, [FromBody] UserRequestDto request)
    {
        if (await userRepository.GetUserByIdAsync(userId) is not { } existingUser)
            return NotFound();
        existingUser = mapper.Map(request, existingUser);
        await userRepository.UpdateUserAsync(existingUser);
        return Ok(mapper.Map<UserResponseDto>(existingUser));   
    }

    [HttpDelete]
    [Authorize(Policy="DeleteUser")]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user is null) 
            return NotFound();
        
        await userRepository.DeleteUserAsync(user);
        return NoContent();   
    }

    [HttpPut("change-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequestDto request)
    {
        var userId = User.GetUserId();
        if (userId == Guid.Empty)
            return Unauthorized();
        if (await userRepository.GetUserByIdAsync(userId) is not { } user)
            return NotFound();

        if (!passwordService.VerifyPassword(request.OldPassword, user.Password))
            return BadRequest("Password errata");
        
        user.Password = passwordService.HashPassword(request.NewPassword);
        await userRepository.UpdateUserAsync(user);
        return Ok();
    }
    
}