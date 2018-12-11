using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class District
    {

        [Key]
        public int DistrictID { get; set; }
        [Required(ErrorMessage = "Enter District")]
        public string District_name { get; set; }

    }
    public class ReqYears
    {        
        public string ReqYear { get; set; }

    }
}