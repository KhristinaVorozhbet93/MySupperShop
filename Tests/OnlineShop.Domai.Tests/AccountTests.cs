using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Events;
using OnlineShop.Domain.Events.Handlers;
using OnlineShop.Domain.Interfaces;
using System.Security.Principal;

namespace OnlineShop.Domain.Tests
{
    public class AccountTests
    {
        [Fact]
        public async void Account_registred_event_notifies_the_user_by_email()
        {
            var emailSender = new FakeEmailSender();
            var handler = new UserRegistrationNotificationByEmailHandler(emailSender);
            var account = new Account(Guid.Empty, "John", "Hello","hgg@mail.ru", new []{Role.Customer });
            var @event = new AccountRegistredEvent(account);

            await handler.Handle(@event, default);
            
        }

        class FakeEmailSender : IEmailSender
        {
            public Task SendEmailAsync(string recepientEmail, string subject, string message, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
