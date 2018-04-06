using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuilderApp.ViewModel
{
    public class AccountCompanyViewModel
    {
        public int ID { get; set; }

        [StringLength(200)]
        [Display(Name = "Nazwa firmy")]
        [Required(ErrorMessage = "Nazwa firmy jest wymagana", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,200}$", ErrorMessage = "Niepoprawna nazwa firmy")]
        public string companyName { get; set; }

        [StringLength(200)]
        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{2,200}$", ErrorMessage = "Niepoprawny login")]
        public string login { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Za-z0-9_\-\.]{1,}@[a-z0-9_\.]{1,}\.[a-z]{2,5}$", ErrorMessage = "Niepoprawny email")]
        public string email { get; set; }

        [StringLength(50)]
        [Display(Name = "Numer telefonu")]
        [Required(ErrorMessage = "Numer telefonu jest wymagany", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-ząóęćłśńżźĄÓĘĆŁŚŃŻŹ!@#$%^&*()_+-=1234567890,.?~`]{6,50}$", ErrorMessage = "Niepoprawny numer telefonu")]
        public string phoneNumber { get; set; }

        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        [Required(ErrorMessage = "Hasło jest wymagane", AllowEmptyStrings = false)]
        [RegularExpression(@"^[A-Z-a-z0-9_\@\.\!]{8,16}$", ErrorMessage = "Niepoprawne hasło (co najmniej 8 znaków)")]
        public string password { get; set; }

        public int role_id { get; set; }
        public bool isDelete { get; set; }

    }
}