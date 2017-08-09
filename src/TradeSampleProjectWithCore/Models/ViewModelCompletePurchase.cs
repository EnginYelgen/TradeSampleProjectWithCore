using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelCompletePurchase
    {
        public IEnumerable<ViewModelCartItem> CartItemList { get; set; }

        public IEnumerable<ViewModelAddress> AddressList { get; set; }
    }
}
