using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelProductCategory
    {
        public int ProductCategoryId { get; set; }

        [Display(Name = "Kategori İsmi")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        [MaxLength(500)]
        public string Description { get; set; }
    }
}
