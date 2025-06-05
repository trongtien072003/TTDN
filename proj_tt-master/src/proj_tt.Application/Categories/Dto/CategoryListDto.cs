using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Categories.Dto
{
   public class CategoryListDto : EntityDto
    {
        public string NameCategory { get; set; }
        
    }
}
