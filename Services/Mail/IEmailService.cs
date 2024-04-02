namespace EManagementVSA.Services.Mail;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailParameters emailParameters);
}