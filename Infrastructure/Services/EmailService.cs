using System.Reflection;
using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.OfferModels;
using DietiEstate.Core.Entities.UserModels;
using DietiEstate.Core.Entities.Worker;
using DietiEstate.Core.Enums;
using DietiEstate.Core.ValueObjects;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace DietiEstate.Infrastructure.Services;

public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

    private async Task<string> GetTemplateText(string fileName)
    {
        var resourceName = $"DietiEstate.Infrastructure.Templates.Email.{fileName}";
        await using var stream = _assembly.GetManifestResourceStream(resourceName);
        if (stream == null) return string.Empty;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public async Task<EmailData> PrepareWelcomeEmailAsync(User toUser)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("welcome.html");
        bodyText = bodyText.Replace("{{nome}}", toUser.FirstName);
        bodyText = bodyText.Replace("{{cognome}}", toUser.LastName);
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = "Recupero password";
        emailData.ToEmail = toUser.Email;
        emailData.ToName = toUser.FirstName;
        
        return emailData;
    }
    
    public async Task<EmailData> PrepareAgencyWelcomeEmailAsync(string agencyName, string toEmail, string randomPassword)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("welcome-agency.html");
        bodyText = bodyText.Replace("{{nome_agenzia}}", agencyName);
        bodyText = bodyText.Replace("{{temp_password}}", randomPassword);
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = "Recupero password";
        emailData.ToEmail = toEmail;
        emailData.ToName = agencyName;
        
        return emailData;
    }
    
    public async Task<EmailData> PreparePasswordResetEmailAsync(User toUser, int token)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("forgot-password.html");
        bodyText = bodyText.Replace("{{nome}}", toUser.FirstName);
        bodyText = bodyText.Replace("{{cognome}}", toUser.LastName);
        bodyText = bodyText.Replace("{{codice_verifica}}", token.ToString());
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = "Recupero password";
        emailData.ToEmail = toUser.Email;
        emailData.ToName = toUser.FirstName;
        
        return emailData;
    }

    public async Task<EmailData> PrepareNewOfferEmailAsync(User toUser, string listingName)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("new-offer.html");
        bodyText = bodyText.Replace("{{nome}}", toUser.FirstName);
        bodyText = bodyText.Replace("{{cognome}}", toUser.LastName);
        bodyText = bodyText.Replace("{{nome_immobile}}", listingName);
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = $"Nuova offerta per l'immobile {listingName}";
        emailData.ToEmail = toUser.Email;
        emailData.ToName = toUser.FirstName;
        
        return emailData;
    }

    public async Task<EmailData> PrepareOfferStatusChangeAsync(User toUser, string listingName, OfferStatus status)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("new-appointment.html");
        bodyText = bodyText.Replace("{{nome}}", toUser.FirstName);
        bodyText = bodyText.Replace("{{cognome}}", toUser.LastName);
        bodyText = bodyText.Replace(
            "{{stato_offerta}}", 
            status == OfferStatus.Accepted 
                ? "accettata"
                : "rifiutata");
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = $"Nuovo appuntamento per l'immobile {listingName}";
        emailData.ToEmail = toUser.Email;
        emailData.ToName = toUser.FirstName;
        
        return emailData;
    }
    
    public async Task<EmailData> PrepareNewBookingEmailAsync(User toUser, string listingName)
    {
        var emailData = new EmailData();
        
        var templateText = await GetTemplateText("base.html");
        var bodyText = await GetTemplateText("new-appointment.html");
        bodyText = bodyText.Replace("{{nome}}", toUser.FirstName);
        bodyText = bodyText.Replace("{{cognome}}", toUser.LastName);
        bodyText = bodyText.Replace("{{nome_immobile}}", listingName);
        templateText = templateText.Replace("{{email_body}}", bodyText);
        
        emailData.Body = templateText;
        emailData.Subject = $"Nuovo appuntamento per l'immobile {listingName}";
        emailData.ToEmail = toUser.Email;
        emailData.ToName = toUser.FirstName;
        
        return emailData;
    }

    public async Task SendEmailAsync(EmailData emailData)
    {
        var smtpOptions = new SmtpOptions
        {
            Server = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? string.Empty, 
            Port = Environment.GetEnvironmentVariable("SMTP_PORT") ?? string.Empty,
            Username = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? string.Empty,
            Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? string.Empty,
            FromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? string.Empty,
            FromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? string.Empty

        };
        if (smtpOptions.Server == "")
        {
            logger.LogError("SMTP options are not configured properly.");
            throw new InvalidOperationException("SMTP options are not configured.");
        }
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtpOptions.FromName, smtpOptions.FromEmail));
        message.To.Add(new MailboxAddress(emailData.ToName, emailData.ToEmail));
        message.Subject = emailData.Subject;
        message.Body = new TextPart(TextFormat.Html)
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