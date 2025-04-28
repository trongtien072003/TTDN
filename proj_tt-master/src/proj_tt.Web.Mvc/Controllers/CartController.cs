using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Cart.Dto;
using proj_tt.Cart;
using proj_tt.Products;
using System.Threading.Tasks;
using proj_tt.Controllers;
using System.Linq;
using proj_tt.Web.Models.Cart;

namespace proj_tt.Web.Controllers
{
    [AbpMvcAuthorize]
    public class CartController : proj_ttControllerBase
    {
        private readonly ICartAppService _cartAppService;
        private readonly IRepository<Product, int> _productRepository;

        public CartController(
            ICartAppService cartAppService,
            IRepository<Product, int> productRepository)
        {
            _cartAppService = cartAppService;
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var cartDto = await _cartAppService.GetCartAsync();

            var model = new IndexViewCartModel
            {
                Items = cartDto.Items.Select(item => new IndexViewCartItemModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity
                }).ToList(),
                TotalPrice = cartDto.TotalPrice,
                UserId = cartDto.UserId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddToCart([FromBody] AddToCartInput input)
        {
            await _cartAppService.AddToCart(input);
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> RemoveFromCart([FromBody] RemoveFromCartInput input)
        {
            await _cartAppService.RemoveFromCart(input);
            return Json(new { success = true });
        }
    }
}
