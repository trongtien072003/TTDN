using Abp.Application.Services.Dto;
using proj_tt.Categories.Dto;

namespace proj_tt.Web.Views.Shared.Components.CategoryMenu
{
    public class CategoryMenuViewModel
    {
        public ListResultDto<CategoryListDto> Categories { get; set; }
    }
}
