using Backend_ThucTap.DTO.Response.Customer;
using Backend_ThucTap.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ThucTap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("searchAsync")]
        public async Task<IActionResult> SearchAll(
            [FromQuery] string? categorySlug,
            [FromQuery] string? brandSlug,
            [FromQuery] string? keyword,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? Voucher,
            [FromQuery] bool? isBestSeller,
            [FromQuery] string? sortBy,
            int page = 1,
            int pagesize = 10
            )
        {

            var (products, totalCount) = await _productService.SearchAsync(categorySlug, brandSlug, keyword, minPrice, maxPrice, Voucher, isBestSeller, sortBy, page, pagesize);

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

            return Ok(new
            {
                items = response,
                totalCount = totalCount,
                page,
                pagesize,
                totalPages = (int)Math.Ceiling((double)totalCount / pagesize)
            });
        }
        [HttpGet("home")]
        public async Task<IActionResult> GetHomeProducts()
        {
            var result = await _productService.GetProductsForHomePageAsync();
            return Ok(new
            {
                Message = "Thành công",
                Data = result
            });
        }
        [HttpGet("product_of_category/{categorySlug}")]
        public async Task<IActionResult> GetProductsByCategorySlug(
            string categorySlug,
            int page = 1,
            int pagesize = 10)
        {
            var (products, totalCount) = await _productService.GetProductByCategorySlugAsync(categorySlug, page, pagesize);
            return Ok(new
            {
                items = products,
                totalCount = totalCount,
                page,
                pagesize,
                totalPages = (int)Math.Ceiling((double)totalCount / pagesize)
            });
        }
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProductDetail(string slug)
        {
            try
            {
                var result = await _productService.GetProductDetailAsync(slug);

                if (result == null)
                    return NotFound(new { Message = "Không tìm thấy sản phẩm" });

                return Ok(new
                {
                    Message = "Thành công",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}
