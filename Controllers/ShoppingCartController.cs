using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using website_ban_hang.Models;
using website_ban_hang.Repositories;
using website_ban_hang.Extensions; // Hãy đảm bảo có namespace chứa hàm mở rộng GetObjectFromJson và SetObjectAsJson

namespace website_ban_hang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;

        // Constructor tiêm Repository để kết nối database lấy thông tin sản phẩm
        public ShoppingCartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Action 1: Thêm sản phẩm vào Giỏ hàng
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu dựa trên productId
            var product = await GetProductFromDatabase(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Tạo một mục giỏ hàng mới
            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };

            // Đọc giỏ hàng hiện tại từ Session, nếu chưa có thì khởi tạo mới
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);

            // Lưu lại cấu trúc giỏ hàng mới cập nhật vào Session dưới dạng JSON
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }

        // Action 2: Hiển thị danh sách các sản phẩm trong Giỏ hàng
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        // Action 3: Xóa một mục sản phẩm ra khỏi Giỏ hàng
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            if (cart != null)
            {
                cart.RemoveItem(productId);

                // Lưu lại trạng thái giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // Hàm bổ trợ: Truy vấn dữ liệu thực thể Product từ Database chạy Async
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }
    }
}