using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Interfaces.Services;

public interface IEmailService
{
    Task<EmailData> PrepareWelcomeEmailAsync(User toUser);
    
    Task<EmailData> PrepareAgencyWelcomeEmailAsync(string agencyName, string toEmail, string randomPassword);
    
    Task<EmailData> PreparePasswordResetEmailAsync(User toUser, int token);
    
    Task SendEmailAsync(EmailData emailData);
}