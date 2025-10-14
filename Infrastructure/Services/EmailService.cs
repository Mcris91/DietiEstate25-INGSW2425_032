using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;
using DietiEstate.Core.ValueObjects;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace DietiEstate.Infrastracture.Services;

public class EmailService(
    IConfiguration configuration,
    ILogger<EmailService> logger) : IEmailService
{
    public Task<EmailData> PrepareEmailAsync(EmailType type, string toName, string toEmail)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmailAsync(EmailData emailData)
    {
        var smtpOptions = configuration.GetSection("Smtp").Get<SmtpOptions>();
        if (smtpOptions == null)
        {
            logger.LogError("SMTP options are not configured properly.");
            throw new InvalidOperationException("SMTP options are not configured.");
        }
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtpOptions.FromName, smtpOptions.FromEmail));
        message.To.Add(new MailboxAddress(emailData.ToName, emailData.ToEmail));
        message.Subject = emailData.Subject;
        message.Body = new TextPart(TextFormat.Plain)
        {
            Text = emailData.Body
        };
        
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(smtpOptions.Server, 
            int.Parse(smtpOptions.Port), 
            MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(smtpOptions.Username,
            smtpOptions.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
        logger.LogInformation("Email sent successfully to {Recipient}", emailData.ToName);
    }
}