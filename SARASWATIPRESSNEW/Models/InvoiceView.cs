using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceView
    {

        public Int64 InvoiceId { get; set; }
        public string CategoryName { get; set; }        
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }
        public string Save_Status { get; set; }
        public string UpdatedTimeStamp { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}