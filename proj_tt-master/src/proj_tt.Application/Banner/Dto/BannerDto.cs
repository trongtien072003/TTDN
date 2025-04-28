using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Banner.Dto
{
   public  class BannerDto : AuditedEntityDto
    {
        public string ImgSlide { get; set; }
    }
}
