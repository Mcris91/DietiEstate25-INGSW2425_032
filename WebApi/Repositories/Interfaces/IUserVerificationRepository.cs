using DietiEstate.Shared.Models.UserModels;

namespace DietiEstate.WebApi.Repositories.Interfaces;

public interface IUserVerificationRepository
{
    Task<UserVerification?> GetVerificationByTokenAsync(string token);
    
    Task<UserVerification?> GetVerificationByUserIdAsync(Guid userId);
    
    Task AddVerificationAsync(UserVerification verification);
    
    Task UpdateVerificationAsync(UserVerification verification);
    
    Task DeleteVerificationAsync(UserVerification verification);
}