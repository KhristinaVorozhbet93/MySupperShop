namespace MyShopBackend.Models
{
    public class Account 
    {
        public Guid Id { get; init; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public Account(string login, string password, string email)
        {
            Login = login ?? throw new ArgumentException(nameof(login));
            Password = password ?? throw new ArgumentException(nameof(password));
            Email = email ?? throw new ArgumentException(nameof(email)); 
        }
    }
}
