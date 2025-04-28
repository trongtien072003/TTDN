using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Products.Dto;

namespace proj_tt.Web.Models.Share
{
    public class IndexShareModel
    {
        public ProductDto Product { get; set; }

       
        public IReadOnlyList<ProductDto> Products { get; set; }
        public IReadOnlyList<SelectListItem> Categories { get; set; }
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }
        public IndexShareModel(IReadOnlyList<ProductDto> products, IReadOnlyList<SelectListItem> categories)
        {
            Products = products;
            Categories = categories;
        }
        public IndexShareModel()
        {
        }
    }
}
