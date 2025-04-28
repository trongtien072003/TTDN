using Microsoft.AspNetCore.Mvc;
using proj_tt.Order.Dto;
using proj_tt.Order;
using System.Threading.Tasks;
using proj_tt.Controllers;
using proj_tt.Web.Models.Order;

namespace proj_tt.Web.Controllers
{
    public class OrdersController : proj_ttControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public async Task<ActionResult> Index()
        {
            var orders = await _orderAppService.GetAllOrders(new PagedOrderResultRequestDto());
            var model = new IndexViewOrderModel(orders.Items);
            return View(model);
        }

        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusInput input)
        {
            await _orderAppService.UpdateOrderStatus(input);
            return Ok();
        }

        public async Task<ActionResult> EditModal(long orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new IndexViewOrderModel
            {
                Order = order
            };

            return PartialView("_EditModal", model);
        }
    }
}
