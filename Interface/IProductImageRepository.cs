using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<List<ProductImage>> GetByProductIdAsync(int productId);
        Task<ProductImage?> GetMainImageByProductIdAsync(int productId);
        Task<bool> IsExistDisplayOrder(int displayOrder);
    }
}

