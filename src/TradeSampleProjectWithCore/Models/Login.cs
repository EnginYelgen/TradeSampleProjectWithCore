using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class Login
    {
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçersiz e-posta formatı")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Email { get; set; }

        //[Display(Name = "İsim")]
        //[DataType(DataType.Text)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        //public string UserName { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Password { get; set; }
    }
}
