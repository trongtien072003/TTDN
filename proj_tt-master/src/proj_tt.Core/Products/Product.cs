using Abp.Domain.Entities.Auditing;
using proj_tt.Categories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Products
{
    [Table("AppProducts")]
    public class Product : AuditedEntity
    {
        public const int MaxNameLength = 256;

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(MaxNameLength, ErrorMessage = "Tên sản phẩm không được vượt quá 256 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là số không âm")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ảnh sản phẩm là bắt buộc")]
        [Url(ErrorMessage = "Đường dẫn ảnh không hợp lệ")]
        public string ImageUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Chiết khấu phải từ 0 đến 100")]
        public int Discount { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tồn kho phải là số không âm")]
        public int Stock { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Product), nameof(ValidateExpiryDate))]
        public DateTime? ExpiryDate { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        public int? CategoryId { get; set; }

        public Product(string name, decimal price, string imageUrl, int discount = 0, int? categoryId = null, string description = null, int stock = 0, DateTime? expiryDate = null)
        {
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Discount = discount;
            CategoryId = categoryId;
            Description = description;
            Stock = stock;
            ExpiryDate = expiryDate;
        }

        // Custom validation method for ExpiryDate
        public static ValidationResult ValidateExpiryDate(DateTime? expiryDate, ValidationContext context)
        {
            if (expiryDate.HasValue && expiryDate.Value < DateTime.Today)
            {
                return new ValidationResult("Ngày hết hạn phải lớn hơn hoặc bằng ngày hôm nay");
            }
            return ValidationResult.Success;
        }
    }
}
