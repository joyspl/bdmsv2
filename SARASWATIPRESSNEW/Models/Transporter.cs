using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class Transporter
    {
        [Key]
        public int TransporterID { get; set; }

      
        public string Transporter_name { get; set; }

      
        public string Transporter_address { get; set; }
        public string Transporter_phone_no { get; set; }

    }
}