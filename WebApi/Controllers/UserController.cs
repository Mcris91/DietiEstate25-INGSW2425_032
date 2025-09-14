using AutoMapper;
using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Dtos.Requests;
using DietiEstate.Shared.Dtos.Responses;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController(
    IMapper mapper,
    IPasswordService passwordService,
    IUserService userService,
    IUserRepository userRepository) : Controller
{
    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<UserResponseDto>>> GetUsers(
        [FromQuery] UserFilterDto filterDto,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize)
    {
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
    public async Task<ActionResult<UserResponseDto>> GetUserById(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user is null) 
            return NotFound();
        
        return Ok(mapper.Map<UserResponseDto>(user));
    }

    [HttpPost]
    public async Task<ActionResult> PostUser(UserRequestDto request)
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

        return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, mapper.Map<UserResponseDto>(user));
    }

    [HttpPut("userId:Guid")]
    public async Task<IActionResult> PutUser(UserRequestDto request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        var user = await userRepository.GetUserByIdAsync(userId);
        if (user is null) 
            return NotFound();
        
        await userRepository.DeleteUserAsync(user);
        return NoContent();   
    }
}