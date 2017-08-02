using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelCart
    {
        public int ShoppingCartId { get; set; }

        public int ShoppingCartDetailId { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Ürün İsmi")]
        public string ProductName { get; set; }

        [Display(Name = "Stok Sayısı")]
        public int ProductStockCount { get; set; }

        [Display(Name = "Açıklama")]
        public string ProductDescription { get; set; }

        [Display(Name = "Ürün Resmi")]
        public byte[] ProductImage { get; set; }

        [Display(Name = "Birim Fiyat")]
        public decimal ProductUnitPrice { get; set; }

        [Display(Name = "Sipariş Adeti")]
        public int NumberOfProduct { get; set; }
    }
}
