using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Repositories
{
    public class VoucherRepository : Repository<Voucher>, IVoucherRepository
    {
        public VoucherRepository(WebBadmintonContext context) : base(context)
        {
        }
        public async Task<Voucher?> GetVoucherByIdAsync(int voucherId)
        {
            return await _dbset
                .Include(vc => vc.VoucherConditions)
                .Include(v => v.VoucherPaymentMethods)
                .FirstOrDefaultAsync(v => v.VoucherId == voucherId);
        }
    }
}
