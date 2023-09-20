using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;

namespace OnlineShop.HttpApiClient
{
    public interface IMyShopClient
    {
        public void SetAuthorizationToken(string token);
        public void ResetAuthorizationToken();
        //Product
        Task AddProduct(ProductRequest product, CancellationToken cancellationToken);
        Task<ProductResponse> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<ProductResponse>> GetProducts(CancellationToken cancellationToken);
        Task DeleteProduct(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(ProductRequest product, CancellationToken cancellationToken);

        //Account
        Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken);
        Task<LoginResponse> LoginByPassword(LoginRequest request, CancellationToken cancellationToken);
        Task<LoginByCodeResponse> LoginByCode(LoginByCodeRequest loginByCodeRequest,
        CancellationToken cancellationToken);
        Task<AccountResponse> GetAccount(CancellationToken cancellationToken);
        Task UpdateAccountData(AccountRequest request, CancellationToken cancellationToken);
        Task UpdateAccountPassword(AccountPasswordRequest request, CancellationToken cancellationToken);
        Task DeleteAccount(AccountRequest accountRequest, CancellationToken cancellationToken);

        //Cart
        Task<CartResponse> GetCart(Guid id, CancellationToken cancellationToken);
        Task AddProductInCart(CartRequest request, CancellationToken cancellationToken);
    }
}
