using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EManagementVSA.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly SMTPParamterSettings _smtpSettings;

    public EmailService(IOptions<SMTPParamterSettings> smtpSettings) => _smtpSettings = smtpSettings.Value;

    public async Task SendEmailAsync(SendEmailParameters emailParamters)
    {
        var email = new MimeMessage()
        {
            Subject = emailParamters.Subject,
            To = { MailboxAddress.Parse(emailParamters.To) },
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailParamters.Body
            },
            From = { MailboxAddress.Parse(emailParamters.From) }
        };

        using var smtp = new SmtpClient();
        smtp.Connect(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_smtpSettings.UserName, _smtpSettings.Password);

        var response = await smtp.SendAsync(email);
        smtp.Disconnect(true);

    }
}

public record SendEmailParameters(string Message, string From, string To, string Subject, string Body);
public class SMTPParamterSettings{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public SMTPParamterSettings() { }
}
