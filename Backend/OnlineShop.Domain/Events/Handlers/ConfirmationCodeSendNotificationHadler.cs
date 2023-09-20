using MediatR;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Events.Handlers
{
    public class ConfirmationCodeSendNotificationHadler :
        INotificationHandler<UserLoginEvent>
    {
        private IEmailSender _emailSender;
        public ConfirmationCodeSendNotificationHadler(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public Task Handle(UserLoginEvent notification, CancellationToken cancellationToken)
        {
            return _emailSender.SendEmailAsync(notification.Email, notification.Subject,
                notification.Message, cancellationToken);
        }
    }
}
