using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Interfaces.Services;

public interface IUserService
{
    Task<User?> AuthenticateUserAsync(string email, string password);

    public string ValidatePassword(string password);
}