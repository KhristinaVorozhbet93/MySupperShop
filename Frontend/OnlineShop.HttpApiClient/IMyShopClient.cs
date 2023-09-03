using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.HttpApiClient
{
    public interface IMyShopClient
    {
        public void SetAuthorizationToken(string token);
        Task AddProduct(ProductRequest product, CancellationToken cancellationToken);
        Task<ProductResponse> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<ProductResponse>> GetProducts(CancellationToken cancellationToken);
        Task DeleteProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(ProductRequest product, CancellationToken cancellationToken);

        Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken);
        Task<AccountResponse> GetAccount(CancellationToken cancellationToken);
        Task UpdateAccountData(AccountRequest request, CancellationToken cancellationToken);
        Task UpdateAccountPassword(AccountPasswordRequest request, CancellationToken cancellationToken);
    }
}
