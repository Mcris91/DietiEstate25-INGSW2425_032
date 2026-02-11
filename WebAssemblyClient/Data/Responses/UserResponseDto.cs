using WebAssemblyClient.Data.Common;

namespace WebAssemblyClient.Data.Responses;

public class UserResponseDto
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public UserRole Role { get; init; }
}