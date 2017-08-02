using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.DataService
{
    public class Product : BaseClasses.BaseService
    {
        public Product(TradeSampleContext context) : base(context) { }

        public List<ViewModelProduct> GetProducts(int? productId = null)
        {
            return this.DbContext.Products
                .Where(x => (!productId.HasValue || x.Id == productId))
                .Select(x => new ViewModelProduct
                {
                    ProductId = x.Id,
                    Name = x.Name,
                    StockCode = x.StockCode,
                    StockNumber = x.StockNumber,
                    Description = x.Description,
                    Image = x.Image,
                    UnitPrice = x.UnitPrice,
                    ProductCategory = new ViewModelProductCategory { ProductCategoryId = x.ProductCategory.Id, Name = x.ProductCategory.Name, Description = x.ProductCategory.Description }
                })
                .ToList();
        }
    }
}
