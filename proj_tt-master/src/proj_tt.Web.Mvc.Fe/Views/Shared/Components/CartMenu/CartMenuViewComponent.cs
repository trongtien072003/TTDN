using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Cart;
using proj_tt.Web.Views.Shared.Components.CartMenu;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Views.Shared.Components.CartSummary
{
    public class CartMenuViewComponent : proj_ttViewComponent
    {
        private readonly ICartAppService _cartAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _abpSession;

        public CartMenuViewComponent(
            ICartAppService cartAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession)
        {
            _cartAppService = cartAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _abpSession = abpSession;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(_abpSession.TenantId))
                {
                    var model = new CartMenuViewModel
                    {
                        CartItem = await _cartAppService.CountCartItemsAsync()
                    };

                    await uow.CompleteAsync();
                    return View(model);
                }
            }
        }
    }
}
