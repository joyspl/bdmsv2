using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public partial class Language
    {
        [Key]
        public int LanguageID { get; set; }

        [Required(ErrorMessage = "Enter Language")]
        public string language_name { get; set; }
    }
}