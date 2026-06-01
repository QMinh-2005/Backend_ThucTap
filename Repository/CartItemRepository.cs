

using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(WebBadmintonContext context) : base(context)
        {
        }
    }
}
