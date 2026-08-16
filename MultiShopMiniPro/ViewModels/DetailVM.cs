using MultiShopMiniPro.Models;

namespace MultiShopMiniPro.ViewModels
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
