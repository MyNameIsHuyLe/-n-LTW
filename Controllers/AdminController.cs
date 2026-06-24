using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using website_ban_hang.Models;

namespace website_ban_hang.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= DOANH THU =================
        public async Task<IActionResult> Revenue()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.TotalRevenue =
                await _context.Orders.SumAsync(x => x.TotalAmount);

            ViewBag.TotalOrders =
                await _context.Orders.CountAsync();

            return View();
        }

        // ================= DANH SÁCH TÀI KHOẢN =================
        public async Task<IActionResult> Users()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var users = await _context.Customers.ToListAsync();

            return View(users);
        }

        // ================= ĐỔI QUYỀN =================
        public async Task<IActionResult> ChangeRole(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Customers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Role = user.Role == "Admin"
                ? "User"
                : "Admin";

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        // ================= KHÓA / MỞ KHÓA =================
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Customers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = !user.IsActive;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }
    }
}