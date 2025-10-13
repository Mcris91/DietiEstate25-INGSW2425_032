namespace DietiEstate.Application.Interfaces.Services;

public interface IPasswordService
{

    string HashPassword(string password);

    bool VerifyPassword(string password, string hashedPassword);
    
    string ValidatePasswordStrength(string password);
}