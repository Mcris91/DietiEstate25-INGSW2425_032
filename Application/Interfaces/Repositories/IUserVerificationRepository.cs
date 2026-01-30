using DietiEstate.Core.Entities.UserModels;

namespace DietiEstate.Application.Interfaces.Repositories;

public interface IUserVerificationRepository
{
    Task<UserVerification?> GetVerificationByTokenAsync(string token);
    
    Task<UserVerification?> GetVerificationByUserIdAsync(Guid userId);
    
    Task AddVerificationAsync(UserVerification verification);
    
    Task UpdateVerificationAsync(UserVerification verification);
    
    Task DeleteVerificationAsync(UserVerification verification);
}