using Microsoft.EntityFrameworkCore;
using website_ban_hang.Models;
using website_ban_hang.Repositories;
using System;

namespace website_ban_hang
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Cấu hình Session trong vùng Services (Đặt TRƯỚC AddControllersWithViews)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session là 30 phút

                // SỬA LỖI TẠI ĐÂY: Thêm thuộc tính .Cookie vào trước HttpOnly và IsEssential
                options.Cookie.HttpOnly = true;  // Bảo mật cookie chống script độc hại
                options.Cookie.IsEssential = true; // Bắt buộc lưu trữ cho ứng dụng hoạt động chính xác
            });

            // 2. Thêm dịch vụ Controller MVC vào Container
            builder.Services.AddControllersWithViews();

            // Kết nối cơ sở dữ liệu SQL Server qua ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ĐỒNG BỘ: Đăng ký EF Repository chạy Async cho DI Container
            builder.Services.AddScoped<IProductRepository, EFProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline. 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // 3. Kích hoạt Middleware Session (Bắt buộc phải đặt SAU UseRouting và TRƯỚC UseEndpoints/MapControllerRoute)
            app.UseSession();

            app.UseAuthorization();

            // Cấu hình định tuyến Route mặc định
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}