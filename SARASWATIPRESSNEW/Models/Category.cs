using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Enter Category")]
        public string Category_name { get; set; }
    }
}