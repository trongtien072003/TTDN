﻿//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using proj_tt.Categories;
//using proj_tt.Categories.Dto;
//using proj_tt.Controllers;
//using proj_tt.Products;
//using proj_tt.Products.Dto;
//using proj_tt.Web.Models.Products;
//using System.Linq;
//using System.Threading.Tasks;

//namespace proj_tt.Web.Controllers
//{
//    public class ProductController : proj_ttControllerBase
//    {

//        private readonly IProductAppService _productAppService;
//        private readonly ICategoriesAppService _categoryAppService;

//        public ProductController(IProductAppService productAppService, ICategoriesAppService categoryAppService)
//        {
//            _productAppService = productAppService;
//            _categoryAppService = categoryAppService;
//        }

//        public async Task<ActionResult> Index()
//        {
//            var products = await _productAppService.GetProductPaged(new PagedProductDto());

//            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
//            var categoriesItems = categories.Items.Select(c => new SelectListItem
//            {
//                Value = c.Id.ToString(),
//                Text = c.NameCategory
//            }).ToList();
//            var model = new IndexViewModel(products.Items, categoriesItems);
//            return View(model);
//        }

//        public async Task<IActionResult> Create(ProductListDto input)
//        {
//            await _productAppService.Create(input);
//            return Ok();
//        }

//        public async Task<IActionResult> Update(UpdateProductDto input)
//        {
//            await _productAppService.Update(input);
//            return Ok();
//        }


//        public async Task<ActionResult> EditModal(int productId)
//        {
//            var product = await _productAppService.GetProductDetail(productId);
//            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
//            var model = new IndexViewModel
//            {
//                Product = product,
//                Categories = categories.Items.Select(x => new SelectListItem
//                {
//                    Value = x.Id.ToString(),
//                    Text = x.NameCategory,
//                    Selected = (x.Id == product.CategoryId)
//                }).ToList()
//            };

//            return PartialView("_EditModal", model);
//        }
//        public async Task<IActionResult> Detail(int id)
//        {
//            // Kiểm tra ID hợp lệ
//            if (id <= 0)
//            {
//                return RedirectToAction("Index", "Home");
//            }

//            try
//            {
//                var product = await _productAppService.GetProductDetail(id);
//                var model = new IndexViewHomeModel
//                {
//                    Product = product
//                };
//                return View("Detail", model);
//            }
//            catch
//            {
//                // Nếu không tìm thấy thì chuyển về trang chủ hoặc trang lỗi tùy bạn
//                return RedirectToAction("Index", "Home");
//            }
//        }


//    }
//}
