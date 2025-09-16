using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Services.Interfaces;

/// <summary>
/// Represents a contract for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Authenticates a user by verifying the provided email and password.
    /// </summary>
    /// <param name="email">The email address of the user attempting to authenticate.</param>
    /// <param name="password">The password associated with the user's account.</param>
    /// <returns>A <see cref="User"/> object if authentication is successful; otherwise, null.</returns>
    Task<User?> AuthenticateUserAsync(string email, string password);

    /// <summary>
    /// Validates the specified password based on predefined rules.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>
    /// A string describing the validation failure if the password does not meet the requirements.
    /// Returns an empty string if the password is valid.
    /// </returns>
    public string ValidatePassword(string password);
}