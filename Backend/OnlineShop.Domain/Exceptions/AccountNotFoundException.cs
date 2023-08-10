namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : DomainException
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message) : base(message)
        {
        }
    }
}