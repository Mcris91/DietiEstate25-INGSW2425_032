namespace DietiEstate.WebApi.Services.Interfaces;

/// <summary>
/// Provides methods for handling password-related operations such as hashing and verification.
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Hashes the specified plaintext password using a cryptographic hashing algorithm.
    /// </summary>
    /// <param name="password">The plaintext password to be hashed.</param>
    /// <returns>A hashed representation of the password as a string.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verifies whether the provided password matches the given hashed password.
    /// </summary>
    /// <param name="password">The plaintext password to be verified.</param>
    /// <param name="hashedPassword">The hashed version of the password for comparison.</param>
    /// <returns>True if the password matches the hashed password, otherwise false.</returns>
    bool VerifyPassword(string password, string hashedPassword);
    
    string ValidatePasswordStrength(string password);
}