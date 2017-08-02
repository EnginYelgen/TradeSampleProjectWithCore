using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.DataService
{
    public class Order : BaseClasses.BaseService
    {
        public Order(TradeSampleContext context) : base(context) { }

        public List<ViewModelCart> GetCartItems(int userId)
        {
            int cartId = this.DbContext.ShoppingCarts.SingleOrDefault(x => x.UserId == userId && x.InUse).Id;

            if (cartId > 0)
            {
                return this.DbContext.ShoppingCartDetails
                    .Where(x => x.ShoppingCartId == cartId)
                    .Select(x => new ViewModelCart
                    {
                        ShoppingCartId = x.ShoppingCartId,
                        ShoppingCartDetailId = x.Id,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        ProductStockCount = x.Product.StockNumber,
                        ProductDescription = x.Product.Description,
                        ProductImage = x.Product.Image,
                        ProductUnitPrice = x.Product.UnitPrice,
                        NumberOfProduct = x.NumberOfProduct
                    }).ToList();
            }
            else
            {
                return new List<ViewModelCart>();
            }
        }

        public int GetCartItemCount(int userId)
        {
            int cartId = 0;

            if (this.DbContext.ShoppingCarts.Any(x => x.UserId == userId && x.InUse))
            {
                cartId = this.DbContext.ShoppingCarts.Single(x => x.UserId == userId && x.InUse).Id;
            }

            if (cartId > 0)
            {
                return this.DbContext.ShoppingCartDetails.Where(x => x.ShoppingCartId == cartId).Count();
            }
            else
            {
                return 0;
            }
        }
    }
}
