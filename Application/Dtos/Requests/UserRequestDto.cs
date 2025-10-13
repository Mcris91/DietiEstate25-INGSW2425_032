using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Dtos.Requests;

public class UserRequestDto
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public UserRole Role { get; init; } = UserRole.Client;
}