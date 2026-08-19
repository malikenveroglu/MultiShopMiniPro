using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.ViewModels
{
    public class CategoryDetailVM
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}