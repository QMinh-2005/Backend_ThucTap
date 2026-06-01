using System.Text.RegularExpressions;
using Backend_ThucTap.DTO.Response.Admin;
using Backend_ThucTap.DTO.Response.Customer;
using Backend_ThucTap.Interfaces;
using Backend_ThucTap.Models;
using Mapster;

namespace Backend_ThucTap.Service
{
    public interface IProductService
    {
        Task<List<ProductHomeResponse>> GetProductsForHomePageAsync();
        Task<(List<Product> products, int TotalCount)> SearchAsync(string? categorySlug, string? brandSlug, string? key, decimal? minPrice, decimal? maxPrice, bool? Voucher, bool? isBestSeller, string? sortBy, int page, int pageSize);
        string GenerateSlug(string categorySlug, string title);
        Task<(List<ProductResponse> products, int TotalCount)> GetProductByCategorySlugAsync(string categorySlug, int page, int pageSize);
        Task<ProductDetailResponse?> GetProductDetailAsync(string slug);

    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        public string NormalizeProductName(string categoryName, string inputProductName)
        {
            if (string.IsNullOrWhiteSpace(inputProductName)) return string.Empty;
            if (string.IsNullOrWhiteSpace(categoryName)) return CapitalizeFirstLetter(inputProductName.Trim());

            string feName = inputProductName.Trim();
            string catName = categoryName.Trim();

            // Kịch bản 1: FE đã nhập chuẩn hoặc gần chuẩn toàn bộ (VD: "Vợt cầu lông Yonex", "vợt cầu lông yonex")
            // StringComparison.OrdinalIgnoreCase tự động bỏ qua khác biệt HOA/thường
            if (feName.StartsWith(catName, StringComparison.OrdinalIgnoreCase))
            {
                return CapitalizeFirstLetter(feName);
            }

            // Lấy từ đầu tiên của tên Danh mục (VD: chữ "Vợt" trong "Vợt cầu lông")
            string firstWordOfCat = catName.Split(' ')[0];

            // Kịch bản 2: FE nhập bị lặp từ đầu tiên nhưng sai kiểu (VD: "vợt Yonex Astrox", "VỢT lining")
            if (feName.StartsWith(firstWordOfCat, StringComparison.OrdinalIgnoreCase))
            {
                // Cắt bỏ phần bị lặp đi, chỉ lấy phần đuôi (Substring dựa trên độ dài của từ đầu tiên)
                string remainingName = feName.Substring(firstWordOfCat.Length).Trim();

                // Ghép tên Danh mục chuẩn trong DB với phần đuôi
                return CapitalizeFirstLetter($"{catName} {remainingName}");
            }

            // Kịch bản 3: FE chỉ nhập đúng tên model (VD: "Astrox 100zz" hoặc "Halbertec 8000")
            return CapitalizeFirstLetter($"{catName} {feName}");
        }

