using MediatR;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Events
{
    public class AccountRegistredEvent: INotification
    {
        public Account Account { get; }
        public AccountRegistredEvent(Account account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
        }
    }
}
