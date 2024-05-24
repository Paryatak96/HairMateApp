using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Implement your email sending logic here
        // For now, just log the email to the console
        System.IO.File.AppendAllText("emails.txt", $"To: {email}\nSubject: {subject}\nMessage: {htmlMessage}\n\n");
        return Task.CompletedTask;
    }
}