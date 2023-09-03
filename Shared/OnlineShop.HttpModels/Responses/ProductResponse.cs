namespace OnlineShop.HttpModels.Responses
{
    public record ProductResponse
        (Guid Id, string Name, string Description, decimal Price, 
        DateTime ProducedAt, DateTime ExpiredAt, string Image);
}

