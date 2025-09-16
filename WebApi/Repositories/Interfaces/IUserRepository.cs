using DietiEstate.Shared.Dtos.Filters;
using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Repositories.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a collection of users based on the provided filters, pagination, and sorting parameters.
    /// </summary>
    /// <param name="filters">An object containing filter criteria such as first name, last name, email, role, and sorting preferences.</param>
    /// <param name="pageNumber">The page number for the returned collection, used in conjunction with pageSize to enable pagination. Can be null for no pagination.</param>
    /// <param name="pageSize">The number of items per page for the returned collection, used in conjunction with pageNumber to enable pagination. Can be null for no pagination.</param>
    /// <returns>An asynchronous task that returns an enumerable collection of users that match the provided parameters.</returns>
    Task<IEnumerable<User?>> GetUsersAsync(UserFilterDto filters, int? pageNumber, int? pageSize);

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to be retrieved.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    Task<User?> GetUserByIdAsync(Guid userId);

    /// Retrieves a user based on the provided email address.
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>A <c>User</c> object if a user with the specified email exists; otherwise, <c>null</c>.</returns>
    Task<User?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Asynchronously adds a new user to the database.
    /// </summary>
    /// <param name="user">The user object to be added, which contains the necessary user details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddUserAsync(User user);

    /// <summary>
    /// Updates an existing user's information in the database.
    /// </summary>
    /// <param name="user">The user entity containing updated information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateUserAsync(User user);

    /// <summary>
    /// Deletes a specified user from the system.
    /// </summary>
    /// <param name="user">The user entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteUserAsync(User user);
}