using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            ReadCookie();

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LogOff(string returnUrl = "/")
        {
            await HttpContext.Authentication.SignOutAsync("CookieAuthentication");

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Products(string returnUrl = "/")
        {
            return RedirectToAction("List", "Product");
        }

        private void ReadCookie()
        {
            ViewData["LogInMailAddress"] = ViewData["LogInUserName"] = string.Empty;

            ClaimsPrincipal loggedInUser = HttpContext.User;
            List<Claim> listClaim = loggedInUser.Claims.ToList();

            if (listClaim.Count > 0)
            {
                ViewData["LogInMailAddress"] = listClaim[0].Value; // mail address
                ViewData["LogInUserName"] = listClaim[2].Value; // user name
            }
        }
    }
}
