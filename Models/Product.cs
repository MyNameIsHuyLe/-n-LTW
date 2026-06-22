using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace website_ban_hang.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        // Khai báo liên kết danh sách ảnh phụ
        public List<ProductImage>? Images { get; set; }

        // Khai báo khóa ngoại đến Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}