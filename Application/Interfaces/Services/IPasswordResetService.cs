using DietiEstate.Core.Entities.AuthModels;

namespace DietiEstate.Application.Interfaces.Services;

public interface IPasswordResetService
{
    Task<PasswordReset?> GetResetRequestAsync(string email);
    Task<PasswordReset?> CreatePasswordResetRequestAsync(string email);
    Task InvalidateResetTokenAsync(string email);
    Task<bool> ValidateResetTokenAsync(string email, int token);
}