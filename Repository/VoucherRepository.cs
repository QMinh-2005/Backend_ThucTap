using Backend_ThucTap.Data;
using Backend_ThucTap.Interface;
using Backend_ThucTap.Models;
using Backend_ThucTap.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Repository
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
