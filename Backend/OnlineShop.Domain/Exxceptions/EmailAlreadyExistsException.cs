using System.Runtime.Serialization;

namespace OnlineShop.Domain.Exxceptions
{
    [Serializable]
    public class EmailAlreadyExistsException : DomainException
    {
        public EmailAlreadyExistsException()
        {
        }

        public EmailAlreadyExistsException(string? message) : base(message)
        {
        }

        public EmailAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}