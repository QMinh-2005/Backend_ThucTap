using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(WebBadmintonContext context) : base(context) { }
        public async Task<int?> GetIdByCategoryName(string categoryName)
        {
            return await _dbset
                .Where(c => c.CategoryName == categoryName)
                .Select(c => (int?)c.CategoryId) // Chỉ Select mỗi cột CategoryId
                .FirstOrDefaultAsync();
        }
    }
}
