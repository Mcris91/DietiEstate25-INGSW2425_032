using DietiEstate.Shared.Enums;
using DietiEstate.Shared.Models.UserModels;
using DietiEstate.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietiEstate.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class UserVerificationController(
    IUserVerificationRepository userVerificationRepository) : Controller
{
    [HttpPost("verify-email/{token:minlength(32):maxlength(32)}")]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        if (string.IsNullOrEmpty(token))
            return BadRequest("Token is required.");
        
        if (await userVerificationRepository.GetVerificationByTokenAsync(token) is not { } verification)
            return BadRequest("Invalid token.");

        if (verification.Status == VerificationStatus.Verified)
            return BadRequest("Verification already completed.");

        if (verification.ExpiresAt <= DateTime.UtcNow)
        {
            verification.Status = VerificationStatus.Expired;
            await userVerificationRepository.UpdateVerificationAsync(verification);
            return BadRequest("Verification expired.");
        }
        
        verification.Status = VerificationStatus.Verified;
        verification.VerifiedAt = DateTime.UtcNow;
        await userVerificationRepository.UpdateVerificationAsync(verification);
        return Ok();   
    }

    [HttpPost("resend-email/{userId:guid}")]
    public async Task<IActionResult> ResendEmail(Guid userId)
    {
        var userVerification = new UserVerification()
        {
            UserId = userId,
        };
        // TODO: Send new verification email
        await userVerificationRepository.AddVerificationAsync(userVerification);
        return Ok();
    }
}