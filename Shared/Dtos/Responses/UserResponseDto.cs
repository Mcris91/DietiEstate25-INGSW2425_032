using System.ComponentModel.DataAnnotations;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.Shared.Dtos.Responses;

public class UserResponseDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Client;
}