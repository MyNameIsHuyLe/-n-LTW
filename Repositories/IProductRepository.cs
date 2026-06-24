using System.Collections.Generic;
using System.Threading.Tasks;
using website_ban_hang.Models;

namespace website_ban_hang.Repositories
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

        
        Task<IEnumerable<Product>> SearchAsync(string keyword, int? categoryId);
    }
}