using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrnekSite.Models
{
    public class Register
    {
        [Required]
        [DisplayName("Adınız:")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Soyadınız:")]
        public string Surname { get; set; }
        [Required]
        [DisplayName("Kullanıcı Adı:")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Geçersiz Mail Adresi")]
        public string Email { get; set; }
        [Required]
        [DisplayName("Şifre:")]
        public string Passsword { get; set; }
        [Required]
        [DisplayName("Şifre Tekrar:")]
        [Compare("Passsword", ErrorMessage ="Şifreler Aynı Değil..")]
        public string RePasssword { get; set; }



    }
}