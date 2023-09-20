using MediatR;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Domain.Events.Handlers
{
    public class UserRegistrationNotificationByEmailHandler :
        INotificationHandler<AccountRegistredEvent>
    {
        private IEmailSender _emailSender;
        public UserRegistrationNotificationByEmailHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }
        public Task Handle(AccountRegistredEvent notification, CancellationToken cancellationToken)
        {
            return _emailSender.SendEmailAsync
                (notification.Account.Email, "Подтверждение регистрации", 
                "Вы успешно зарегистрированы!", cancellationToken);
        }
    }
}
