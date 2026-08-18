using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShopMiniPro.Models
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Product> Products { get; set; }

        [NotMapped]
        public int ProductCount => Products?.Count ?? 0;
    }
}
