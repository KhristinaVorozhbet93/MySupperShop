namespace OnlineShop.Domain.Entities
{
    public class Account : IEntity
    {
        private Guid _id;
        private string _login;
        private string _hashedPassword;
        private string _email;

        protected Account() { }
        public Account(Guid id, string login, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentException($"Value can not be null or whitespace{nameof(login)}");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException($"Value can not be null or whitespace{nameof(password)}");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException($"Value can not be null or whitespace{nameof(email)}");
            }

            _id = id;
            _login = login ?? throw new ArgumentException(nameof(login));
            _hashedPassword = password ?? throw new ArgumentException(nameof(password));
            _email = email ?? throw new ArgumentException(nameof(email));
        }

        public Guid Id
        {
            get => _id;
            init => _id = value;
        }
        public string Login
        {
            get => _login;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Value can not be null or whitespace{nameof(value)}");
                }
                _login = value ?? throw new ArgumentException(nameof(value));
            }
        }
        public string HashedPassword
        {
            get => _hashedPassword;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Value can not be null or whitespace{nameof(value)}");
                }
                _hashedPassword = value ?? throw new ArgumentException(nameof(value));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Value can not be null or whitespace{nameof(value)}");
                }
                _email = value ?? throw new ArgumentException(nameof(value));
            }
        }
    }
}
