using DietiEstate.WebApi.Services.Interfaces;

namespace DietiEstate.WebApi.Services.Implementations;

/// <summary>
/// A service that provides password hashing and verification functionality using the BCrypt algorithm.
/// </summary>
public class BCryptPasswordService : IPasswordService
{
    private const int WorkFactor = 12;
    
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch (Exception)
        {
            return false;
        }
    }
}