using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeSampleProjectWithCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    [Authorize]
    public class ProductController : BaseClasses.BaseController
    {
        public ProductController(TradeSampleContext context) : base(context) { }

        // GET: /<controller>/
        public IActionResult List()
        {
            return View(new DataService.Product(this.DbContext).GetProducts());
        }

        public IActionResult Detail(int? productId)
        {
            if (productId > 0)
            {
                return View(new DataService.Product(this.DbContext).GetProducts(productId).Single());
            }
            else
            {
                return RedirectToAction("List", "Product");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart(int? productId)
        {
            return RedirectToAction("Detail", "Product", new { productId = productId });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel()
        {
            return RedirectToAction("List", "Product");
        }
    }
}
