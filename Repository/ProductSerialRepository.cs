using Microsoft.EntityFrameworkCore;
using Backend_ThucTap.Data;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;

namespace Backend_ThucTap.Repositories
{
    public class ProductSerialRepository : Repository<ProductSerial>, IProductSerialRepository
    {
        public ProductSerialRepository(WebBadmintonContext context) : base(context)
        {
        }
        public async Task<bool> IsSerialNumberExistsAsync(string serialNumber)
        {
            return await _dbset.AnyAsync(ps => ps.SerialNumber == serialNumber.Trim());
        }
    }
}