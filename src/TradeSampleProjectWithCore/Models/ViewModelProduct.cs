using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelProduct
    {
        public int ProductId { get; set; }

        [Display(Name = "Ürün İsmi")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Name { get; set; }

        [Display(Name = "Stok Kodu")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string StockCode { get; set; }

        [Display(Name = "Stok Sayısı")]
        [Required(ErrorMessage = "Gerekli alan")]
        public int StockNumber { get; set; }

        [Display(Name = "Açıklama")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "Ürün Resmi")]
        public byte[] Image { get; set; }

        [Display(Name = "Birim Fiyat")]
        [Required(ErrorMessage = "Gerekli alan")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Gerekli alan")]
        public ViewModelProductCategory ProductCategory { get; set; }
    }
}
