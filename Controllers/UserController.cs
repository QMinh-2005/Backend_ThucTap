using System.Security.Claims;
using Backend_ThucTap.DTO.Request;
using Backend_ThucTap.DTO.Request.Customer;
using Backend_ThucTap.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_ThucTap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize] // Bắt buộc phải có Token hợp lệ
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                // 1. Lấy User ID từ Claim trong Token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Không xác định được người dùng." });

                int userId = int.Parse(userIdClaim);

                // 2. Kiểm tra mật khẩu mới
                if (request.NewPassword.Length < 6)
                    return BadRequest(new { message = "Mật khẩu mới quá ngắn." });

                // 3. Gọi Service xử lý
                var result = await _userService.ChangePasswordAsync(userId, request.OldPassword, request.NewPassword);

                return Ok(new { message = "Đổi mật khẩu thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize] // Bắt buộc phải đăng nhập
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ChangeInfoRequest request)
        {
            try
            {
                // 1. Lấy User ID từ Token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "Không xác định được người dùng." });

                int userId = int.Parse(userIdClaim);

                var result = await _userService.UpdateProfileAsync(userId, request);

                if (result)
                {
                    return Ok(new { message = "Cập nhật thông tin thành công." });
                }

                return NotFound(new { message = "Không tìm thấy người dùng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("user-info")]
        public async Task<IActionResult> GetInfo()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(userIdClaim))
                    return Unauthorized(new { message = "Không xác định được người dùng." });
                int userId = int.Parse(userIdClaim);
                var res = await _userService.GetInfoAsync(userId);
                if (res == null) return NotFound(new { message = $"Không tìm thấy {userId}" });
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
