using ARQIDL3.Models.DTOs;

namespace ARQIDL3.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResponse<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filter);
        Task<ProductDto?> GetProductByIdAndStoreAsync(int id, string storeName);
    }

}
