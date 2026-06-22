using System.ComponentModel.DataAnnotations;

namespace Backend_ThucTap.DTO.Request.Customer
{
    public class UpdateReviewRequest
    {
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao.")]
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public List<string>? ImageUrls { get; set; }
    }
}
