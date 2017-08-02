﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeSampleProjectWithCore.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TradeSampleProjectWithCore.Controllers
{
    public class OrderController : BaseClasses.BaseController
    {
        public OrderController(TradeSampleContext context) : base(context) { }

        public IActionResult Cart()
        {
            return View(new DataService.Order(this.DbContext).GetCartItems(this.LogInUserId));
        }

        public IActionResult Delete(int? detailId)
        {
            if (detailId > 0)
            {
                
            }

            return RedirectToAction("Cart");
        }

        public IActionResult CompletePurchase()
        {

            return RedirectToAction("Cart");
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("List", "Product");
        }
    }
}