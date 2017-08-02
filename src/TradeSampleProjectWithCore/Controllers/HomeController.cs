using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TradeSampleProjectWithCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    [Authorize]
    public class HomeController : BaseClasses.BaseController
    {
        public HomeController(TradeSampleContext context) : base(context) { }

        public IActionResult About()
        {
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Cart(string returnUrl = "/")
        {
            if (this.ShoppingItemCount > 0)
            {
                return RedirectToAction("Cart", "Order");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}
