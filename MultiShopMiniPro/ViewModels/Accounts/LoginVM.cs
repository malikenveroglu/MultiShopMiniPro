using System.ComponentModel.DataAnnotations;

namespace MultiShopMiniPro.ViewModels
{
    public class LoginVM
    {
        [MaxLength(256)]
        public string EmailOrUsername { get; set; }

        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersisted { get; set; }
    }
}
