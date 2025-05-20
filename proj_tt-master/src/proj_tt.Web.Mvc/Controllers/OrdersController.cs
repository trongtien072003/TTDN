using Microsoft.AspNetCore.Mvc;
using proj_tt.Order.Dto;
using proj_tt.Order;
using System.Threading.Tasks;
using proj_tt.Controllers;
using proj_tt.Web.Models.Order;
using System;
using Abp.UI;

namespace proj_tt.Web.Controllers
{
    [Route("admin/orders")]
    public class OrderController : proj_ttControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string keyword, string statusStr, DateTime? minDate, DateTime? maxDate)
        {
            OrderStatus? status = null;

            if (!string.IsNullOrWhiteSpace(statusStr) && Enum.TryParse(statusStr, out OrderStatus parsedStatus))
            {
                status = parsedStatus;
            }

            var input = new PagedOrderRequestDto
            {
                Keyword = keyword,
                Status = status,
                MinDate = minDate,
                MaxDate = maxDate,
                SkipCount = 0,
                MaxResultCount = 100
            };

            var result = await _orderAppService.GetAllAsync(input);
            return View(new AdminOrderListViewModel
            {
                Orders = result.Items,
                Keyword = keyword,
                Status = status,
                MinDate = minDate,
                MaxDate = maxDate
            });
        }


        [HttpGet("view-modal/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var order = await _orderAppService.GetOrderAsync(id);
            return PartialView("_OrderDetailModal", order);
        }

        [HttpGet("edit-modal/{id}")]
        public async Task<IActionResult> EditModal(long id)
        {
            var order = await _orderAppService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return PartialView("_EditOrderModal", order);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusInput input)
        {
            if (input == null || input.Id <= 0)
            {
                return BadRequest(new
                {
                    error = new { message = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại." }
                });
            }

            try
            {
                await _orderAppService.UpdateStatusAsync(input);
                return Ok(new { message = "Cập nhật thành công." });
            }
            catch (UserFriendlyException ex)
            {
                Logger.Warn("Lỗi người dùng khi cập nhật đơn hàng: " + ex.Message);
                return BadRequest(new { error = new { message = ex.Message } });
            }
            catch (Exception ex)
            {
                Logger.Error("Lỗi hệ thống khi cập nhật đơn hàng", ex);
                return StatusCode(500, new
                {
                    error = new { message = "Đã xảy ra lỗi. Vui lòng thử lại sau." }
                });
            }
        }


        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            var file = await _orderAppService.ExportAllToFileAsync();
            return File(file.FileBytes, file.FileType, file.FileName);
        }
    }
}
