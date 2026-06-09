using Microsoft.EntityFrameworkCore;
using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(WebBadmintonContext context) : base(context) { }
        public async Task<List<ProductImage>> GetByProductIdAsync(int productId)
        {
            return await _dbset
                .Where(img => img.ProductId == productId)
                .OrderBy(img => img.DisplayOrder)
                .ToListAsync();
        }

        public async Task<ProductImage?> GetMainImageByProductIdAsync(int productId)
        {
            return await _dbset.FirstOrDefaultAsync(img => img.ProductId == productId && img.IsMain == true);
        }
        public async Task<bool> IsExistDisplayOrder(int displayOrder)
        {
            return await _dbset.AnyAsync(img => img.DisplayOrder == displayOrder);
        }
    }
}
