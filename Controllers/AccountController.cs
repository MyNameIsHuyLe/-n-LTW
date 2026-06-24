using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website_ban_hang.Models;

namespace website_ban_hang.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool exists = await _context.Customers
                .AnyAsync(x => x.Email == model.Email);

            if (exists)
            {
                ModelState.AddModelError("", "Email này đã được sử dụng");
                return View(model);
            }

            model.Role = "User";
            model.IsActive = true;

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đăng ký tài khoản thành công!";

            return RedirectToAction("Login");
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ email và mật khẩu";
                return View();
            }

            var customer = _context.Customers
                .FirstOrDefault(x =>
                    x.Email == email &&
                    x.Password == password);

            if (customer == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng";
                return View();
            }

            // KIỂM TRA TÀI KHOẢN BỊ KHÓA
            if (!customer.IsActive)
            {
                ViewBag.Error = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.";
                return View();
            }

            HttpContext.Session.SetInt32("CustomerId", customer.Id);
            HttpContext.Session.SetString("CustomerName", customer.FullName);
            HttpContext.Session.SetString("Role", customer.Role);

            return RedirectToAction("Index", "Home");
        }

        // ================= LOGOUT =================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}