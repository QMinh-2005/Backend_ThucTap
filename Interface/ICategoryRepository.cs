

using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<int?> GetIdByCategoryName(string categoryName);
    }
}
