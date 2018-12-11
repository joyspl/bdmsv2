using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SARASWATIPRESSNEW.Models
{
    public class InvoiceAnnexureIDtl
    {
        public string SlNo { get; set; }
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string BookNameDesc { get; set; }
        public string Language { get; set; }
        public string HsnSac { get; set; }
        public double Qty { get; set; }
        public string UQC { get; set; }
        public string Rate { get; set; }
        public string TaxableAmnt { get; set; }
        public string CGSTRate { get; set; }
        public string CGSTAmt { get; set; }
        public string SGSTRate { get; set; }
        public string SGSTAmt { get; set; }
        public string IGSTRate { get; set; }
        public string IGSTAmt { get; set; }
        public string TotalAmt { get; set; }


    }
}