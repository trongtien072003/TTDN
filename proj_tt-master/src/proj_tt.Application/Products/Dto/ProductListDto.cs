using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Products.Dto
{
    [AutoMapTo(typeof(Product))]

    public class ProductListDto
    {
        //public int Id { get; set; }

        [Required]
        [StringLength(Product.MaxNameLength)]
        public string Name { get; set; }
        [Required]

        public decimal Price { get; set; }
        //[Required]

        public IFormFile ImageUrl { get; set; }

        public int Discount { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int CategoryId { get; set; }
        public string NameCategory { get; set; }



    }
}
