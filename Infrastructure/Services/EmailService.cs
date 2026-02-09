using DietiEstate.Application.Interfaces.Services;
using DietiEstate.Core.Entities.UserModels;
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
    private readonly string _templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Email", "base.html");

    private string GetTemplateText()
    {
        using var streamReader = new StreamReader(_templatePath);
        return streamReader.ReadToEnd();
    }

    public async Task<EmailData> PrepareWelcomeEmailAsync(User toUser)
    {
        var emailData = new EmailData();
        
        var bodyPath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Email", "welcome.html");
        using var streamReader = new StreamReader(bodyPath);
        var templateText = GetTemplateText();
        var bodyText = await streamReader.ReadToEndAsync();
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
        
        var bodyPath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Email", "welcome-agency.html");
        using var streamReader = new StreamReader(bodyPath);
        var templateText = GetTemplateText();
        var bodyText = await streamReader.ReadToEndAsync();
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
        
        var bodyPath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Email", "forgot-password.html");
        using var streamReader = new StreamReader(bodyPath);
        var templateText = GetTemplateText();
        var bodyText = await streamReader.ReadToEndAsync();
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