using Microsoft.AspNetCore.Mvc;
using proj_tt.Cart;
using proj_tt.Controllers;
using proj_tt.Order;
using proj_tt.Order.Dto;
using proj_tt.Web.Models.Orders;
using System;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    [Route("/orders")]
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

            var result = await _userOrderAppService.GetMyOrdersPagedAsync(input);

            return View(new MyOrderListViewModel
            {
                Orders = result.Items,
                Keyword = keyword,
                Status = status,
                MinDate = minDate,
                MaxDate = maxDate
            });
        }
        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartAppService.GetCartItemsByUserIdAsync();

            if (cart == null || cart.Count == 0)
            {
                TempData["CartEmpty"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }

            //ViewBag.TotalAmount = cart.TotalPrice;

            return View(new CheckoutViewModel());
        }

        [HttpPost("checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrderFromCart(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Checkout", model);

            var input = new CreateOrderInput
            {
                UserName = model.Name,
                UserEmail = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Note = model.Note
            };

            await _userOrderAppService.CreateOrderAsync(input);

            TempData["SuccessMessage"] = "Đơn hàng của bạn đã được đặt thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(long id)
        {
            var order = await _userOrderAppService.GetMyOrderDetailAsync(id);
            var model = new MyOrderDetailViewModel
            {
                Order = order
            };
            return View(model);
        }

        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> Cancel(long id)
        {
            await _userOrderAppService.CancelMyOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}
