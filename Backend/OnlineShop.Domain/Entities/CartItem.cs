namespace OnlineShop.Domain.Entities
{
    public class CartItem : IEntity
    {
        private Guid _id;
        private Guid _productId;
        private double _quantity;
        public Cart Cart { get; set; } = null;
        protected CartItem() { }
        public CartItem(Guid id,Guid productId, double quantity )
        {
            _id = id;
            _productId = productId;
            _quantity = quantity; 
        }
  
        public Guid Id
        {
            get => _id;
            init => _id = value;
        }
        public Guid ProductId
        {
            get => _productId; 
            init => _productId = value; 
        }
        public double Quantity
        {
            get => _quantity; 
            set => _quantity = value; 
        }

    }
}
