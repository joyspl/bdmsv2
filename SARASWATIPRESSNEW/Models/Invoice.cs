using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class Invoice
    {
        public Int64 InvoiceId { get; set; }
        public int CategoryId { get; set; }
        public string ChallanNo { get; set; }
        public Int64 ChallanId { get; set; }
        public string InvoiceNo { get; set; }
        public string ManualInvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string UserId { get; set; }
        public string UpdateCode { get; set; }
        public string UpdateMsg { get; set; }
        public string SaveStatus { get; set; }

        public int DistrictId { get; set; }       
        public int CircleId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public List<InvoiceChallanDtl> InvoiceChallanDtlCollection { get; set; }
    }
}