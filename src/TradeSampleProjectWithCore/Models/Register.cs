using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class Register
    {
        [Display(Name = "E-Posta Adresi")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Geçersiz e-posta formatı")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Email { get; set; }
        
        [Display(Name = "İsim")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Gerekli alan")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Telefon No")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalı", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Şifre(Tekrar)")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifre girilen değerle uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
