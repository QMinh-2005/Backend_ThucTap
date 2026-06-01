

using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
    }
}
