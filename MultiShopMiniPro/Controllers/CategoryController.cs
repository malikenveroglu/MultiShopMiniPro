using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category is null) return NotFound();

            List<Product> products = await _context.Products
                .Where(p => p.CategoryId == id && !p.IsDeleted)
                .OrderBy(p => p.Order)
                .ToListAsync();

            List<Category> otherCategories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            CategoryDetailVM detailVM = new()
            {
                Category = category,
                Products = products,
                Categories = otherCategories
            };

            return View(detailVM);
        }
    }
}