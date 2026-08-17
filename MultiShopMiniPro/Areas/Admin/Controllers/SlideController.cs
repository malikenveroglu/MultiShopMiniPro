using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.Utilities.Enums;
using MultiShopMiniPro.Utilities.Extensions;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        public IWebHostEnvironment _env { get; }

        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid) return View();

            if (!slideVM.Photo.ValidateSize(FileSize.MB,2))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo),"File size must be less than 2 MB");
                return View();
            }

            if (!slideVM.Photo.ValidateType("image"))
            {
                ModelState.AddModelError(nameof(CreateSlideVM.Photo), "File type is invalid");
                return View();
            }

            bool result = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order);

            if (result)
            {
                ModelState.AddModelError(nameof(Slide.Order), $"Order {slideVM.Order} is already exist");
                return View();
            }

            string image = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "img");

            Slide slide = new Slide
            {
                Title = slideVM.Title,
                SubTitle = slideVM.SubTitle,
                Description = slideVM.Description,
                Order = slideVM.Order,
                Image = image,
                CreatedAt = DateTime.Now,
            };
            
            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if(id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            UpdateSlideVM slideVM = new()
            {
                Title = existed.Title,
                SubTitle = existed.SubTitle,
                Description = existed.Description,
                Image = existed.Image,
                Order = existed.Order
            };

            return View(slideVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if(!ModelState.IsValid) return View(slideVM);

            if(slideVM.Photo is not null)
            {
                if(!slideVM.Photo.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "File size must be less than 2 MB");
                    return View(slideVM);
                }

                if (!slideVM.Photo.ValidateType("image"))
                {
                    ModelState.AddModelError(nameof(UpdateSlideVM.Photo), "File type is invalid");
                    return View(slideVM);
                }
            }

            bool result = await _context.Slides.AnyAsync(s => s.Order == slideVM.Order && s.Id != id);

            if(result)  
            {
                ModelState.AddModelError(nameof(UpdateSlideVM.Order), $"Order {slideVM.Order} is already exist");
                return View(slideVM);
            }

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slideVM.Photo is not null)
            {
                string newFileName = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "img");
                existed.Image.DeleteFile(_env.WebRootPath, "img");
                existed.Image = newFileName;
            }

            

            existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.SubTitle = slideVM.SubTitle;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            existed.Image.DeleteFile(_env.WebRootPath,"img");

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
