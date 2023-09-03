namespace OnlineShop.Domain.Entities
{
    public class Product : IEntity
    {
        private Guid _id;
        private string _name;
        private string _description;
        private decimal _price;
        private DateTime _producedAt;
        private DateTime _expiredAt;
        public string _image;

        protected Product() { }
        public Product(Guid id, string name, string description, decimal price,
            DateTime producedAt, DateTime expiredAt, string image)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"Value can not be null or whitespace{nameof(name)}");
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException
                    ($"Value can not be null or whitespace{nameof(description)}");
            }
            if (string.IsNullOrWhiteSpace(image))
            {
                throw new ArgumentException
                    ($"Value can not be null or whitespace{nameof(image)}");
            }
            if (price <= 0)
            {
                throw new ArgumentException
                    ($"Value cannot be less than or equal to zero{nameof(price)}");
            }
            _id = id;
            _name = name ?? throw new ArgumentException(nameof(name));
            _description = description ?? throw new ArgumentException(nameof(description));
            _price = price;
            _producedAt = producedAt;
            _expiredAt = expiredAt;
            _image = image ?? throw new ArgumentException(nameof(image));

        }
        public Guid Id
        {
            get => _id;
            init => _id = value;
        }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException
                        ($"Value can not be null or whitespace{nameof(value)}");
                }
                _name = value ?? throw new ArgumentException(nameof(value));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException
                        ($"Value can not be null or whitespace{nameof(value)}");
                }
                _description = value ?? throw new ArgumentException(nameof(value));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException
                        ($"Value cannot be less than or equal to zero{nameof(value)}");
                }
                _price = value;
            }
        }

        public DateTime ProducedAt
        {
            get => _producedAt;
            set => _producedAt = value;
        }

        public DateTime ExpiredAt
        {
            get => _expiredAt;
            set => _expiredAt = value;
        }

        public string Image
        {
            get => _image;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException
                        ($"Value can not be null or whitespace{nameof(value)}");
                }
                _image = value ?? throw new ArgumentException(nameof(value));
            }
        }

    }
}
