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

namespace proj_tt.Categories
{
    public interface ICategoriesFrontendAppService: IApplicationService
    {
        Task<PagedResultDto<CategoriesDto>> GetAll(PagedCategoriesDto input);
    }
    [AllowAnonymous]

    public class CategoriesFrontendAppService : proj_ttAppServiceBase, ICategoriesFrontendAppService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoriesFrontendAppService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [AllowAnonymous]
        public async Task<PagedResultDto<CategoriesDto>> GetAll(PagedCategoriesDto input)
        {
            var categories = _categoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                categories = categories.Where(p => p.NameCategory.Contains(input.Keyword));
            }

            var count = await categories.CountAsync();

            input.Sorting = "CreationTime DESC";

            var items = await categories.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<CategoriesDto> { TotalCount = count, Items = ObjectMapper.Map<List<CategoriesDto>>(items) };
        }

    }
}
