using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .OrderBy(p => p.Order)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Product? product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (product is null) return NotFound();

            List<Product> relatedProds = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && !p.IsDeleted)
                .Include(p => p.Category)
                .OrderBy(p => p.Order)
                .ToListAsync();

            DetailVM detailVM = new()
            {
                Product = product,
                RelatedProducts = relatedProds
            };

            return View(detailVM);
        }
    }
}