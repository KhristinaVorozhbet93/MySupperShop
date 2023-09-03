namespace OnlineShop.HttpModels.Responses
{
    public record AccountResponse
        (Guid Id, string Login, string Email, string Name, string LastName, string Image);
}