using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Data;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RegisterController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return Redirect("/register?error=Passwords do not match");
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Level = 2 // Default level for registered users
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }

            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Redirect($"/register?error={System.Web.HttpUtility.UrlEncode(errors)}");
        }

        public class RegisterModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}