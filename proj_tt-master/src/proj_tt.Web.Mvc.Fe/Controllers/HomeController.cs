using Microsoft.AspNetCore.Mvc;
using proj_tt.Controllers;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using proj_tt.Products;
using proj_tt.Products.Dto;
using proj_tt.Web.Models.Products;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Abp.Authorization;

namespace proj_tt.Web.Controllers
{
 
    public class HomeController : proj_ttControllerBase
    {
        private readonly IUserProductAppService _userProductAppService;
        private readonly ICategoriesFrontendAppService _categoryFrontendAppService;

        public HomeController(IUserProductAppService userProductAppService, ICategoriesFrontendAppService categoryFrontendAppService)
        {
            _userProductAppService = userProductAppService;
            _categoryFrontendAppService = categoryFrontendAppService;
        }
        [AbpAuthorize]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var products = await _userProductAppService.GetAllAsync(new PagedProductDto());

            var categories = await _categoryFrontendAppService.GetAll(new PagedCategoriesDto());
            var categoryItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();

            var model = new IndexViewHomeModel(products.Items, categoryItems);
            return View(model);
        }

        [HttpGet("/Home/Detail/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
                return BadRequest("ID sản phẩm không hợp lệ");

            var productDto = await _userProductAppService.GetAsync(new EntityDto<int>(id));

            // ✅ Trả về View với model là ProductDto
            return View("Detail", productDto);
        }



        [HttpGet("Home/Page")]
        public async Task<ActionResult> Page(int pageNumber = 1, int pageSize = 10)
        {
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            var input = new PagedProductDto
            {
                SkipCount = (pageNumber - 1) * pageSize,
                MaxResultCount = pageSize
            };

            var products = await _userProductAppService.GetAllAsync(input);
            var categories = await _categoryFrontendAppService.GetAll(new PagedCategoriesDto());

            var categoryItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();

            var totalPages = (int)Math.Ceiling(products.TotalCount / (double)pageSize);

            var model = new IndexViewHomeModel(products.Items, categoryItems)
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = products.TotalCount,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };

            return View("Index", model);
        }
    }
}
