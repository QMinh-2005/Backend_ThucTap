
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher?> GetVoucherByIdAsync(int voucherId);
    }
}
