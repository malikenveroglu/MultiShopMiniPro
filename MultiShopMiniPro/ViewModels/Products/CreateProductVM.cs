using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MultiShopMiniPro.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Discount price is required")]
        public decimal ExPrice { get; set; }

        [Required(ErrorMessage = "Sub-title is required")]
        [MaxLength(200)]
        public string SubTitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Photo is required")]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Order is required")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }
}