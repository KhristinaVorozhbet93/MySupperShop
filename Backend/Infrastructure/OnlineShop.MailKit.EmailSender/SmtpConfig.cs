using System.ComponentModel.DataAnnotations;

namespace OnlineShop.WebApi.Configurations
{
    public class SmtpConfig
    {
        [Required] public string Host { get; set; }
        [EmailAddress] public string UserName { get; set; }
        [Required] public string Password { get; set; }
        [Range(1, ushort.MaxValue)] public int Port { get; set; }
    }
}
