using System.Text.RegularExpressions;
using Mapster;


namespace Backend_ThucTap.Service
{
    public interface IProductService
    {
        string GenerateSlug(string categorySlug, string title);
    }
    public class ProductService : IProductService
    {
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

    }
}