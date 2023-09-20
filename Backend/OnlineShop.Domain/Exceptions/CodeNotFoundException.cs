namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class CodeNotFoundException : DomainException
    {
        public CodeNotFoundException()
        {
        }

        public CodeNotFoundException(string message) : base(message)
        {
        }
    }
}
