using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class BinderWiseBookQtyRpt
    {
        public int BinderID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public List<ChallanRemarks> ChallanRemarksCollection { get; set; }
    }
}