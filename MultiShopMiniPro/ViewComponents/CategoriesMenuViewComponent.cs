using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopMiniPro.DAL;
using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.ViewComponents
{
    public class CategoriesMenuViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public CategoriesMenuViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = await _context.Categories
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }
    }
}