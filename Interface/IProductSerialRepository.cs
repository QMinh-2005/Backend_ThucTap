using Backend_ThucTap.Models;

namespace Backend_ThucTap.Interfaces
{
    public interface IProductSerialRepository : IRepository<ProductSerial>
    {
        Task<bool> IsSerialNumberExistsAsync(string serialNumber);
    }
}
