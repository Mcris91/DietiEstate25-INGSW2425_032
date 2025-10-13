using DietiEstate.Application.Interfaces.Services;

namespace DietiEstate.Infrastracture.Services;

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
    
    public string ValidatePasswordStrength(string password)
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