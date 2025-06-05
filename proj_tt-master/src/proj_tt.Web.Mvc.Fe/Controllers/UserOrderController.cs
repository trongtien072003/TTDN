using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Cart;
using proj_tt.Controllers;
using proj_tt.Order;
using proj_tt.Order.Dto;
using proj_tt.Web.Models.Orders;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    [Route("orders")]
    public class UserOrderController : proj_ttControllerBase
    {
        private readonly IUserOrderAppService _userOrderAppService;
        private readonly ICartAppService _cartAppService;

        public UserOrderController(IUserOrderAppService userOrderAppService, ICartAppService cartAppService)
        {
            _userOrderAppService = userOrderAppService;
            _cartAppService = cartAppService;
        }

        [HttpGet("MyOrder")]
        public async Task<IActionResult> Index(string keyword, string statusStr, DateTime? minDate, DateTime? maxDate)
        {
            OrderStatus? status = null;
            if (!string.IsNullOrWhiteSpace(statusStr) && Enum.TryParse(statusStr, out OrderStatus parsed))
                status = parsed;

            var input = new PagedOrderRequestDto
            {
                Keyword = keyword,
                Status = status,
                MinDate = minDate,
                MaxDate = maxDate,
                SkipCount = 0,
                MaxResultCount = 100
            };

            var orders = await _userOrderAppService.GetMyOrdersPagedAsync(input);

            return View(new MyOrderListViewModel
            {
                Orders = orders.Items,
                Keyword = keyword,
                Status = status,
                MinDate = minDate,
                MaxDate = maxDate
            });
        }

        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cartItems = await _cartAppService.GetCartItemsByUserIdAsync();
            if (cartItems == null || !cartItems.Any())
            {
                TempData["CartEmpty"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                CartItems = cartItems
            };

            return View(model);
        }

        [HttpPost("checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrderFromCart(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CartItems = await _cartAppService.GetCartItemsByUserIdAsync();
                return View("Checkout", model);
            }

            try
            {
                var input = new CreateOrderInput
                {
                    UserName = model.Name,
                    UserEmail = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Note = model.Note
                };

                await _userOrderAppService.CreateOrderAsync(input);

                TempData["SuccessMessage"] = "Đặt hàng thành công!";
                return RedirectToAction("Index", new { success = true });
            }
            catch (UserFriendlyException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống. Vui lòng thử lại sau.");
            }

            model.CartItems = await _cartAppService.GetCartItemsByUserIdAsync();
            return View("Checkout", model);
        }

        [HttpGet("view-modal/{id}")]
        public async Task<IActionResult> ViewModal(long id)
        {
            var order = await _userOrderAppService.GetMyOrderDetailAsync(id);
            return PartialView("_OrderDetailModalPartial", order); 
        }

        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> Cancel(long id)
        {
            await _userOrderAppService.CancelMyOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}
