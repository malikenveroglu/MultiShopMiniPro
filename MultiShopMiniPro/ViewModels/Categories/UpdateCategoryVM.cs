using System.ComponentModel.DataAnnotations;

namespace MultiShopMiniPro.ViewModels
{
    public class UpdateCategoryVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Image { get; set; }

        public IFormFile? Photo { get; set; }
    }
}