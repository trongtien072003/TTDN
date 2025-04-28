using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Order.Dto
{
    public class PagedOrderResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public OrderStatus? Status { get; set; }
        public long? UserId { get; set; }
    }
}
