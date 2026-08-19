using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.ViewModels
{
    public class HomeVM
    {
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> RecentProducts { get; set; }
        public List<Slide> Slides { get; set; }
        public List<Slide> Offers { get; set; }
        public List<Category> Categories { get; set; }

    }
}
