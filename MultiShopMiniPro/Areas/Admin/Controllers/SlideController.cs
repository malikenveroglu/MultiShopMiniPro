using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;

        public SlideController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()    
        {
            List<Slide> slides = await _context.Slides.ToListAsync();

            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]    
        public async Task<IActionResult> Create(Slide slide)
        {
            if (!ModelState.IsValid) return View();

            bool result = await _context.Slides.AnyAsync(s => s.Order == slide.Order);

            if (result)
            {
                ModelState.AddModelError(nameof(slide.Order), $"Order {slide.Order} is already exist");
                return View();
            }

            slide.CreatedAt = DateTime.Now;
            
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Slide slide)
        {
            if(!ModelState.IsValid) return View();

            bool result = await _context.Slides.AnyAsync(s => s.Order == slide.Order && s.Id != id);

            if(result)
            {
                ModelState.AddModelError(nameof(slide.Order), $"Order {slide.Order} is already exist");
                return View();
            }

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s=> s.Id == id);

            existed.Title = slide.Title;
            existed.Description = slide.Description;
            existed.SubTitle = slide.SubTitle;
            existed.Order = slide.Order;
            existed.Image = slide.Image;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            return View(existed);
        }
    }
}
