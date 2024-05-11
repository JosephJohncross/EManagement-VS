namespace EManagementVSA.Infrastructure.Mail;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailParameters emailParameters);
}