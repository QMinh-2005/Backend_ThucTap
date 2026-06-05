using Backend_ThucTap.DTO.Request.Customer;
using Backend_ThucTap.DTO.Response.Admin;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Backend_ThucTap.Service;

namespace Backend_ThucTap.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<(List<OrderSummaryResponse> Orders, int TotalCount)> GetAllOrderSummariesAsync(int page, int pageSize);
        Task<Order> CreateOrderAsync(int userId, CreateOrderRequest request, List<AppliedVoucherDetail> voucherDetails);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> UpdateStatusOrderAsync(int orderId, int newStatusId);
        Task<Order> GetOrderByIdAndUserIdAsync(int orderId, int userId);
        Task<(List<OrderSummaryResponse> Orders, int TotalCount)> GetOrderSummariesByStatusIdAsync(int statusId, int page, int pageSize);
        Task<(List<OrderSummaryResponse> Orders, int TotalCount)> SearchOrderSummaryAdminAsync(decimal? minPrice, decimal? maxPrice, DateTime? orderDate, int? statusId, int page, int pageSize);
        Task<int> CountSuccessfulUsesAsync(int userId, int voucherId);
    }
}
