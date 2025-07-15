using GroceryBillingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryBillingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalProducts = await _context.Products.CountAsync();
            var totalBills = await _context.Bills.CountAsync();
            var totalRevenue = await _context.Bills.SumAsync(b => b.FinalAmount);
            var today = DateTime.Today;
            var todayRevenue = await _context.Bills
                .Where(b => b.Date.Date == today)
                .SumAsync(b => b.FinalAmount);

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalBills = totalBills;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TodayRevenue = todayRevenue;

            return View();
        }
    }
}
