using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Order.Dto
{
    public class PagedOrderRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }

    }
}
