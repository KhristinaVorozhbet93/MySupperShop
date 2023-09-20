using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineShop.Domain.Interfaces;
using OnlineShop.WebApi.Configurations;
using System.Net;

namespace OnlineShop.MailKit.EmailSender
{
    public class MailKitSmptEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();
        private readonly SmtpConfig _smtpConfig;

        public async ValueTask DisposeAsync()
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }

        public MailKitSmptEmailSender(IOptions<SmtpConfig> options)
        {
            _smtpConfig = options.Value;
        }
        private async Task EnsureConnectAndAuthenticateAsync(CancellationToken cancellationToken)
        {
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false, cancellationToken);
            }
            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync
                    (_smtpConfig.UserName, _smtpConfig.Password, cancellationToken);
            }
        }
        public async Task SendEmailAsync(string recepientEmail, string subject, 
            string message, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(recepientEmail);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(message);

            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Магазин продуктов", _smtpConfig.UserName));
            emailMessage.To.Add(new MailboxAddress("", recepientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            await EnsureConnectAndAuthenticateAsync(cancellationToken);
            await _smtpClient.SendAsync(emailMessage, cancellationToken);
        }
    }
}
