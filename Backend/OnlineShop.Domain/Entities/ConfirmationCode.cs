namespace OnlineShop.Domain.Entities
{
    public class ConfirmationCode : IEntity
    {
        private Guid _id;
        private Guid _accountId;
        private string _code;
        private DateTimeOffset _createAt;
        private DateTimeOffset _expiredAt;

        protected ConfirmationCode() { }
        public ConfirmationCode(Guid id, Guid accountId,
            DateTimeOffset createdAt, TimeSpan codeLifeTime)
        {
            Id = id;
            AccountId = accountId;
            Code = GenerateCode();
            CreateAt = createdAt;
            ExpiredAt = CreateAt.Add(codeLifeTime);
        }

        public Guid Id
        {
            get => _id;
            init => _id = value;
        }
        public Guid AccountId
        {
            get => _accountId;
            init => _accountId = value;
        }
        public string Code
        {
            get => _code;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Value can not be null or whitespace{nameof(value)}");
                }
                _code = value ?? throw new ArgumentException(nameof(value));
            }
        }
        public DateTimeOffset CreateAt
        {
            get => _createAt;
            init => _createAt = value;
        }
        public DateTimeOffset ExpiredAt
        {
            get => _expiredAt;
            init => _expiredAt = value;
        }

        private string GenerateCode()
        {
            return Random.Shared.Next(100000, 999999).ToString();
            //return Guid.NewGuid().ToString().Replace("-", "")[..6];
        }
    }
}