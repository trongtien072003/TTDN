using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Products.Dto
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(Product.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile ImageUrl { get; set; } 

        public int Discount { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public string ExistingImageUrl { get; set; } // Lưu ảnh cũ

        public int CategoryId { get; set; }
        public string NameCategory { get; set; }
    }

}
