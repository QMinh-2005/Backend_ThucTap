using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IProductDetailRepository : IRepository<ProductDetail>
    {
        Task<ProductDetail> getProductDetailByIdAsync(int detailId);
        Task<ProductDetail> getProductDetailWithSerialNumberAsync(int detailId);
    }
}
