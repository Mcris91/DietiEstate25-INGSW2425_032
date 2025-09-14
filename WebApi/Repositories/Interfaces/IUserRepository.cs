using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User?>> GetUsersAsync(UserFilterDto filters, int? pageNumber, int? pageSize);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}