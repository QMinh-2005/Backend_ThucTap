using Backend_ThucTap.Data;
using Backend_ThucTap.Models;
using Backend_ThucTap.Repositories;
using Microsoft.EntityFrameworkCore;
using Backend_ThucTap.Interfaces;


namespace Backend_ThucTap.Repositories
{
    public class ProductDetailRepository : Repository<ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(WebBadmintonContext context) : base(context)
        {
        }
        public async Task<ProductDetail> getProductDetailByIdAsync(int detailId)
        {
            return await _dbset
                .Include(p => p.Product)
                .Include(s => s.ProductSerials)
                .FirstOrDefaultAsync(s => s.DetailId == detailId);
        }
    }
}
