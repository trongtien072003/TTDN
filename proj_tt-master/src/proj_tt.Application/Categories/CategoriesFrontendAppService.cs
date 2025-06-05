using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using proj_tt.Categories.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using proj_tt.Products;

namespace proj_tt.Categories
{
    public interface ICategoriesFrontendAppService: IApplicationService
    {
        Task<PagedResultDto<CategoryListDto>> GetCategory(GetAllCategory input);
    }
    [AllowAnonymous]

    public class CategoriesFrontendAppService : proj_ttAppServiceBase, ICategoriesFrontendAppService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;

        public CategoriesFrontendAppService(IRepository<Category> categoryRepository,IRepository<Product> productRepository )
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        [AllowAnonymous]

        public async Task<PagedResultDto<CategoryListDto>> GetCategory(GetAllCategory input)
        {
            using var uow = UnitOfWorkManager.Begin();
            using (CurrentUnitOfWork.SetTenantId(AbpSession.TenantId))
            {
                try
                {
                    var query = _categoryRepository.GetAll();

                    // ✅ Thêm điều kiện tìm kiếm
                    if (!string.IsNullOrWhiteSpace(input.Keyword))
                    {
                        query = query.Where(x => x.NameCategory.Contains(input.Keyword));
                    }

                    var totalCount = await query.CountAsync();

                    var categoryDtos = await query
                        .OrderBy(x => x.NameCategory)
                        .PageBy(input)
                        .Select(p => new CategoryListDto
                        {
                            Id = p.Id,
                            NameCategory = p.NameCategory
                        })
                        .ToListAsync();

                    return new PagedResultDto<CategoryListDto>(totalCount, categoryDtos);
                }
                catch
                {
                    return null;
                }
                finally
                {
                    await uow.CompleteAsync();
                }
            }
        }

    }
}
