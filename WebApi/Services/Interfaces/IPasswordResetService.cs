using DietiEstate.Shared.Models.AuthModels;

namespace DietiEstate.WebApi.Services.Interfaces;

public interface IPasswordResetService
{
    Task<PasswordReset?> GetResetRequestAsync(string email);
    Task<PasswordReset?> CreatePasswordResetRequestAsync(string email);
    Task InvalidateResetTokenAsync(string email);
    Task<bool> ValidateResetTokenAsync(string email, int token);
}