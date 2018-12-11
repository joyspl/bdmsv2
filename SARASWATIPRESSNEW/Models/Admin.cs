using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SARASWATIPRESSNEW.Models
{
    public class Admin
    {
        [Required(ErrorMessage = "Enter User name")]
        public string admin_user_name { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string admin_password { get; set; }
    }
}