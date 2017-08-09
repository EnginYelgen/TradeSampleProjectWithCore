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

        public IActionResult Detail(int? productId, string message)
        {
            if (productId > 0)
            {
                ViewBag.Message = message;

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
            string errMess = string.Empty;

            if (productId > 0)
            {
                if (new DataService.Order(this.DbContext).AddCart(this.LogInUserId, (int)productId, ref errMess))
                {
                    return RedirectToAction("List", "Product");
                }
            }

            return RedirectToAction("Detail", "Product", new { productId = productId, message = errMess });
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
