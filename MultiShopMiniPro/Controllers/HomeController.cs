using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> feauterdProducts = await _context.Products.OrderBy(p => p.Order).Where(p=>p.Order < 9).ToListAsync();

            List<Product> recentProducts = await _context.Products.OrderBy(p => p.Order).Where(p=>p.Order > 8).ToListAsync();

            List<Slide> slides = await _context.Slides.OrderBy(s => s.Order).Take(3).ToListAsync();

            List<Slide> offers = await _context.Slides.Where(o=>o.Order==4 || o.Order==5).ToListAsync();


            HomeVM homeVM = new()
            {
                FeaturedProducts = feauterdProducts,
                RecentProducts = recentProducts,
                Slides = slides,
                Offers = offers
            };

            return View(homeVM);  
        }
    }
}
