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
    public async Task<EmailData> PrepareEmailAsync(EmailType type, string toName, string toEmail)
    {
        string subject = "";
        string body = "";

        switch (type)
        {
            case EmailType.Verification:
                subject = "Benvenuto su DietiEstate!";
                body = $@"La tua azienda è stata registrata con successo!
                        La tua password è: {toName}";
                break;
            
            case EmailType.Welcome:
                subject = "Benvenuto su DietiEstate!";
                body = $@"Ciao {toName},
                      Il tuo account è stato creato con successo!";
                break;

            case EmailType.PasswordReset:
                subject = "Recupero Password";
                body = $"Ciao {toName}, ecco il tuo codice di ripristino...";
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), "Tipo email non supportato.");
        }
        return new EmailData
        {
            ToEmail = toEmail,
            ToName = toName,
            Subject = subject,
            Body = body
        };
    }

    public async Task SendEmailAsync(EmailData emailData)
    {
        var smtpOptions = new SmtpOptions
        {
            Server = Environment.GetEnvironmentVariable("SMTP_SERVER"), 
            Port = Environment.GetEnvironmentVariable("SMTP_PORT"),
            Username = Environment.GetEnvironmentVariable("SMTP_USERNAME"),
            Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD"),
            FromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL"),
            FromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME")

        };
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