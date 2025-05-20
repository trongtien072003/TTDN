using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using proj_tt.Products.Dto;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;

namespace proj_tt.Products
{
    [AllowAnonymous]
    public class UserProductAppService : ApplicationService, IUserProductAppService
    {
        private readonly IRepository<Product> _productRepository;

        public UserProductAppService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm có hỗ trợ tìm kiếm, lọc theo danh mục, tên danh mục, giá, còn hạn, sắp xếp, phân trang
        /// </summary>
        [AllowAnonymous]
        public async Task<PagedResultDto<ProductDto>> GetAllAsync(PagedProductDto input)
        {
            var query = _productRepository.GetAll()
                .Include(p => p.Category)
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), p => p.Name.Contains(input.Keyword))
                .WhereIf(input.CategoryId.HasValue, p => p.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryName), p => p.Category != null && p.Category.NameCategory.Contains(input.CategoryName))
                .WhereIf(input.MinPrice.HasValue, p => p.Price >= input.MinPrice.Value)
                .WhereIf(input.MaxPrice.HasValue, p => p.Price <= input.MaxPrice.Value)
                .WhereIf(input.ExpiryOnly == true, p => p.ExpiryDate.HasValue && p.ExpiryDate.Value >= DateTime.Today);

            var totalCount = await query.CountAsync();

            // ✅ Danh sách các trường hợp lệ được phép sort
            var allowedSortFields = new[] { "Name", "Price", "CreationTime", "Discount", "Stock" };
            string sortField = input.Sorting;

            if (string.IsNullOrWhiteSpace(sortField) || !allowedSortFields.Any(f => sortField.StartsWith(f, StringComparison.OrdinalIgnoreCase)))
            {
                sortField = "CreationTime DESC";
            }

            var items = await query
                .OrderBy(sortField)
                .PageBy(input)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Discount = p.Discount,
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Stock = p.Stock,
                    ExpiryDate = p.ExpiryDate,
                    CategoryId = p.CategoryId ?? 0,
                    NameCategory = p.Category != null ? p.Category.NameCategory : null
                })
                .ToListAsync();

            return new PagedResultDto<ProductDto>(totalCount, items);
        }

        /// <summary>
        /// Lấy chi tiết một sản phẩm cụ thể theo Id
        /// </summary>
         [AllowAnonymous] 
        public async Task<ProductDto> GetAsync(EntityDto<int> input)
        {
            if (input.Id <= 0)
                throw new UserFriendlyException("ID sản phẩm không hợp lệ.");

            var product = await _productRepository
                .GetAllIncluding(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == input.Id);

            if (product == null)
                throw new UserFriendlyException($"Không tìm thấy sản phẩm với ID: {input.Id}");

            var dto = ObjectMapper.Map<ProductDto>(product);
            dto.NameCategory = product.Category?.NameCategory;

            return dto;
        }
    }
}
