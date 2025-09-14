using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using DietiEstate.WebApi.Services.Interfaces;

namespace DietiEstate.WebApi.Services.Implementations;

public class UserService(
    IUserRepository userRepository,
    IPasswordService passwordService) : IUserService
{
    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await userRepository.GetUserByEmailAsync(email);
        if (user is not null) 
            return passwordService.VerifyPassword(password, user.Password) ? user : null;
        
        // Hashing simulation to prevent timing attacks
        passwordService.VerifyPassword(password, "dummy_password");
        return null;
    }

    public string ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "Password cannot be empty";
        if (password.Length < 8)
            return "Password must be at least 8 characters";
        if (!password.Any(char.IsUpper))
            return "Password must contain at least one uppercase letter";
        if (!password.Any(char.IsLower))
            return "Password must contain at least one lowercase letter";
        if (!password.Any(char.IsDigit))
            return "Password must contain at least one digit";
        
        return "";
    }
}