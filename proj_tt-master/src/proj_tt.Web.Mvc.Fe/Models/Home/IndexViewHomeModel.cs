using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Products.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Products
{
    public class IndexViewHomeModel
    {

        public ProductDto Product { get; set; }

        public IReadOnlyList<ProductDto> Products { get; }

        public IReadOnlyList<SelectListItem> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }

        public IndexViewHomeModel(IReadOnlyList<ProductDto> products, IReadOnlyList<SelectListItem> categories)
        {
            Products = products;
            Categories = categories;
        }

        public IndexViewHomeModel()
        {
        }
    }
}

