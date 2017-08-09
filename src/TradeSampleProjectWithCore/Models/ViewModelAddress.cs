using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSampleProjectWithCore.Models
{
    public class ViewModelAddress
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }

        [Display(Name = "Kullanıcı İsmi")]
        public string UserFullName { get; set; }

        public int CityId { get; set; }

        [Display(Name = "Adres İsmi")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        [StringLength(200, ErrorMessage = "Maksimum uzunluk {0} karakterdir.")]
        public string AddressName { get; set; }

        [Display(Name = "Şehir")]
        public string CityName { get; set; }

        public int CountryId { get; set; }

        [Display(Name = "Ülke")]
        public string CountryName { get; set; }

        [Display(Name = "Numara")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        [StringLength(50, ErrorMessage = "Maksimum uzunluk {0} karakterdir.")]
        public string Number { get; set; }

        [Display(Name = "Sokak/Cadde")]
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli alan")]
        [StringLength(100, ErrorMessage = "Maksimum uzunluk {0} karakterdir.")]
        public string Street { get; set; }

        [Display(Name = "Posta Kodu")]
        [DataType(DataType.Text)]
        [StringLength(20, ErrorMessage = "Maksimum uzunluk {0} karakterdir.")]
        public string PostCode { get; set; }

        [Display(Name = "Açıklama")]
        [DataType(DataType.Text)]
        [StringLength(500, ErrorMessage = "Maksimum uzunluk {0} karakterdir.")]
        public string Description { get; set; }

        [Display(Name = "Aktif")]
        [Required(ErrorMessage = "Gerekli alan")]
        public bool InUse { get; set; }

        public bool IsDeleted { get; set; }
    }
}
