namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class InvalidCodeException : DomainException
    {
        public InvalidCodeException()
        {
        }

        public InvalidCodeException(string message) : base(message)
        {
        }
    }
}
