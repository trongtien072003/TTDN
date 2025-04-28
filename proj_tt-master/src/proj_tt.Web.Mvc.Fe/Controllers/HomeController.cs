using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using proj_tt.Controllers;
using proj_tt.Categories.Dto;
using proj_tt.Categories;
using proj_tt.Products.Dto;
using proj_tt.Products;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Web.Models.Products;
using Abp.Authorization;
using Microsoft.AspNetCore.Authorization;
using System;

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
        [AbpAuthorize]
        public async Task<ActionResult> Index()
        {
            var products = await _productAppService.GetProductPaged(new PagedProductDto());

            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
            var categoriesItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();
            var model = new IndexViewHomeModel(products.Items, categoriesItems);
            return View(model);
        }
        [HttpPost]
        [Route("Product/DetailModal")]
        [AllowAnonymous]
        public async Task<PartialViewResult> DetailModal(int productId)
        {
            var product = await _productAppService.GetProductDetail(productId);
            return PartialView("_ProductDetailModal", product);
        }
       
        public async Task<ActionResult> Page(int pageNumber = 1, int pageSize = 10)
        {
            // Giới hạn đầu vào hợp lệ
            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Max(pageSize, 1);

            // Tạo DTO gọi dịch vụ
            var input = new PagedProductDto
            {
                SkipCount = (pageNumber - 1) * pageSize,
                MaxResultCount = pageSize
            };

            // Gọi AppService lấy dữ liệu
            var products = await _productAppService.GetProductPaged(input);
            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());

            var categoriesItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();

            var totalPages = (int)Math.Ceiling(products.TotalCount / (double)pageSize);
            pageNumber = Math.Min(pageNumber, totalPages == 0 ? 1 : totalPages); // xử lý khi không có sản phẩm

            var model = new IndexViewHomeModel(products.Items, categoriesItems)
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
