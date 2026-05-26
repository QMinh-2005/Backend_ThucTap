using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<int?> GetIdByBrandName(string brandName);
    }
}
