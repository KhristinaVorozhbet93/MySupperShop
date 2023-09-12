namespace OnlineShop.HttpModels.Responses
{
    public record ProductCartResponse
     (Guid Id, string Name, decimal Price,string Image, double Quantity);
}
