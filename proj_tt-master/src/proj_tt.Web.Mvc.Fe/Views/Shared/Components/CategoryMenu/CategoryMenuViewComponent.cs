using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Views.Shared.Components.CategoryMenu
{
    public class CategoryMenuViewComponent : proj_ttViewComponent
    {
        private readonly ICategoriesFrontendAppService _categoriesFrontendAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CategoryMenuViewComponent(
            ICategoriesFrontendAppService categoriesFrontendAppService,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _categoriesFrontendAppService = categoriesFrontendAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                using (_unitOfWorkManager.Current.SetTenantId(AbpSession.TenantId))
                {
                    var result = await _categoriesFrontendAppService.GetCategory(new GetAllCategory
                    {
                        Keyword = null,
                        MaxResultCount = 100
                    });

                    await uow.CompleteAsync();

                    var model = new CategoryMenuViewModel
                    {
                        Categories = new ListResultDto<CategoryListDto>
                        {
                            Items = result.Items.ToList()
                        }
                    };

                    return View(model);
                }
            }
        }
    }
}
