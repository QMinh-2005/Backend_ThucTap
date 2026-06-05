using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interface
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher?> GetVoucherByIdAsync(int voucherId);
    }
}
