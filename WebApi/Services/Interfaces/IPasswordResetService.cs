namespace DietiEstate.WebApi.Services.Interfaces;

public interface IPasswordResetService
{
    Task<string> GetResetRequestEmailAsync(Guid resetTokenId);
    Task<bool> CreatePasswordResetRequestAsync(string email);
    Task<bool> ValidateResetTokenAsync(Guid resetTokenId);
}