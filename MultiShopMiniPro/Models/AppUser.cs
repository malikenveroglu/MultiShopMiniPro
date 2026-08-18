using Microsoft.AspNetCore.Identity;
using MultiShopMiniPro.Utilities.Enums;

namespace MultiShopMiniPro.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public Gender Gender { get; set; }

    }
}
