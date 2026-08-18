using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;
using MultiShopMiniPro.Utilities.Enums;
using MultiShopMiniPro.Utilities.Extensions;
using MultiShopMiniPro.ViewModels;

namespace MultiShopMiniPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public IWebHostEnvironment _env { get; }

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Products.Where(p => !p.IsDeleted))
                .ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();

            if (categoryVM.Photo is null)
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Photo), "Photo is required");
                return View(categoryVM);
            }

            if (!categoryVM.Photo.ValidateSize(FileSize.MB, 5))
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Photo), "File size must be less than 5 MB");
                return View();
            }

            if (!categoryVM.Photo.ValidateType("image"))
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Photo), "File type is invalid");
                return View();
            }

            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name && !c.IsDeleted);

            if (result)
            {
                ModelState.AddModelError(nameof(CreateCategoryVM.Name), $"Category '{categoryVM.Name}' already exists");
                return View();
            }

            string image = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");

            Category category = new Category
            {
                Name = categoryVM.Name,
                Image = image,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (existed is null) return NotFound();

            UpdateCategoryVM categoryVM = new()
            {
                Name = existed.Name,
                Image = existed.Image
            };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateCategoryVM categoryVM)
        {
            if (id is null || id < 1) return BadRequest();

            if (!ModelState.IsValid) return View(categoryVM);

            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (existed is null) return NotFound();

            bool result = await _context.Categories.AnyAsync(c => c.Name == categoryVM.Name && c.Id != id && !c.IsDeleted);

            if (result)
            {
                ModelState.AddModelError(nameof(UpdateCategoryVM.Name), $"Category '{categoryVM.Name}' already exists");
                categoryVM.Image = existed.Image;
                return View(categoryVM);
            }

            if (categoryVM.Photo is not null)
            {
                if (!categoryVM.Photo.ValidateSize(FileSize.MB, 5))
                {
                    ModelState.AddModelError(nameof(UpdateCategoryVM.Photo), "File size must be less than 5 MB");
                    categoryVM.Image = existed.Image;
                    return View(categoryVM);
                }

                if (!categoryVM.Photo.ValidateType("image"))
                {
                    ModelState.AddModelError(nameof(UpdateCategoryVM.Photo), "File type is invalid");
                    categoryVM.Image = existed.Image;
                    return View(categoryVM);
                }

                string newFileName = await categoryVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.Image = newFileName;
            }

            existed.Name = categoryVM.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? existed = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (existed is null) return NotFound();

            if (existed.Products is not null && existed.Products.Any())
            {
                ModelState.AddModelError(string.Empty, $"Cannot delete '{existed.Name}' because it has {existed.Products.Count} product(s) assigned to it.");

                List<Category> categories = await _context.Categories
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Products.Where(p => !p.IsDeleted))
                    .ToListAsync();

                return View("Index", categories);
            }

            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? existed = await _context.Categories
                .Include(c => c.Products.Where(p => !p.IsDeleted))
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (existed is null) return NotFound();

            return View(existed);
        }
    }
}