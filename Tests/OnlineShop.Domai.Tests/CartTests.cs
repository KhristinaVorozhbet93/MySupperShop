using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Tests
{
    public class CardTests
    {
        [Fact]
        public void Item_is_added_to_cart()
        {
            //Arrange
            var cart = new Cart(Guid.NewGuid());
            var product = new Product(Guid.NewGuid(), "sfs", "sfs", 123, DateTime.Now,
                DateTime.Now.AddDays(10), "sdfg");

            //Act
            cart.AddItem(product, 1d);

            //Assert
            Assert.Single(cart.Items);
        }

        [Fact]
        public void Adding_an_existing_item_to_cart_increases_its_quantity()
        {
            var cart = new Cart(Guid.NewGuid());
            var product = new Product(Guid.NewGuid(), "sfs", "sfs", 123, DateTime.Now,
                DateTime.Now.AddDays(10), "sdfg");

            //Act
            cart.AddItem(product, 1d);
            cart.AddItem(product, 1d);

            //Assert
            Assert.Single(cart.Items);
            Assert.Equal(2d, cart.Items.First().Quantity);       
        }

        [Theory]
        [InlineData (-1)]
        [InlineData (0)]

        public void Adding_an_item_to_cart_with_a_negative_quantity(double quantity)
        {
            var cart = new Cart(Guid.NewGuid());
            var product = new Product(Guid.NewGuid(), "Киви", "Киви, цена за кг", 123, DateTime.Now,
                DateTime.Now.AddDays(10), "Киви");

            //Act / Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => cart.AddItem(product, quantity)); 
        }
    }
}