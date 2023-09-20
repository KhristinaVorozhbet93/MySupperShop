namespace OnlineShop.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recepientEmail, string subject,
            string message, CancellationToken cancellationToken);
    }
}
