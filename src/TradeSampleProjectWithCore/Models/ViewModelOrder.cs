using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelOrder
    {
        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }

        public bool IsDelivered { get; set; }

        [Display(Name = "Sipariş Durumu")]
        public string OrderStatus { get { return IsDelivered ? "Tamamlandı" : "Devam ediyor"; } }

        [Display(Name = "Sipariş Tutarı")]
        public decimal Total { get; set; }

        public ViewModelAddress Address { get; set; }

        public IEnumerable<ViewModelCartItem> CartItemList { get; set; }
    }
}
