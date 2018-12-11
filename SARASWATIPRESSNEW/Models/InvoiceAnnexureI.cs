using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceAnnexureI
    {
        public Int64 InvoiceId { get; set; }
        public string DisplayInvoiceNo { get; set; }
        public string InvoiceCategory { get; set; }
        public string InvoiceNo { get; set; }
        public string ManualInvoiceNo { get; set; }
        public string InvoiceDate { get; set; }

        public string TaxableTotAmnt { get; set; }
        public string CGSTTotAmt { get; set; }
        public string SGSTTotAmt { get; set; }
        public string IGSTTotAmt { get; set; }
        public string SumTotAmt { get; set; }

        public string TotalTaxAmt { get; set; }
        public string RoundingOff { get; set; }
        public string AmountinWord { get; set; }
        public int PgCount { get; set; }
        public string CustomerOrderNo { get; set; }

        public List<InvoiceAnnexureIDtl> InvoiceAnnexureIDtlCollection { get; set; }

    }
}