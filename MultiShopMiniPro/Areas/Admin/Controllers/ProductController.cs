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
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public IWebHostEnvironment _env { get; }

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.Order)
                .ToListAsync();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid) return View(productVM);

            if (productVM.Photo is null)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "Photo is required");
                return View();
            }

            if (!productVM.Photo.ValidateSize(FileSize.MB, 5))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File size must be less than 5 MB");
                return View();
            }

            if (!productVM.Photo.ValidateType("image"))
            {
                ModelState.AddModelError(nameof(CreateProductVM.Photo), "File type is invalid");
                return View();
            }

            bool result = await _context.Products.AnyAsync(p => p.Order == productVM.Order && !p.IsDeleted);

            if (result)
            {
                ModelState.AddModelError(nameof(CreateProductVM.Order), $"Order {productVM.Order} is already exist");
                return View(productVM);
            }

            string image = await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");

            Product product = new Product
            {
                Name = productVM.Name,
                Price = productVM.Price,
                ExPrice = productVM.ExPrice,
                SubTitle = productVM.SubTitle,
                Description = productVM.Description,
                Order = productVM.Order,
                Image = image,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (existed is null) return NotFound();

            UpdateProductVM productVM = new()
            {
                Name = existed.Name,
                Price = existed.Price,
                ExPrice = existed.ExPrice,
                SubTitle = existed.SubTitle,
                Description = existed.Description,
                Order = existed.Order,
                Image = existed.Image
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            if (id is null || id < 1) return BadRequest();

            if (!ModelState.IsValid) return View(productVM);

            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (existed is null) return NotFound();

            bool result = await _context.Products.AnyAsync(p => p.Order == productVM.Order && p.Id != id && !p.IsDeleted);

            if (result)
            {
                ModelState.AddModelError(nameof(UpdateProductVM.Order), $"Order {productVM.Order} is already exist");
                productVM.Image = existed.Image;
                return View(productVM);
            }

            if (productVM.Photo is not null)
            {
                if (!productVM.Photo.ValidateSize(FileSize.MB, 5))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File size must be less than 5 MB");
                    productVM.Image = existed.Image;
                    return View(productVM);
                }

                if (!productVM.Photo.ValidateType("image"))
                {
                    ModelState.AddModelError(nameof(UpdateProductVM.Photo), "File type is invalid");
                    productVM.Image = existed.Image;
                    return View(productVM);
                }

                string newFileName = await productVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.Image = newFileName;
            }

            existed.Name = productVM.Name;
            existed.Price = productVM.Price;
            existed.ExPrice = productVM.ExPrice;
            existed.SubTitle = productVM.SubTitle;
            existed.Description = productVM.Description;
            existed.Order = productVM.Order;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (existed is null) return NotFound();

            existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");

            _context.Products.Remove(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Product? existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (existed is null) return NotFound();

            return View(existed);
        }
    }
}