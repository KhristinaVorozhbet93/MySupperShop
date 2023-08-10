using System.Runtime.Serialization;

namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message) : base(message)
        {
        }
    }
}