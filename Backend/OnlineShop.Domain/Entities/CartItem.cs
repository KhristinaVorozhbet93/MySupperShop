namespace OnlineShop.Domain.Entities
{
    public class CartItem : IEntity
    {
        private Guid _id;
        private Product _product;
        private double _quantity;
        public Cart Cart { get; set; } = null;

        protected CartItem() { }
        public CartItem(Guid id, Product product, double quantity )
        {           
            _id = id;
            _quantity = quantity;
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }
  
        public Guid Id
        {
            get => _id;
            init => _id = value;
        }
        public Product Product
        {
            get => _product; 
            init => _product = value; 
        }
        public double Quantity
        {
            get => _quantity; 
            set => _quantity = value; 
        }
    }
}
