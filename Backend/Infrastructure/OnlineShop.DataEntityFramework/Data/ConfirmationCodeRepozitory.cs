using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.Data.EntityFramework.Data
{
    public class ConfirmationCodeRepozitory : EfRepozitory<ConfirmationCode>, IConfirmationCodeRepozitory
    {
        public ConfirmationCodeRepozitory(AppDbContext dbContext) : base(dbContext) { }
    }
}
