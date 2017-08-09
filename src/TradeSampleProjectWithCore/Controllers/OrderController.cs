using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradeSampleProjectWithCore.Models;
using TradeSampleProjectWithCore.Common;

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

        public IActionResult CompletePurchase(string message, string messageKey)
        {
            //if (!string.IsNullOrEmpty(messageKey))
            //{
            //    //ViewData[messageKey] = message;
            //    ViewBag.Message = message;
            //}

            return View(new DataService.Order(this.DbContext).GetCompletePurchase(this.LogInUserId));
        }

        public IActionResult History()
        {
            List<ViewModelOrder> vmOrderList = new DataService.Order(this.DbContext).GetPurchaseHistory(this.LogInUserId);

            return View(vmOrderList);
        }

        public IActionResult PurchaseSuccess()
        {
            return View();
        }

        public IActionResult Delete(int? detailId)
        {
            if (detailId > 0)
            {

            }

            return RedirectToAction("Cart");
        }

        public IActionResult Next()
        {
            return RedirectToAction("CompletePurchase");
        }

        [HttpPost]
        public JsonResult Complete(List<DataPair> data)
        {
            //string errMessKey = "Message_ViewModelCompletePurchase";
            string errMess = string.Empty;
            bool isSucc = true;

            int addressId = Convert.ToInt32(data.Where(x => Convert.ToBoolean(x.Value)).Single().Key);

            List<ViewModelCartItem> vmCartItemList = new DataService.Order(this.DbContext).GetCartItems(this.LogInUserId);

            foreach (ViewModelCartItem item in vmCartItemList)
            {
                if (!(new DataService.Product(this.DbContext).StockAvailable(item.ProductId, item.NumberOfProduct)))
                {
                    errMess = "Stokta malzeme yoktur!";
                    errMess += "\n";
                    errMess += "Ürün Adı: " + item.ProductName;

                    isSucc = false;

                    break;
                }
            }

            if (isSucc)
            {
                if (data.Count > 1)
                {
                    isSucc = new DataService.Account(this.DbContext).SetActiveAddress(
                        addressId,
                        this.LogInUserId,
                        out errMess);
                }
            }

            if (isSucc)
            {
                int shoppingCartId = vmCartItemList.Take(1).Single().ShoppingCartId;

                isSucc = new DataService.Order(this.DbContext).CompletePurchase(
                    userId: this.LogInUserId,
                    order: new Order
                    {
                        AddressId = addressId,
                        ShoppingCartId = shoppingCartId,
                        Total = vmCartItemList.Sum(x => x.Total)
                    },
                    errMess: ref errMess);

                if (isSucc)
                {
                    return Json(new { status = "OK", message = "" });
                    //return RedirectToAction("PurchaseSuccess");
                }
            }

            return Json(new { status = "ERROR", message = errMess });
            //return RedirectToAction("CompletePurchase", new { message = errMess, messageKey = errMessKey });
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("List", "Product");
        }
    }
}
