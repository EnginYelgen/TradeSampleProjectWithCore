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
    public class ProductController : Controller
    {
        private readonly TradeSampleContext _context;

        public ProductController(TradeSampleContext context)
        {
            this._context = context;
        }

        // GET: /<controller>/
        public IActionResult List()
        {
            return View(_context.Products.Select(x => new ViewModelProduct
            {
                ProductId = x.Id,
                Name = x.Name,
                StockCode = x.StockCode,
                StockNumber = x.StockNumber,
                Description = x.Description,
                Image = x.Image,
                UnitPrice = x.UnitPrice,
                ProductCategory = new ViewModelProductCategory { ProductCategoryId = x.ProductCategory.Id, Name = x.ProductCategory.Name, Description = x.ProductCategory.Description }
            }).ToList());
        }

        public IActionResult Detail(int? productId)
        {
            if (productId > 0)
            {
                return View(
                    _context.Products.Where(x => x.Id == productId).Select(x => new ViewModelProduct
                    {
                        ProductId = x.Id,
                        Name = x.Name,
                        StockCode = x.StockCode,
                        StockNumber = x.StockNumber,
                        Description = x.Description,
                        Image = x.Image,
                        UnitPrice = x.UnitPrice,
                        ProductCategory = new ViewModelProductCategory { ProductCategoryId = x.ProductCategory.Id, Name = x.ProductCategory.Name, Description = x.ProductCategory.Description }
                    }).Single());
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
