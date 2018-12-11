using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Collections;

namespace SARASWATIPRESSNEW.Models
{
    public class ChallanBinderBookQtyRpt
    {
        public string ChallanNumber { get; set; }
        public DateTime ChallanDate { get; set; }
        public string Language { get; set; }
        public string TransporterName { get; set; }
        public string BinderName { get; set; }
        public int Qty { get; set; }
    }
}