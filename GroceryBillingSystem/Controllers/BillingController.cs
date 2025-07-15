using GroceryBillingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryBillingSystem.Controllers
{
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Show list of past bills
        public async Task<IActionResult> Bills()
        {
            var bills = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Product)
                .OrderByDescending(b => b.Date)
                .ToListAsync();

            return View(bills);
        }

        // ✅ GET: New billing form
        public IActionResult Create()
        {
            ViewBag.Products = _context.Products.ToList();
            return View();
        }

        // ✅ POST: Create a new bill
        [HttpPost]
        public async Task<IActionResult> Create(Bill bill, List<int> productIds, List<int> quantities, decimal discount)
        {
            if (ModelState.IsValid)
            {
                // ✅ Validate customer fields
                if (bill.Customer == null ||
                    string.IsNullOrWhiteSpace(bill.Customer.Name) ||
                    string.IsNullOrWhiteSpace(bill.Customer.MobileNumber) ||
                    string.IsNullOrWhiteSpace(bill.Customer.Address))
                {
                    ModelState.AddModelError("", "Customer details are required.");
                    ViewBag.Products = _context.Products.ToList();
                    return View(bill);
                }

                // ✅ Save customer first
                _context.Customers.Add(bill.Customer);
                await _context.SaveChangesAsync();

                bill.CustomerId = bill.Customer.CustomerId;
                bill.Date = DateTime.Now;
                bill.TotalAmount = 0;
                bill.BillItems = new List<BillItem>();

                // ✅ Add each product to bill
                for (int i = 0; i < productIds.Count; i++)
                {
                    var product = await _context.Products.FindAsync(productIds[i]);
                    if (product != null && quantities[i] > 0 && product.Quantity >= quantities[i])
                    {
                        decimal itemTotal = product.Rate * quantities[i];

                        bill.BillItems.Add(new BillItem
                        {
                            ProductId = product.ProductId,
                            Quantity = quantities[i],
                            Rate = product.Rate,
                            Total = itemTotal
                        });

                        bill.TotalAmount += itemTotal;

                        // Reduce stock
                        product.Quantity -= quantities[i];
                    }
                }

                // ✅ Apply discount as percentage
                decimal discountAmount = bill.TotalAmount * (discount / 100);
                bill.Discount = discountAmount;
                bill.FinalAmount = bill.TotalAmount - discountAmount;

                // ✅ Save Bill
                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                // ✅ Set success message
                TempData["Success"] = "Thank you for your purchase!";
                return RedirectToAction("Receipt", new { id = bill.BillId });
            }

            ViewBag.Products = _context.Products.ToList();
            return View(bill);
        }

        // ✅ Show final receipt
        public async Task<IActionResult> Receipt(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Customer)
                .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.BillId == id);

            if (bill == null)
                return NotFound();

            return View(bill);
        }
    }
}
