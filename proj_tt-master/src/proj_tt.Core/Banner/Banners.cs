using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Banner
{
    public class Banners: AuditedEntity
    {
        public string ImgSlide { get; set; }
        
    }
}
