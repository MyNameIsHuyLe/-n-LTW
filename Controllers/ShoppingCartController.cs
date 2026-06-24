using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using website_ban_hang.Models;
using website_ban_hang.Repositories;
using website_ban_hang.Extensions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace website_ban_hang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(
            IProductRepository productRepository,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        // =========================
        // THÊM GIỎ HÀNG
        // =========================
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await GetProductFromDatabase(productId);

            if (product == null)
                return NotFound();

            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };

            var cart = HttpContext.Session
                .GetObjectFromJson<ShoppingCart>("Cart")
                ?? new ShoppingCart();

            cart.AddItem(cartItem);

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }

        // =========================
        // XEM GIỎ HÀNG
        // =========================
        public IActionResult Index()
        {
            var cart = HttpContext.Session
                .GetObjectFromJson<ShoppingCart>("Cart")
                ?? new ShoppingCart();

            return View(cart);
        }

        // =========================
        // XÓA SẢN PHẨM
        // =========================
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session
                .GetObjectFromJson<ShoppingCart>("Cart");

            if (cart != null)
            {
                cart.RemoveItem(productId);

                HttpContext.Session
                    .SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // =========================
        // THANH TOÁN
        // =========================
        public async Task<IActionResult> Checkout()
        {
            var customerId =
                HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = HttpContext.Session
                .GetObjectFromJson<ShoppingCart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                TempData["Message"] = "Giỏ hàng đang trống!";
                return RedirectToAction("Index");
            }

            decimal totalAmount =
                cart.Items.Sum(x => x.Price * x.Quantity);

            var order = new Order
            {
                CustomerId = customerId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Completed"
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            // Xóa giỏ hàng sau khi thanh toán
            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Đặt hàng thành công!";

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // LẤY SẢN PHẨM TỪ DB
        // =========================
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            return await _productRepository
                .GetByIdAsync(productId);
        }
    }
}