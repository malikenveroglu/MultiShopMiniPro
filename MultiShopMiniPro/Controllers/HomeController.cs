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
            List<Slide> slides = await _context.Slides.OrderBy(s => s.Order).Take(3).ToListAsync();

            List<Slide> offers = await _context.Slides.Where(o=>o.Order==4 || o.Order==5).ToListAsync();


            HomeVM homeVM = new()
            {
                Slides = slides,
                Offers = offers.OrderBy(s => s.Order).Where(o => o.Order == 4 || o.Order == 5).ToList()
            };

            return View(homeVM);  
        }
    }
}
