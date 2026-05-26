using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Microsoft.EntityFrameworkCore;


namespace Backend_ThucTap.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(WebBadmintonContext context) : base(context) { }
        public async Task<int?> GetIdByBrandName(string brandName)
        {
            return await _dbset
                .Where(b => b.BrandName == brandName)
                .Select(b => (int?)b.BrandId) // Chỉ Select mỗi cột CategoryId
                .FirstOrDefaultAsync();
        }
    }
}
