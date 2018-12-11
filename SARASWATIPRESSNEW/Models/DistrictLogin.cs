using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SARASWATIPRESSNEW.Models
{
    public class DistrictLogin
    {
        [Required(ErrorMessage = "Enter User name")]
        public string district_user_name { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string district_password { get; set; }

        public string district_name { get; set; }
        public string district_id { get; set; }

    }
}