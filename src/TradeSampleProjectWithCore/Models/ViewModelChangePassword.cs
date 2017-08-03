using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelChangePassword
    {
        [Display(Name = "Eski Şifre")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalı", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name = "Yeni Şifre(Tekrar)")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifre girilen değerle uyuşmuyor")]
        public string NewConfirmPassword { get; set; }
    }
}
