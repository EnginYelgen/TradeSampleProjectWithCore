using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelAccountDetail
    {
        public ViewModelChangePassword ChangePassword { get; set; }

        public ViewModelAddress NewAddress { get; set; }

        public IEnumerable<ViewModelAddress> AddressList { get; set; }
    }
}
