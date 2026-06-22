using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using website_ban_hang.Repositories; // Đảm bảo có namespace này để nhận diện IProductRepository

namespace website_ban_hang.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        // Constructor để inject IProductRepository tham khảo slide tài liệu
        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Hiển thị danh sách sản phẩm ra trang chủ (Async)
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
    }
}