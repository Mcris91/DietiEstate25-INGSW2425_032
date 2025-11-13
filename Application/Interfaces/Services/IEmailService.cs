using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;

namespace DietiEstate.Application.Interfaces.Services;

public interface IEmailService
{
    // TODO: Implement email templates and localization
    Task<EmailData> PrepareEmailAsync(EmailType type, string toName, string toEmail);
    Task SendEmailAsync(EmailData emailData);
}