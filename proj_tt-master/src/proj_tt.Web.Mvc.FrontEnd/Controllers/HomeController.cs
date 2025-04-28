using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using proj_tt.Controllers;
using proj_tt.Categories.Dto;
using proj_tt.Categories;
using proj_tt.Products.Dto;
using proj_tt.Products;
using System.Threading.Tasks;
using proj_tt.Web.Models.Share;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace proj_tt.Web.Controllers
{
  
    public class HomeController : proj_ttControllerBase
    {

        private readonly IProductAppService _productAppService;
        private readonly ICategoriesAppService _categoryAppService;

        public HomeController(IProductAppService productAppService, ICategoriesAppService categoryAppService)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
        }
        [AbpMvcAuthorize]
        public async Task<ActionResult> Index(int page = 1, int pageSize = 8)
        {
            var input = new PagedProductDto
            {
                SkipCount = (page - 1) * pageSize,
                MaxResultCount = pageSize
            };

            var products = await _productAppService.GetProductPaged(input);

            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
            var categoriesItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();

            var totalPages = (int)System.Math.Ceiling((double)products.TotalCount / pageSize);

            var model = new IndexShareModel
            {
                Products = products.Items,
                Categories = categoriesItems,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpGet]
       public async Task<IActionResult> ModalDetail(int id)
        {
            var product = await _productAppService.GetProductDetail(id);
            return PartialView("_ModalDetail", product);
        }





    }
}
