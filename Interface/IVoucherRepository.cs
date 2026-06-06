
using Backend_ThucTap.DTO.Response.Admin;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IVoucherRepository : IRepository<Voucher>
    {
        Task<Voucher?> GetVoucherByIdAsync(int voucherId);
        Task<List<Voucher>> GetVouchersForDropdownAsync(int userId);
        Task<List<Voucher>> GetAllAvailableVouchersAsync();
        Task<(List<VoucherAdminResponse> Vouchers, int TotalCount)> GetVouchersForAdminAsync(
            string? keyword,
            bool? isActive,
            bool? isGlobal,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize);
        Task<bool> SetVoucherActiveAsync(int voucherId, bool isActive);
    }
}
