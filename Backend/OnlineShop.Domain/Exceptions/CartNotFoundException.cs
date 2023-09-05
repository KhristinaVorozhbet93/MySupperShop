namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class CartNotFoundException : DomainException
    {
        public CartNotFoundException()
        {
        }

        public CartNotFoundException(string message) : base(message)
        {
        }
    }
}
