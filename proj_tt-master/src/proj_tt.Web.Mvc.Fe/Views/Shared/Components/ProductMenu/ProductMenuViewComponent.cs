using Microsoft.AspNetCore.Mvc;
using proj_tt.Products.Dto;
using proj_tt.Products;
using System.Threading.Tasks;
using System.Linq;

namespace proj_tt.Web.Views.Shared.Components.ProductMenu
{
    public class ProductMenuViewComponent : proj_ttViewComponent
    {
        private readonly IUserProductAppService _productAppService;

        public ProductMenuViewComponent(IUserProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var input = new PagedProductDto
            {
                MaxResultCount = 5,
                Sorting = "CreationTime DESC"
            };

            var result = await _productAppService.GetAllAsync(input);

            var model = new ProductMenuViewModel
            {
                Products = result.Items.ToList(),
            };

            return View(model);
        }
    }
}
