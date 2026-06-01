using System.Threading.Tasks;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(List<Product> products, int TotalCount)> GetAll();
        Task<bool> IsExistProduct(string productName);
        Task<(List<Product> products, int TotalCount)> SearchAsync(string? categorySlug, string? brandSlug, string? keyword, decimal? minPrice, decimal? maxPrice, bool? Voucher, bool? isBestSeller, string? sortBy, int page, int pageSize);
        Task<List<Product>> GetProductsForHomePageAsync(List<int> categoryIds);
        Task<(List<Product> products, int TotalCount)> GetProductsByCategorySlugAsync(string categorySlug, int page, int pageSize);

        Task<Product?> GetProductDetailBySlugAsync(string slug);

        Task<(List<ProductDetail> productDetails, int TotalCount)> GetProductDetailsByIdAsync(int productId, int page, int pageSize);
        Task<Product?> GetProductForDeletionAsync(int productId);
        Task<(List<Product> products, int TotalCount)> GetProductsForAdminAsync(string? keyword, int? categoryId, int? brandId, int page, int pageSize);
    }
}
