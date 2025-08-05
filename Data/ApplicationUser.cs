// ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int Level { get; set; } // 0 = Admin, 1 = Manager, 2 = User
    }
}