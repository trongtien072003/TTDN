using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Cart;
using proj_tt.Cart.Dto;
using proj_tt.Controllers;
using proj_tt.Web.Models.Cart;

namespace proj_tt.Web.Controllers
{
    [Route("cart")]
    [AbpMvcAuthorize]

    public class CartController : proj_ttControllerBase
    {
        private readonly ICartAppService _cartAppService;

        public CartController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cartItems = await _cartAppService.GetCartItemsByUserIdAsync();

            var model = new CartViewModel
            {
                Items = cartItems.Select(x => new CartItemViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice
                }).ToList()
            };

            return View(model); // Views/Cart/Index.cshtml
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartInput input)
        {
            await _cartAppService.AddToCartAsync(input);
            TempData["Message"] = "🛒 Đã thêm sản phẩm vào giỏ hàng.";
            return RedirectToAction("Index");
        }

        [HttpPost("update-quantity")]
        public async Task<IActionResult> UpdateQuantity(UpdateCartItemInput input)
        {
            await _cartAppService.UpdateCartItemQuantityAsync(input);
            TempData["Message"] = "✔️ Cập nhật số lượng sản phẩm.";
            return RedirectToAction("Index");
        }

        [HttpPost("remove-item")]
        public async Task<IActionResult> RemoveItem(RemoveFromCartInput input)
        {
            await _cartAppService.RemoveCartItemAsync(input);
            TempData["Message"] = "🗑️ Đã xóa sản phẩm khỏi giỏ hàng.";
            return RedirectToAction("Index");
        }

        [HttpPost("clear")]
        public async Task<IActionResult> Clear()
        {
            await _cartAppService.ClearCartAsync();
            TempData["Message"] = "🧹 Đã xóa toàn bộ giỏ hàng.";
            return RedirectToAction("Index");
        }
    }
}
