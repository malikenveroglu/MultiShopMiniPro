using System.ComponentModel.DataAnnotations;

namespace MultiShopMiniPro.ViewModels
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public IFormFile Photo { get; set; }
    }
}