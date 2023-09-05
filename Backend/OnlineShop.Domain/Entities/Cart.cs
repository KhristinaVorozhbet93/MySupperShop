namespace OnlineShop.Domain.Entities
{
    public class Cart : IEntity
    {
		private Guid _id;
        private Guid _accountId;
        private List<CartItem> _itmes;

        protected Cart() { }
        public Cart(Guid accountId)
        {
            _accountId = accountId; 
        }

        public Guid Id
        {
            get => _id;
            init => _id = value;
        }

        public Guid AccountId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

    }
}
