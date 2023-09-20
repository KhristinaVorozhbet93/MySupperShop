namespace OnlineShop.HttpModels.Requests
{
    public class LoginByCodeRequest
    {
        public Guid CodeId { get; set; }
        public string Code { get; set; }
        public string Login { get; set; }
    }
}