        // Hàm phụ trợ: Giúp viết hoa chữ cái đầu tiên của sản phẩm cho đẹp
        private string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;
            if (text.Length == 1) return text.ToUpper();
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        private string RemoveVietnameseAccents(string text)
        {
            string[] vietnameseSigns = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ", "ÍÌỊỈĨ",
                "đ", "Đ",
                "ýỳỵỷỹ", "ÝỲỴỶỸ"
            };
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                    text = text.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
            }
            return text;
        }

        // SỬA: Logic sinh Slug ghép nối CategorySlug và ProductName
        public string GenerateSlug(string categorySlug, string title)
        {
            if (string.IsNullOrEmpty(title)) return "";

            // Xóa dấu tiếng việt và chuyển thành chữ thường
            string formattedTitle = RemoveVietnameseAccents(title).ToLower();

            // Xóa ký tự đặc biệt, chỉ giữ lại chữ, số và khoảng trắng
            formattedTitle = Regex.Replace(formattedTitle, @"[^a-z0-9\s-]", "");

            // Thay khoảng trắng thành dấu gạch ngang và xóa gạch ngang dư thừa
            formattedTitle = Regex.Replace(formattedTitle, @"\s+", "-").Trim('-');

            // Ghép CategorySlug vào phía trước (nếu có)
            if (!string.IsNullOrEmpty(categorySlug))
            {
                return $"{categorySlug}-{formattedTitle}";
            }

            return formattedTitle;
        }
        public async Task<List<ProductHomeResponse>> GetProductsForHomePageAsync()
        {
            List<int> categories = new List<int> { 1, 2, 7 };
            var products = await _productRepository.GetProductsForHomePageAsync(categories);
            var response = products.Select(p => new ProductHomeResponse
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Slug = p.Slug,
                MainImageUrl = p.MainImageUrl,
                CategoryName = p.Category.CategoryName,
                BasePrice = p.BasePrice,
                SellingPrice = (decimal)(p.DiscountPrice.HasValue ? p.DiscountPrice : p.BasePrice),
                DiscountPercent = p.DiscountPrice.HasValue && p.BasePrice > 0
                ? (int)Math.Round((p.BasePrice - p.DiscountPrice.Value) / p.BasePrice * 100)
                : 0,
                IsBestSeller = p.SoldQuantity >= 10
            }).ToList();
            return response;
        }
        public async Task<(List<Product> products, int TotalCount)> SearchAsync(string? categorySlug, string? brandSlug, string? keyword, decimal? minPrice, decimal? maxPrice, bool? Voucher, bool? isBestSeller, string? sortBy, int page, int pageSize)
        {
            return await _productRepository.SearchAsync(categorySlug, brandSlug, keyword, minPrice, maxPrice, Voucher, isBestSeller, sortBy, page, pageSize);
        }


        public async Task<(List<ProductResponse> products, int TotalCount)> GetProductByCategorySlugAsync(string categorySlug, int page, int pageSize)
        {
            var (products, totalCount) = await _productRepository.GetProductsByCategorySlugAsync(categorySlug, page, pageSize);
            var response = products.Select(p => new ProductResponse
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Slug = p.Slug,
                MainImageUrl = p.MainImageUrl,
                BasePrice = p.BasePrice,
                SellingPrice = (decimal)(p.DiscountPrice.HasValue ? p.DiscountPrice : p.BasePrice),
                DiscountPercent = p.DiscountPrice.HasValue && p.BasePrice > 0
                    ? (int)Math.Round((p.BasePrice - p.DiscountPrice.Value) / p.BasePrice * 100)
                    : 0,
                IsBestSeller = p.SoldQuantity >= 10
            }).ToList();
            return (response, totalCount);
        }
        public async Task<ProductDetailResponse?> GetProductDetailAsync(string slug)
        {
            var product = await _productRepository.GetProductDetailBySlugAsync(slug);
            if (product == null) return null;

            var variants = product.ProductDetails?
                .Select(d => new ProductVariant
                {
                    DetailId = d.DetailId,
                    WeightClass = d.WeightClass,
                    GripSize = d.GripSize,
                    BalancePoint = d.BalancePoint,
                    Stiffness = d.Stiffness,
                    MaxTension = d.MaxTension,
                    Price = d.Price,
                    StockQuantity = d.StockQuantity ?? 0,

                    // Trả về true nếu số lượng > 0
                    InStock = (d.StockQuantity ?? 0) > 0
                }).ToList() ?? new List<ProductVariant>();

            return new ProductDetailResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                BasePrice = product.BasePrice,
                SellingPrice = product.DiscountPrice ?? product.BasePrice,
                DiscountPercent = product.DiscountPrice.HasValue && product.BasePrice > 0
                    ? (int)Math.Round((product.BasePrice - product.DiscountPrice.Value) / product.BasePrice * 100)
                    : 0,
                MainImageUrl = product.MainImageUrl,
                Description = product.Description,

                // Sản phẩm được coi là "Còn hàng" nếu CÓ ÍT NHẤT 1 phân loại (Variant) có Stock > 0
                IsAvailable = variants.Any(v => v.InStock),

                // Map danh sách ảnh
                Imgaes = product.ProductImages?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImage
                    {
                        ImageUrl = i.ImageUrl,
                        DisplayOrder = i.DisplayOrder
                    }).ToList() ?? new List<ProductImage>(),

                Variants = variants
            };
        }
    }
}