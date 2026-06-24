using Microsoft.EntityFrameworkCore;
using website_ban_hang.Models;

namespace website_ban_hang.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Hàm khởi tạo (Constructor) cấu hình kết nối Database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Cấu hình các bảng dữ liệu (Tables) trong Cơ sở dữ liệu
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}