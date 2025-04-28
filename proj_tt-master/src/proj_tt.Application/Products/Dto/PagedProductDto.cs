using System;
using System.Globalization;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace proj_tt.Products.Dto
{
    public class PagedProductDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {


        public string Keyword { get; set; }
    
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinPrice { get; set; } 
        public decimal? MaxPrice { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrWhiteSpace(Sorting) || Sorting == "0 ASC")
            {
                Sorting = "CreationTime DESC";
            }

        }

    }
}
