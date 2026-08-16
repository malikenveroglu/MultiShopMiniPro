using Microsoft.AspNetCore.Http.HttpResults;
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
        public IActionResult Index()
        {
            return View();
        }

        public async  Task<IActionResult> Detail(int? id)
        {
            if(id is null || id < 1) return BadRequest();

            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            List<Product> relatedProds = await _context.Products.Where(p => p.Id > 5).ToListAsync();

            DetailVM detailVM = new()
            {
                Product = product,
                RelatedProducts = relatedProds
            };

            return View(detailVM);
        }
    }
}
