using Backend_ThucTap.Data;
using Backend_ThucTap.Interface;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Backend_ThucTap.Repository;
using Microsoft.EntityFrameworkCore;

namespace Backend_ThucTap.Service
{
    public class VoucherValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public decimal TotalDiscount { get; set; }
        public List<AppliedVoucherDetail> AppliedVoucherDetails { get; set; } = new();
    }
    public class AppliedVoucherDetail
    {
        public int VoucherId { get; set; }
        public decimal DiscountValue { get; set; }
    }
    public interface IVoucherService
    {
        Task<VoucherValidationResult> ValidateAndCalculateDiscountAsync(int userId, List<int> VoucherIds, List<OrderDetail> orderItems, string paymentMethod);
    }
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IUserVoucherRepository _userVoucherRepository;
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly WebBadmintonContext _context;
        public VoucherService(
            IVoucherRepository voucherRepository,
            IUserVoucherRepository userVoucherRepository,
            IProductDetailRepository productDetailRepository,
            IOrderRepository orderRepository,
            WebBadmintonContext context)
        {
            _voucherRepository = voucherRepository;
            _userVoucherRepository = userVoucherRepository;
            _productDetailRepository = productDetailRepository;
            _orderRepository = orderRepository;
            _context = context;
        }
        private bool IsValidPaymentMethodForVoucher(Voucher voucher, string paymentMethod)
        {
            if (voucher.VoucherPaymentMethods == null || !voucher.VoucherPaymentMethods.Any())
            {
                return true;
            }
            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                return true;
            }
            var allowPaymentMethod = voucher.VoucherPaymentMethods.Select(x => x.PaymentMethod.Trim()).ToList();
            return allowPaymentMethod.Contains(paymentMethod.Trim(), StringComparer.OrdinalIgnoreCase);
        }
        public async Task<VoucherValidationResult> ValidateAndCalculateDiscountAsync(int userId, List<int> VoucherIds, List<OrderDetail> orderItems, string paymentMethod)
        {
            var result = new VoucherValidationResult { IsValid = true };
            decimal totalOrderDiscount = 0;
            decimal totalOrderSubTotal = orderItems.Sum(item => item.Quantity * item.UnitPrice);
            foreach (var vId in VoucherIds)
            {
                var voucher = await _voucherRepository.GetVoucherByIdAsync(vId);
                if (voucher == null || voucher.IsActive == false || voucher.EndDate < DateTime.UtcNow)
                {
                    return Error("Voucher không tồn tại hoặc đã hết hạn.");
                }
                if (voucher.UsageLimit.HasValue && voucher.UsedCount >= voucher.UsageLimit.Value)
                {
                    return Error($"Mã {voucher.VoucherCode} đã hết lượt sử dụng.");
                }
                if (!IsValidPaymentMethodForVoucher(voucher, paymentMethod))
                {
                    var allowedMethods = voucher.VoucherPaymentMethods.Select(pm => pm.PaymentMethod);
                    string methodsStr = string.Join(" hoặc ", allowedMethods);
                    return Error($"Mã {voucher.VoucherCode} chỉ áp dụng khi thanh toán qua: {methodsStr}.");
                }
                if (voucher.IsGlobal == true)
                {
                    // Mã Global: Đếm trực tiếp trong lịch sử đơn hàng thành công
                    int usedTimes = await _orderRepository.CountSuccessfulUsesAsync(userId, vId);
                    if (usedTimes >= voucher.MaxUsagePerUser)
                        return Error($"Bạn đã dùng hết lượt cho phép của mã toàn sàn {voucher.VoucherCode}.");
                }
                else
                {
                    // Mã Cá nhân: Bắt buộc phải có trong ví và kiểm tra CurrentUsageCount
                    var userVoucher = await _userVoucherRepository.GetUserVoucherAsync(userId, vId);
                    if (userVoucher == null)
                        return Error($"Bạn chưa sở hữu mã {voucher.VoucherCode} trong ví.");

                    if (userVoucher.CurrentUsageCount >= voucher.MaxUsagePerUser)
                        return Error($"Bạn đã dùng hết lượt cho phép của mã {voucher.VoucherCode}.");
                }
                List<OrderDetail> eligibleItems;
                if (voucher.IsGlobal == true)
                {
                    eligibleItems = orderItems;
                }
                else
                {
                    eligibleItems = new List<OrderDetail>();
                    foreach (var item in orderItems)
                    {
                        if (!item.DetailId.HasValue) continue;
                        var productDetail = await _productDetailRepository.getProductDetailByIdAsync(item.DetailId.Value);
                        if (productDetail == null) continue;
                        bool match = voucher.VoucherConditions.Any(c =>
                            (c.ProductId == null || c.ProductId == productDetail.ProductId) &&
                            (c.CategoryId == null || c.CategoryId == productDetail.Product.CategoryId) &&
                            (c.BrandId == null || c.BrandId == productDetail.Product.BrandId)
                        );
                        if (match) eligibleItems.Add(item);
                    }
                }
                if (!eligibleItems.Any())
                    return Error($"Mã {voucher.VoucherCode} không áp dụng cho các sản phẩm bạn chọn.");
                decimal eligibleSubTotal = eligibleItems.Sum(x => x.Quantity * x.UnitPrice);
                if (eligibleSubTotal < voucher.MinOrderValue)
                    return Error($"Mã {voucher.VoucherCode} yêu cầu đơn hàng từ {voucher.MinOrderValue:N0}đ.");
                decimal discount = 0;
                if (voucher.IsPercent == true)
                {
                    discount = eligibleSubTotal * (voucher.DiscountValue / 100m);
                    if (discount > voucher.MaxDiscountAmount) discount = voucher.MaxDiscountAmount.Value;
                }
                else
                {
                    discount = voucher.DiscountValue;
                }

                result.AppliedVoucherDetails.Add(new AppliedVoucherDetail { VoucherId = vId, DiscountValue = discount });
                totalOrderDiscount += discount;

            }
            result.TotalDiscount = Math.Min(totalOrderDiscount, totalOrderSubTotal);
            return result;
        }
        private VoucherValidationResult Error(string message) => new() { IsValid = false, ErrorMessage = message };
    }
}
