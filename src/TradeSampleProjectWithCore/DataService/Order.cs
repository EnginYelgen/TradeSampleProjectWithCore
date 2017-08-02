using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;
using Microsoft.EntityFrameworkCore;

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
                return this.DbContext.ShoppingCartDetails.Where(x => x.ShoppingCartId == cartId).Sum(x => x.NumberOfProduct);
            }
            else
            {
                return 0;
            }
        }

        public bool AddCart(int userId, int productId, ref string errMess)
        {
            bool isSucc = true;
            errMess = string.Empty;

            try
            {
                int stockNumber = 0;

                if (!this.DbContext.Products.Any(x => x.Id == productId && x.InUse && x.StockNumber > 0))
                {
                    errMess = "Stokta malzeme yoktur!";
                    return false;
                }
                else
                {
                    stockNumber = this.DbContext.Products.Single(x => x.Id == productId).StockNumber;
                }

                ShoppingCart shoppingCart;

                if (this.DbContext.ShoppingCarts.Any(x => x.UserId == userId && x.InUse))
                {
                    shoppingCart = this.DbContext.ShoppingCarts.Include(x => x.ShoppingCartDetails).Single(x => x.UserId == userId && x.InUse);

                    if (shoppingCart.ShoppingCartDetails.Where(x => x.ProductId == productId).Sum(x => x.NumberOfProduct) >= stockNumber)
                    {
                        errMess = "Stokta malzeme yoktur!";
                        return false;
                    }
                }
                else
                {
                    shoppingCart = new ShoppingCart
                    {
                        CreateDate = DateTime.Now,
                        InUse = true,
                        UpdateDate = DateTime.Now,
                        UpdateUserId = userId,
                        UserId = userId
                    };

                    this.DbContext.ShoppingCarts.Add(shoppingCart);
                    this.DbContext.SaveChanges();

                    shoppingCart = this.DbContext.ShoppingCarts.Include(x => x.ShoppingCartDetails).Single(x => x.Id == shoppingCart.Id);
                }

                if (shoppingCart.ShoppingCartDetails.Any(x => x.ProductId == productId))
                {
                    ShoppingCartDetail shoppingCartDetail = shoppingCart.ShoppingCartDetails.Single(x => x.ProductId == productId);

                    shoppingCartDetail.NumberOfProduct = shoppingCartDetail.NumberOfProduct + 1;
                }
                else
                {
                    shoppingCart.ShoppingCartDetails.Add(new ShoppingCartDetail
                    {
                        InUse = true,
                        NumberOfProduct = 1,
                        ProductId = productId,
                        UpdateDate = DateTime.Now,
                        UpdateUserId = userId
                    });
                }

                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                errMess = "Beklenmeyen hata!";
                isSucc = false;
            }

            return isSucc;
        }
    }
}
