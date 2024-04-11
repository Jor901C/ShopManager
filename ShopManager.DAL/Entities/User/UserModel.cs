using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopManager.DAL.Entities.User
{
    //Class for storing Administrator and Manager data
    public class UserModel : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string MiddleName { get; set; } = string.Empty;

        // //Mark for deletion.
        public bool IsDeleted { get; set; }

        //The role of Manager
        public bool IsManager {  get; set; }
    }

}
