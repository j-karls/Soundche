using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Soundche.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Login { get; set; }
        public string Message { get; set; }

        private readonly ILogger<IndexModel> _logger;

        private readonly IConfiguration configuration;
        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            this.configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost()
        {
            var users = configuration.GetSection("SiteUsers").Get<List<string>>();

            Login = Request.Form["Login"];

            if (users.Contains(Login))
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Login)
                    };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToPage("/VideoPlayer");

            }
            Message = "Invalid attempt";
            return Page();
        }
    }
}
