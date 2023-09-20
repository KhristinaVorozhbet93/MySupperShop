namespace OnlineShop.HttpModels.Responses
{
    public record LoginByCodeResponse(Guid Id, string Login, string Token);
}
