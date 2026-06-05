using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Repositories
{
    public class UserVoucherRepository : Repository<UserVoucher>, IUserVoucherRepository
    {
        public UserVoucherRepository(WebBadmintonContext context) : base(context)
        {
        }
        public async Task<UserVoucher> GetUserVoucherAsync(int userId, int vId)
        {
            return await _dbset.FirstOrDefaultAsync(uv => uv.UserId == userId && uv.VoucherId == vId);
        }
        public async Task<bool> IsVoucherAlreadySavedAsync(int userId, int voucherId)
        {
            return await _dbset
                .AnyAsync(uv => uv.UserId == userId && uv.VoucherId == voucherId);
        }
    }
}
