using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Enter User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Please select accadmic year")]
        public int AccadmicYear { get; set; }
    }

    public class UserObject
    {
        public int ID { get; set; }
        public double REF_ID { get; set; }
        public string USER_ID { get; set; }
        public string PASSWORD { get; set; }
        public int IsPasswordEncrypted { get; set; }
    }
}