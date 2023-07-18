using MyShopBackend.Interfaces;
using MyShopBackend.Models;

namespace MyShopBackend.Data
{
    public class AccountRepozitory : EfRepozitory<Account>, IAccountRepozitory
    {
        private readonly AppDbContext _dbContext;
        public AccountRepozitory(AppDbContext _dbContext) : base(_dbContext) { }
    }
}
