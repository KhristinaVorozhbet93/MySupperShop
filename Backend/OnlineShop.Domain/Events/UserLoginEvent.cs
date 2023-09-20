using MediatR;

namespace OnlineShop.Domain.Events
{
    public class UserLoginEvent : INotification
    {
        public string Email { get; }
        public string Subject { get; }
        public string Message { get; }
        public UserLoginEvent(string email, string subject, string message)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Subject = subject ?? throw new ArgumentNullException(nameof(subject));
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
