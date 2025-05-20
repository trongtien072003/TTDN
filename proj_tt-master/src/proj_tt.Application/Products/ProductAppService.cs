using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using proj_tt.Authorization;
using proj_tt.Products.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;

namespace proj_tt.Products
{
    [AbpAuthorize]
    public class ProductAppService : proj_ttAppServiceBase, IProductAppService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<Product> _productRepository;
        private readonly string _imageRootPath;

        public ProductAppService(IRepository<Product> productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageRootPath = Path.GetFullPath(Path.Combine(webHostEnvironment.ContentRootPath, @"..\Product\ProductImages"));
        }
        [AbpAuthorize(PermissionNames.Pages_Products_Create)]
        public async Task Create(ProductListDto input)
        {
            string imagePath = null;

            // Nếu có ảnh mới thì lưu vào thư mục ProductImages
            if (input.ImageUrl != null && input.ImageUrl.Length > 0)
            {
                imagePath = await SaveImageAsync(input.ImageUrl);
            }

            var product = new Product(
                input.Name.Trim(),
                input.Price,
                imagePath,
                input.Discount,
                input.CategoryId,
                input.Description,
                input.Stock,
                input.ExpiryDate
            );

            await _productRepository.InsertAsync(product);
        }

        [AbpAuthorize]
        public async Task<PagedResultDto<ProductDto>> GetProductPaged(PagedProductDto input)
        {
            input.Normalize();
            var products = _productRepository.GetAllIncluding(p => p.Category).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                var keyword = input.Keyword.ToLower().Trim();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(keyword) ||
                    p.Price.ToString().Contains(keyword) ||
                    p.Discount.ToString().Contains(keyword) ||
                    p.Category.NameCategory.ToLower().Contains(keyword) ||
                    p.CreationTime.ToString().Contains(keyword) ||
                    p.Description.ToLower().Contains(keyword) ||
                    p.Stock.ToString().Contains(keyword) ||
                    p.ExpiryDate.ToString().Contains(keyword)
                );
            }
            if (input.StartDate.HasValue)
            {
                products = products.Where(p => p.ExpiryDate >= input.StartDate.Value);
            }
            if (input.EndDate.HasValue)
            {
                products = products.Where(p => p.ExpiryDate <= input.EndDate.Value);
            }
            if (input.MinPrice.HasValue)
            {
                products = products.Where(p => p.Price >= input.MinPrice.Value);
            }
            if (input.MaxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= input.MaxPrice.Value);
            }

            var count = await products.CountAsync();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                products = products.OrderBy(input.Sorting);
            }

            //        var items = await products
            //.Skip(input.SkipCount)
            //.Take(input.MaxResultCount)
            //.ToListAsync();  
            var items = await products.PageBy(input).ToListAsync();

            var result = items.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Discount = p.Discount,
                CategoryId = p.CategoryId ?? 0,
                NameCategory = p.Category != null ? p.Category.NameCategory : "",
                CreationTime = p.CreationTime,
                LastModificationTime = p.LastModificationTime,
                Description = p.Description,
                Stock = p.Stock,
                ExpiryDate = p.ExpiryDate
            }).ToList();

            return new PagedResultDto<ProductDto>(count, result);
        }


        [AbpAuthorize(PermissionNames.Pages_Products_Edit)]
        public async Task Update(UpdateProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);

            product.Name = input.Name.Trim();
            product.Price = input.Price;
            product.Discount = input.Discount;
            product.CategoryId = input.CategoryId;
            product.Description = input.Description;
            product.Stock = input.Stock;
            product.ExpiryDate = input.ExpiryDate;

            // Nếu có upload ảnh mới
            if (input.ImageUrl != null && input.ImageUrl.Length > 0)
            {
                product.ImageUrl = await SaveImageAsync(input.ImageUrl);
            }
            else
            {
                // Nếu không upload ảnh mới, giữ lại ảnh cũ
                product.ImageUrl = input.ExistingImageUrl;
            }

            await _productRepository.UpdateAsync(product);
        }

        [AbpAuthorize(PermissionNames.Pages_Products_Delete)]
        public async Task Delete(int id)
        {

            await _productRepository.DeleteAsync(id);
        }


        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var savePath = Path.Combine(_imageRootPath, fileName);

            // Tạo thư mục nếu chưa tồn tại
            Directory.CreateDirectory(_imageRootPath);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn tương đối để truy cập từ trình duyệt
            return "/ProductImages/" + fileName;
        }
        public async Task<List<ProductListDto>> GetAllAsync()
        {
            var products = await _productRepository
                .GetAllIncluding(p => p.Category)
                .OrderByDescending(p => p.CreationTime)
                .ToListAsync();

            return ObjectMapper.Map<List<ProductListDto>>(products);
        }
        [AbpAuthorize]
        public async Task<ProductDto> GetProductDetail(int id)
        {
            if (id <= 0)
            {
                throw new UserFriendlyException("ID sản phẩm không hợp lệ: " + id);
            }

            var product = await _productRepository
                .GetAllIncluding(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new UserFriendlyException("Không tìm thấy sản phẩm với ID: " + id);
            }

            var dto = ObjectMapper.Map<ProductDto>(product);
            dto.NameCategory = product.Category?.NameCategory;
            return dto;
        }


    }
}