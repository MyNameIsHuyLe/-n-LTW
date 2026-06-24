using Microsoft.AspNetCore.Mvc;
using website_ban_hang.Models;

public class CustomerController : Controller
{
    private readonly ApplicationDbContext _context;

    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public IActionResult Edit(int id)
    {
        var customer = _context.Customers.FirstOrDefault(x => x.Id == id);

        if (customer == null)
        {
            return NotFound();
        }

        return View(customer);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Customer model)
    {
        System.Diagnostics.Debug.WriteLine("🔥 POST EDIT CHẠY OK");

        var customer = _context.Customers.FirstOrDefault(x => x.Id == model.Id);

        if (customer == null)
        {
            return NotFound();
        }

        customer.FullName = model.FullName;
        customer.Email = model.Email;
        customer.PhoneNumber = model.PhoneNumber;
        customer.Address = model.Address;

        _context.SaveChanges();

        TempData["Success"] = "Cập nhật thành công!";

        return RedirectToAction("Edit", new { id = model.Id });
    }
}