using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BlazorApp.Data;

namespace BlazorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized(new { error = "Invalid username or password" });

            var result = await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { error = "Invalid username or password" });
        }

        public class LoginModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; }
        }
    }
}
