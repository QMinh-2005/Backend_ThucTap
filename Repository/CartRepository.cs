using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Backend_ThucTap.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Backend_ThucTap.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(WebBadmintonContext context) : base(context)
        {
        }
        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _dbset
                .Include(c => c.CartItems)
                    .ThenInclude(cd => cd.Detail)
                        .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}