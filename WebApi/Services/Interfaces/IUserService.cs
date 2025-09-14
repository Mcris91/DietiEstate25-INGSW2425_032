using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Services.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateUserAsync(string email, string password);
    public string ValidatePassword(string password);
}