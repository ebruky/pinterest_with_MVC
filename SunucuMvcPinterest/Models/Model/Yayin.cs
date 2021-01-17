using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SunucuMvcPinterest.Models.Model
{
    public class Yayin
    {
        [Key] public int İd { get; set; }
        [Required]
        [DisplayName("Resim Yolu")]
        public string resim_yol { get; set; }
        [Required]
        [DisplayName("İçerik")]
        public string icerik { get; set; }
        [Required]
        public DateTime tarih { get; set; }
        public int uye_Id { get; set; }
        
    }
}