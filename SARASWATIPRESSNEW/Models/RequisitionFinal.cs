using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class RequisitionFinal
    {
         [Key]
        public string prev_requisitionquantity { get; set; }

         public string curr_requisitionquantity { get; set; }

    }
}