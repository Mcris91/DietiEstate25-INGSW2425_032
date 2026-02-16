using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Interfaces.Services;

public interface IEmailService
{
    Task<EmailData> PrepareWelcomeEmailAsync(User toUser);
    
    Task<EmailData> PrepareAgencyWelcomeEmailAsync(string agencyName, string toEmail, string randomPassword);
    
    Task<EmailData> PreparePasswordResetEmailAsync(User toUser, int token);
    
    Task<EmailData> PrepareNewOfferEmailAsync(User toUser, string listingName);

    Task<EmailData> PrepareOfferStatusChangeAsync(User toUser, string listingName, OfferStatus status);
    
    Task<EmailData> PrepareNewBookingEmailAsync(User toUser, string listingName);
    
    Task SendEmailAsync(EmailData emailData);
}