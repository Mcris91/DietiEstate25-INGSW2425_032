using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;

namespace DietiEstate.WebApi.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordService passwordService)
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
}