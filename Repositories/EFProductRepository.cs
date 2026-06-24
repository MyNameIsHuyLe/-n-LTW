using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using website_ban_hang.Models;

namespace website_ban_hang.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor để inject ApplicationDbContext (kết nối database thực tế)
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ danh sách sản phẩm kèm theo thông tin Category (Async)
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                                 .Include(p => p.Category) // Include thông tin danh mục tương ứng
                                 .ToListAsync();
        }

        // Tìm một sản phẩm dựa trên Id kèm theo thông tin Category (Async)
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Thêm sản phẩm mới vào Database (Async)
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin sản phẩm (Async)
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm dựa trên Id (Async)
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        // 🔍 Tìm kiếm sản phẩm theo tên + danh mục (Async)
        public async Task<IEnumerable<Product>> SearchAsync(string keyword, int? categoryId)
        {
            var query = _context.Products
                                .Include(p => p.Category)
                                .AsQueryable();

            // Tìm theo tên sản phẩm
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }

            // Lọc theo danh mục
            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            return await query.ToListAsync();
        }
    }
}
