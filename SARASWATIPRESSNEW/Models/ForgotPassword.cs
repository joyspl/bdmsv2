using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class ForgotPassword
    {
        [Key]
        public string CircleuserId { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        public string new_password { get; set; }

        [Required(ErrorMessage = "Enter Confirm Password")]
        public string confirm_password { get; set; }

    }
}