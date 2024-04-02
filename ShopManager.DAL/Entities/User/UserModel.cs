using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopManager.DAL.Entities.User
{
    public class UserModel : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public string MiddleName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public bool IsManager {  get; set; }
    }

}
