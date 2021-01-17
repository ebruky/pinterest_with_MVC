using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SunucuMvcPinterest.Models.Model
{
    public class Uye
    {
        [Key] public int id { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir.")]
        [DisplayName("Kullanıcı Adı")]
        public string k_adi { get; set; }
        [Required, StringLength(10, ErrorMessage = "En fazla 10 karakter olabilir.")]
        [DisplayName("Şifre")]
        public string sifre { get; set; }
        [Required, StringLength(100, ErrorMessage = "En fazla 100 karakter olabilir.")]
        [DisplayName("Adı Soyadı")]
        public string adSoyad { get; set; }
        [Required, StringLength(11, ErrorMessage = "En fazla 11 karakter olabilir.")]
        [DisplayName("Telefon")]
        public string telefon { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir.")]
        [DisplayName("Mail Adresi")]
        public string Mail { get; set; }

        public class LoginModel
        {
            [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olabilir.")]
            [DisplayName("Kullanıcı Adı")]
            public string k_adi { get; set; }
            [Required, StringLength(10, ErrorMessage = "En fazla 10 karakter olabilir.")]

            [DisplayName("Şifre")]
            public string sifre { get; set; }
        }
    }
}