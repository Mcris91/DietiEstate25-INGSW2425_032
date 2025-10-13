using DietiEstate.Application.Dtos.Filters;
using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User?>> GetUsersAsync(UserFilterDto filters, int? pageNumber, int? pageSize);

    Task<User?> GetUserByIdAsync(Guid userId);

    Task<User?> GetUserByEmailAsync(string email);

    Task AddUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync(User user);
}