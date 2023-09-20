namespace OnlineShop.HttpModels.Responses
{
    public record LoginResponse(Guid Id, string Login, Guid ConfirmationCodeId);
}
