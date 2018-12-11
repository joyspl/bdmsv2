using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SARASWATIPRESSNEW.Models
{
    public class Directorate
    {
        [Required(ErrorMessage = "Enter User name")]
        public string directorate_user_name { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string directorate_password { get; set; }
    }
}