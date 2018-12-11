using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class TBLogin
    {
        [Required(ErrorMessage = "Enter Saraswati Press User name")]
        public string sp_user_name { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string sp_password { get; set; }
    }
}