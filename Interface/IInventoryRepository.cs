using Backend_ThucTap.DTO.Response.Admin;

namespace Backend_ThucTap.Interfaces
{
    public interface IInventoryRepository
    {
        Task<(List<LowStockVariantResponse> Items, int TotalCount)> GetLowStockVariantsAsync(int threshold = 5);
        Task<(List<InventorySerialResponse> Items, int TotalCount)> GetSerialsByStatusAsync(string status, int page, int pageSize);
        Task<bool> MarkSerialAsDefectiveAsync(int serialId);
        Task<bool> MarkSerialAsInStockAsync(int serialId);
    }
}
