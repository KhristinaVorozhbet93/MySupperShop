namespace OnlineShop.Domain.Entities
{
    public class Cart : IEntity
    {
		private Guid _id;
        private Guid _accountId;
        public List<CartItem> Items { get; set; }

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

        public void AddItem(Product product, double quantity)
        {
            ArgumentNullException.ThrowIfNull(product);
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity)); 
            if (Items == null) throw new InvalidOperationException("Cart items is null");

            var existedItem = Items.SingleOrDefault(item => item.Product.Id == product.Id);

            if (existedItem is null)
            {
                Items.Add(new CartItem(Guid.Empty, product, quantity));
            }
            else
            {
                existedItem.Quantity += quantity;
            }
        }

    }
}
